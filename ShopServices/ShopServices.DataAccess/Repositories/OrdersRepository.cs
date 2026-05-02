using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;

namespace ShopServices.DataAccess.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly ShopServicesDbContext _dbContext;

        public OrdersRepository(ShopServicesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Create(Order newOrder)
        {
            ArgumentNullException.ThrowIfNull(newOrder);

            var newOrderEntity = new Entities.Order
            (
                id: 0,
                buyerId: newOrder.BuyerId,
                cost: 0m, //стоимость укажем после успешного добавления товарных позиций
                currency: newOrder.Currency,
                created: newOrder.Created,
                plannedDeliveryTime: newOrder.PlannedDeliveryTime,
                paymentInfo: newOrder.PaymentInfo,
                deliveryAddress: newOrder.DeliveryAddress,
                shopId: newOrder.ShopId,
                extraInfo: newOrder.ExtraInfo,
                archieved: false,
                massInGrams: newOrder.MassInGrams,
                dimensions: newOrder.Dimensions
            );

            await _dbContext.Orders.AddAsync(newOrderEntity);
            await _dbContext.SaveChangesAsync();
            
            await _dbContext.Entry(newOrderEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id заказа

            var orderPositionsEntities = from op in newOrder.Positions
                                         select new Entities.OrderPosition(
                                         id: 0,
                                         orderId: newOrderEntity.Id,
                                         productId: op.ProductId,
                                         articleNumber: op.ArticleNumber,
                                         brand: op.Brand,
                                         name: op.Name,
                                         @params: op.Params,
                                         price: op.Price,
                                         quantity: op.Quantity,
                                         cost: op.Cost,
                                         currency: op.Currency);

            ArgumentNullException.ThrowIfNull(orderPositionsEntities);

            await _dbContext.OrderPositions.AddRangeAsync(orderPositionsEntities);

            newOrderEntity.SetCost(newOrder.Cost);
            await _dbContext.SaveChangesAsync();

            return new Result
            {
                Id = newOrderEntity.Id,
                BuyerId = newOrderEntity.BuyerId,
                StatusCode = HttpStatusCode.Created,
                Message = ResultMessager.OK
            };
        }

        public async Task<Order?> GetOrderInfoById(uint orderId)
        {
            var orderEntity = await GetOrderEntity(orderId, asNoTracking: true);
            if (orderEntity is null)
                return null;

            return GetCoreOrder(orderEntity);
        }

        public async Task<Result> CancelOrderByBuyer(
            uint buyerIdFromRequest,
            uint orderId,
            string confirmationString,
            string comment)
        {
            var orderEntity = await GetOrderEntity(orderId, asNoTracking: false);
            if (orderEntity is null)
                return new Result(ResultMessager.ORDER_NOT_FOUND, HttpStatusCode.NotFound);

            if (orderEntity.BuyerId != buyerIdFromRequest)
                return new Result(ResultMessager.OTHER_BUYERS_ORDER, HttpStatusCode.Forbidden);

            if (!string.Equals(confirmationString, $"{orderEntity.BuyerId}/CANCEL{orderEntity.Id}"))
                return new Result(ResultMessager.CONFIRMATION_STRING_MISMATCH, HttpStatusCode.Forbidden);

            if (!orderEntity.Archieved)
                orderEntity.Cancel();

            if (_dbContext.ChangeTracker.HasChanges())
            {
                comment = $"CANCELED BY BUYER|{comment}";
                orderEntity.UpdateComment(comment);
                orderEntity.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());

                await _dbContext.SaveChangesAsync();

                return new Result(ResultMessager.ORDER_CANCELED, HttpStatusCode.OK, buyerId: orderEntity.BuyerId);
            }
            return new Result(ResultMessager.ORDER_CANCELED_EARLIER, HttpStatusCode.OK, buyerId: orderEntity.BuyerId);
        }

        public async Task<Result> CancelOrderByManager(
            uint managerId,
            uint orderId,
            string confirmationString,
            string comment)
        {
            var orderEntity = await GetOrderEntity(orderId, asNoTracking: false);
            if (orderEntity is null)
                return new Result(ResultMessager.ORDER_NOT_FOUND, HttpStatusCode.NotFound);

            if (!string.Equals(confirmationString, $"{managerId}/CANCEL/BUYER{orderEntity.BuyerId}/ORDER{orderEntity.Id}"))
                return new Result(ResultMessager.CONFIRMATION_STRING_MISMATCH, HttpStatusCode.Forbidden);

            if (!orderEntity.Archieved)
                orderEntity.Cancel();

            if (orderEntity.ManagerId != managerId)
                orderEntity.UpdateManagerId(managerId);

            if (_dbContext.ChangeTracker.HasChanges())
            {
                comment = $"CANCELED BY MANAGER|{comment}";
                orderEntity.UpdateComment(comment);
                orderEntity.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.ORDER_CANCELED, HttpStatusCode.OK, buyerId: orderEntity.BuyerId);
            }
            return new Result(ResultMessager.ORDER_CANCELED_EARLIER, HttpStatusCode.OK, buyerId: orderEntity.BuyerId);
        }

        public async Task<Result> ConfirmOrderByBuyer(
            uint buyerIdFromRequest,
            uint orderId,
            string confirmationString)
        {
            var orderEntity = await GetOrderEntity(orderId, asNoTracking: false);
            if (orderEntity is null)
                return new Result(ResultMessager.ORDER_NOT_FOUND, HttpStatusCode.NotFound);

            if (orderEntity.BuyerId != buyerIdFromRequest)
                return new Result(ResultMessager.OTHER_BUYERS_ORDER, HttpStatusCode.Forbidden);

            if (!string.Equals(confirmationString, $"{orderEntity.BuyerId}/{orderEntity.Id}"))
                return new Result(ResultMessager.CONFIRMATION_STRING_MISMATCH, HttpStatusCode.Forbidden);

            if (orderEntity.Archieved)
                return new Result(ResultMessager.ORDER_CANCELED_EARLIER, HttpStatusCode.Conflict);

            if (orderEntity.Received != null)
                return new Result(ResultMessager.ORDER_RECEIVED_EARLIER, HttpStatusCode.Conflict);

            if (orderEntity.Delivered != null)
                return new Result(ResultMessager.ORDER_DELIVERED_EARLIER, HttpStatusCode.Conflict);

            var comment = $"CONFIRMED";
            orderEntity.UpdateComment(comment);
            orderEntity.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.ORDER_CONFIRMED, HttpStatusCode.OK);
            }
            return new Result(ResultMessager.ORDER_CONFIRMED_EARLIER, HttpStatusCode.OK);
        }

        public async Task<Result> MarkAsDeliveredToShop(
            uint managerId,
            uint orderId,
            string comment)
        {
            var orderEntity = await GetOrderEntity(orderId, asNoTracking: false);
            if (orderEntity is null)
                return new Result(ResultMessager.ORDER_NOT_FOUND, HttpStatusCode.NotFound);

            if (orderEntity.Archieved)
                return new Result(ResultMessager.ERROR_ORDER_CANCELED_EARLIER_OR_DELIVERED, HttpStatusCode.Conflict, buyerId: orderEntity.BuyerId);

            if (orderEntity.Received != null)
                return new Result(ResultMessager.ORDER_RECEIVED_EARLIER, HttpStatusCode.Conflict, buyerId: orderEntity.BuyerId);

            if (orderEntity.ManagerId != managerId)
                orderEntity.UpdateManagerId(managerId);

            comment = $"DELIVERED TO SHOP|{comment}";
            if (!string.Equals(comment, orderEntity.Comment))
                orderEntity.UpdateComment(comment);

            var datetime = DateTime.Now.ToUniversalTime();
            if (orderEntity.Delivered == null)
                orderEntity.UpdateDelivered(datetime);

            if (_dbContext.ChangeTracker.HasChanges())
            {
                orderEntity.UpdateUpdatedDt(datetime);
                await _dbContext.SaveChangesAsync();
            }

            if (orderEntity.Archieved) //отмена заказа не помешала его быстрой доставке в магазин
                return new Result(ResultMessager.ORDER_CANCELED_EARLIER_DELIVERED_TO_SHOP, HttpStatusCode.OK, buyerId: orderEntity.BuyerId);

            return new Result(ResultMessager.OK, HttpStatusCode.OK, buyerId: orderEntity.BuyerId);
        }

        public async Task<Result> MarkAsReceived(
            uint orderId,
            string comment,
            uint? managerId,
            uint? courierId)
        {
            var orderEntity = await GetOrderEntity(orderId, asNoTracking: false);
            if (orderEntity is null)
                return new Result(ResultMessager.ORDER_NOT_FOUND, HttpStatusCode.NotFound);

            if (orderEntity.Archieved)
                return new Result(ResultMessager.ERROR_ORDER_CANCELED_EARLIER_OR_RECEIVED, HttpStatusCode.Conflict);

            if (managerId != null && orderEntity.ManagerId != managerId)
                orderEntity.UpdateManagerId(managerId);

            if (courierId != null && orderEntity.CourierId != courierId)
                orderEntity.UpdateCourierId(courierId);

            var datetime = DateTime.Now.ToUniversalTime();

            if (orderEntity.Received == null)
            {
                orderEntity.UpdateReceived(datetime);
                comment = $"RECEIVED BY BUYER|{comment}";
                if (!string.Equals(comment, orderEntity.Comment))
                    orderEntity.UpdateComment(comment);
            }

            if (_dbContext.ChangeTracker.HasChanges())
            {
                orderEntity.UpdateUpdatedDt(datetime);
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.OK, HttpStatusCode.OK);
            }
            return new Result(ResultMessager.ORDER_RECEIVED_EARLIER, HttpStatusCode.OK);
        }

        public async Task<Result> UpdateCourierId(
            uint orderId,
            uint? courierId,
            string comment)
        {
            var orderEntity = await GetOrderEntity(orderId, asNoTracking: false);

            if (orderEntity is null)
                return new Result(ResultMessager.ORDER_NOT_FOUND, HttpStatusCode.NotFound);

            if (courierId != null)
            {
                if (orderEntity.Archieved)
                    return new Result(ResultMessager.ORDER_CANCELED_EARLIER, HttpStatusCode.Conflict);

                if (orderEntity.Received != null)
                    return new Result(ResultMessager.ORDER_RECEIVED_EARLIER, HttpStatusCode.Conflict);

                if (orderEntity.Delivered != null)
                    return new Result(ResultMessager.ORDER_DELIVERED_EARLIER, HttpStatusCode.Conflict);
            }

            if (orderEntity.CourierId != courierId)
            {
                orderEntity.UpdateCourierId(courierId);

                comment = comment ?? $"CourierId updated|{comment}";

                if (!string.Equals(comment, orderEntity.Comment))
                    orderEntity.UpdateComment(comment);
            }

            if (_dbContext.ChangeTracker.HasChanges())
            {
                orderEntity.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());

                await _dbContext.SaveChangesAsync();

                return new Result(ResultMessager.OK, HttpStatusCode.OK);
            }
            
            return new Result(ResultMessager.ORDER_IS_ACTUAL, HttpStatusCode.OK);
        }

        public async Task<Result> UpdateDeliveryAddressByBuyer(
            uint buyerIdFromRequest,
            uint orderId,
            string deliveryAddress)
        {
            var orderEntity = await GetOrderEntity(orderId, asNoTracking: false);

            if (orderEntity is null)
                return new Result(ResultMessager.ORDER_NOT_FOUND, HttpStatusCode.NotFound);

            if (orderEntity.Archieved)
                return new Result(ResultMessager.ORDER_CANCELED_EARLIER, HttpStatusCode.Conflict);

            if (orderEntity.Received != null)
                return new Result(ResultMessager.ORDER_RECEIVED_EARLIER, HttpStatusCode.Conflict);

            if (orderEntity.Delivered != null)
                return new Result(ResultMessager.ORDER_DELIVERED_EARLIER, HttpStatusCode.Conflict);

            if (!string.Equals(deliveryAddress, orderEntity.DeliveryAddress))
            {
                orderEntity.UpdateDeliveryAddress(deliveryAddress);

                var comment = "DeliveryAddress upd.";

                if (!string.Equals(comment, orderEntity.Comment))
                    orderEntity.UpdateComment(comment);
            }

            if (_dbContext.ChangeTracker.HasChanges())
            {
                orderEntity.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());

                await _dbContext.SaveChangesAsync();

                return new Result(ResultMessager.OK, HttpStatusCode.OK);
            }

            return new Result(ResultMessager.ORDER_IS_ACTUAL, HttpStatusCode.OK);
        }

        public async Task<Result> UpdateDeliveryId(
            uint orderId,
            uint? deliveryId,
            uint? managerId,
            uint? courierId,
            string comment)
        {
            var orderEntity = await GetOrderEntity(orderId, asNoTracking: false);
            if (orderEntity is null)
                return new Result(ResultMessager.ORDER_NOT_FOUND, HttpStatusCode.NotFound);

            #region Проверки не требуются (можно актуализировать данные)
            //if (orderEntity.Received != null)
            //    return new Result(ResultMessager.ORDER_RECEIVED_EARLIER, HttpStatusCode.Conflict);
            #endregion Проверки не требуются (можно актуализировать данные)

            if (managerId != null && orderEntity.ManagerId != managerId)
                orderEntity.UpdateManagerId(managerId);

            if (courierId != null && orderEntity.CourierId != courierId)
                orderEntity.UpdateCourierId(courierId);

            comment = $"DeliveryId|{comment}";
            if (!string.Equals(comment, orderEntity.Comment))
                orderEntity.UpdateComment(comment);

            if (deliveryId != orderEntity.DeliveryId)
                orderEntity.UpdateDeliveryId(deliveryId);

            if (_dbContext.ChangeTracker.HasChanges())
            {
                orderEntity.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.OK, HttpStatusCode.OK);
            }
            return new Result(ResultMessager.ORDER_DELIVERED_EARLIER, HttpStatusCode.OK);
        }

        public async Task<Result> UpdateExtraInfoByBuyer(
            uint buyerIdFromRequest,
            uint orderId,
            string extraInfo)
        {
            var orderEntity = await GetOrderEntity(orderId, asNoTracking: false);

            if (orderEntity is null)
                return new Result(ResultMessager.ORDER_NOT_FOUND, HttpStatusCode.NotFound);

            if (orderEntity.Archieved)
                return new Result(ResultMessager.ORDER_CANCELED_EARLIER, HttpStatusCode.Conflict);

            if (orderEntity.Received != null)
                return new Result(ResultMessager.ORDER_RECEIVED_EARLIER, HttpStatusCode.Conflict);

            if (orderEntity.Delivered != null)
                return new Result(ResultMessager.ORDER_DELIVERED_EARLIER, HttpStatusCode.Conflict);

            if (!string.Equals(extraInfo, orderEntity.ExtraInfo))
            {
                orderEntity.UpdateExtraInfo(extraInfo);

                var comment = "ExtraInfo upd.";

                if (!string.Equals(comment, orderEntity.Comment))
                    orderEntity.UpdateComment(comment);
            }

            if (_dbContext.ChangeTracker.HasChanges())
            {
                orderEntity.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());

                await _dbContext.SaveChangesAsync();

                return new Result(ResultMessager.OK, HttpStatusCode.OK);
            }

            return new Result(ResultMessager.ORDER_IS_ACTUAL, HttpStatusCode.OK);
        }

        public async Task<Result> UpdateManagerId(
            uint managerId,
            uint orderId,
            string comment)
        {
            var orderEntity = await GetOrderEntity(orderId, asNoTracking: false);

            if (orderEntity is null)
                return new Result(ResultMessager.ORDER_NOT_FOUND, HttpStatusCode.NotFound);

            if (orderEntity.Archieved)
                return new Result(ResultMessager.ORDER_CANCELED_EARLIER, HttpStatusCode.Conflict);

            if (orderEntity.Received != null)
                return new Result(ResultMessager.ORDER_RECEIVED_EARLIER, HttpStatusCode.Conflict);

            if (orderEntity.Delivered != null)
                return new Result(ResultMessager.ORDER_DELIVERED_EARLIER, HttpStatusCode.Conflict);

            if (orderEntity.ManagerId != managerId)
            {
                orderEntity.UpdateManagerId(managerId);

                comment = $"ManagerId updated|{comment}";

                if (!string.Equals(comment, orderEntity.Comment))
                    orderEntity.UpdateComment(comment);
            }

            if (_dbContext.ChangeTracker.HasChanges())
            {
                orderEntity.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());

                await _dbContext.SaveChangesAsync();

                return new Result(ResultMessager.OK, HttpStatusCode.OK);
            }

            return new Result(ResultMessager.ORDER_IS_ACTUAL, HttpStatusCode.OK);
        }

        public async Task<Result> UpdateMassInGramsDimensions(
            uint orderId,
            uint massInGrams,
            string dimensions,
            uint? managerId,
            uint? courierId,
            string comment,
            uint? deliveryId)
        {
            var orderEntity = await GetOrderEntity(orderId, asNoTracking: false);
            if (orderEntity is null)
                return new Result(ResultMessager.ORDER_NOT_FOUND, HttpStatusCode.NotFound);

            if (orderEntity.Archieved)
                return new Result(ResultMessager.ORDER_CANCELED_EARLIER, HttpStatusCode.Conflict);

            if (orderEntity.Received != null)
                return new Result(ResultMessager.ORDER_RECEIVED_EARLIER, HttpStatusCode.Conflict);

            #region //доставка в магазин не мешает уточнить массу и габариты
            //if (orderEntity.Delivered != null)
            //    return new Result(ResultMessager.ORDER_DELIVERED_EARLIER, HttpStatusCode.Conflict);
            #endregion //доставка в магазин не мешает уточнить массу и габариты

            if (managerId != null && orderEntity.ManagerId != managerId)
                orderEntity.UpdateManagerId(managerId);

            if (courierId != null && orderEntity.CourierId != courierId)
                orderEntity.UpdateCourierId(courierId);

            if (massInGrams != orderEntity.MassInGrams)
                orderEntity.UpdateMassInGrams(massInGrams);

            if (!string.Equals(dimensions, orderEntity.Dimensions))
                orderEntity.UpdateDimensions(dimensions);

            comment = $"massInGrams/dimensions upd.|{comment}";
            if (!string.Equals(comment, orderEntity.Comment))
                orderEntity.UpdateComment(comment);

            if (_dbContext.ChangeTracker.HasChanges())
            {
                orderEntity.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());
                
                await _dbContext.SaveChangesAsync();

                return new Result(ResultMessager.OK, HttpStatusCode.OK);
            }
            return new Result(ResultMessager.ORDER_IS_ACTUAL, HttpStatusCode.OK);
        }

        public async Task<Result> UpdatePaymentInfo(
            uint orderId,
            string paymentInfo,
            string comment,
            uint? managerId,
            uint? courierId)
        {
            var orderEntity = await GetOrderEntity(orderId, asNoTracking: false);
            if (orderEntity is null)
                return new Result(ResultMessager.ORDER_NOT_FOUND, HttpStatusCode.NotFound);

            if (managerId != null && orderEntity.ManagerId != managerId)
                orderEntity.UpdateManagerId(managerId);

            if (courierId != null && orderEntity.CourierId != courierId)
                orderEntity.UpdateCourierId(courierId);

            if (!string.Equals(paymentInfo, orderEntity.PaymentInfo))
                orderEntity.UpdatePaymentInfo(paymentInfo);

            comment = $"PaymentInfo|{comment}";
            if (!string.Equals(comment, orderEntity.Comment))
                orderEntity.UpdateComment(comment);

            if (_dbContext.ChangeTracker.HasChanges())
            {
                orderEntity.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());

                await _dbContext.SaveChangesAsync();

                return new Result(ResultMessager.OK, HttpStatusCode.OK);
            }
            return new Result(ResultMessager.ORDER_IS_ACTUAL, HttpStatusCode.OK);
        }

        public async Task<Result> UpdatePlannedDeliveryTimeByManager(
            uint orderId,
            DateTime plannedDeliveryTime,
            uint managerId,
            string comment)
        {
            var orderEntity = await GetOrderEntity(orderId, asNoTracking: false);
            if (orderEntity is null)
                return new Result(ResultMessager.ORDER_NOT_FOUND, HttpStatusCode.NotFound);

            if (orderEntity.ManagerId != managerId)
                orderEntity.UpdateManagerId(managerId);

            if (orderEntity.PlannedDeliveryTime.ToLocalTime() != plannedDeliveryTime)
                orderEntity.UpdatePlannedDeliveryTime(plannedDeliveryTime);

            comment = $"PlannedDeliveryTime|{comment}";
            if (!string.Equals(comment, orderEntity.Comment))
                orderEntity.UpdateComment(comment);

            if (_dbContext.ChangeTracker.HasChanges())
            {
                orderEntity.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());

                await _dbContext.SaveChangesAsync();

                return new Result(ResultMessager.OK, HttpStatusCode.OK);
            }
            return new Result(ResultMessager.ORDER_IS_ACTUAL, HttpStatusCode.OK);
        }

        public async Task<Result> UpdateShopIdByBuyer(
            uint buyerId,
            uint orderId,
            uint? shopId)
        {
            var orderEntity = await GetOrderEntity(orderId, asNoTracking: false);

            if (orderEntity is null)
                return new Result(ResultMessager.ORDER_NOT_FOUND, HttpStatusCode.NotFound);

            if (orderEntity.Archieved)
                return new Result(ResultMessager.ORDER_CANCELED_EARLIER, HttpStatusCode.Conflict);

            if (orderEntity.Received != null)
                return new Result(ResultMessager.ORDER_RECEIVED_EARLIER, HttpStatusCode.Conflict);

            if (orderEntity.Delivered != null)
                return new Result(ResultMessager.ORDER_DELIVERED_EARLIER, HttpStatusCode.Conflict);

            if (shopId != orderEntity.ShopId)
            {
                if (shopId != null)
                {
                    var shopEntity = _dbContext.Shops.AsNoTracking().Where(shop => shop.Id == shopId).SingleOrDefaultAsync();
                    if (shopId is null)
                        return new Result(ResultMessager.SHOP_NOT_FOUND, HttpStatusCode.NotFound);
                }

                orderEntity.UpdateShopId(shopId);

                var comment = "ShopId upd.";

                if (!string.Equals(comment, orderEntity.Comment))
                    orderEntity.UpdateComment(comment);
            }

            if (_dbContext.ChangeTracker.HasChanges())
            {
                orderEntity.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());

                await _dbContext.SaveChangesAsync();

                return new Result(ResultMessager.OK, HttpStatusCode.OK);
            }

            return new Result(ResultMessager.ORDER_IS_ACTUAL, HttpStatusCode.OK);
        }

        public async Task<IEnumerable<Order>> GetOrdersByBuyerId(
            uint buyerId,
            DateTime createdFromDt,
            DateTime? createdToDt,
            uint take,
            uint skipCount)
        {
            List<Entities.Order> entitiesOrders = await GetIQueryableOrdersByByyer(buyerId, createdFromDt, createdToDt)
                .Skip((int)skipCount)
                .Take((int)take)
                .ToListAsync();

            return entitiesOrders.Select(order => GetCoreOrder(order));
        }

        public async Task<Result> MarkAsDeliveredToBuyer(
            uint orderId,
            string comment,
            uint courierId)
        {
            var orderEntity = await GetOrderEntity(orderId, asNoTracking: false);
            if (orderEntity is null)
                return new Result(ResultMessager.ORDER_NOT_FOUND, HttpStatusCode.NotFound);

            if (orderEntity.Archieved)
                return new Result(ResultMessager.ERROR_ORDER_CANCELED_EARLIER_OR_DELIVERED, HttpStatusCode.Conflict, buyerId: orderEntity.BuyerId);

            if (orderEntity.Received != null)
                return new Result(ResultMessager.ORDER_RECEIVED_EARLIER, HttpStatusCode.Conflict, buyerId: orderEntity.BuyerId);

            if (orderEntity.CourierId != courierId)
                orderEntity.UpdateCourierId(courierId);

            comment = $"DELIVERED TO BUYER|{comment}";
            if (!string.Equals(comment, orderEntity.Comment))
                orderEntity.UpdateComment(comment);

            var datetime = DateTime.Now.ToUniversalTime();
            if (orderEntity.Delivered == null)
                orderEntity.UpdateDelivered(datetime);

            if (_dbContext.ChangeTracker.HasChanges())
            {
                orderEntity.UpdateUpdatedDt(datetime);
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.OK, HttpStatusCode.OK, buyerId: orderEntity.BuyerId);
            }
            return new Result(ResultMessager.ORDER_DELIVERED_EARLIER, HttpStatusCode.OK, buyerId: orderEntity.BuyerId);
        }

        /// <summary> Маппер Entities.Order - Core.Models.Order </summary>
        /// <param name="orderEntity"> Entities.Order - заказ (из БД) </param>
        /// <returns> Core.Models.Order - заказ </returns>
        private static Order GetCoreOrder(Entities.Order orderEntity)
        {
            var coreOrderPositions = from op in orderEntity.Positions
                                     select new OrderPosition
                                     (
                                         id: op.Id,
                                         productId: op.ProductId,
                                         articleNumber: op.ArticleNumber,
                                         brand: op.Brand,
                                         name: op.Name,
                                         @params: op.Params,
                                         price: op.Price,
                                         quantity: op.Quantity,
                                         cost: op.Cost,
                                         currency: op.Currency
                                     );


            return new Order(
                id: orderEntity.Id,
                buyerId: orderEntity.BuyerId,
                buyer: null, //вывод данных покупателя в самом заказе не нужен
                positions: coreOrderPositions.ToList(),
                cost: orderEntity.Cost,
                currency: orderEntity.Currency,
                created: orderEntity.Created,
                plannedDeliveryTime: orderEntity.PlannedDeliveryTime,
                paymentInfo: orderEntity.PaymentInfo,
                deliveryAddress: orderEntity.DeliveryAddress,
                shopId: orderEntity.ShopId,
                extraInfo: orderEntity.ExtraInfo,
                archived: orderEntity.Archieved,
                massInGrams: orderEntity.MassInGrams,
                dimensions: orderEntity.Dimensions,
                comment: orderEntity.Comment);
        }

        /// <summary> Получение информации о заказах покупателя для указанного временного интервала </summary>
        /// <param name="buyerId"> Id покупателя </param>
        /// <param name="createdFromDt"> Создан от какого времени </param>
        /// <param name="createdToDt"> Создан до какого времени </param>
        /// <returns> IQueryable(Entities.Order) </returns>
        private IQueryable<Entities.Order> GetIQueryableOrdersByByyer(
            uint buyerId,
            DateTime createdFromDt,
            DateTime? createdToDt)
        {
            createdFromDt = createdFromDt.ToUniversalTime();
            createdToDt = createdToDt?.ToUniversalTime();

            Expression<Func<Entities.Order, bool>> expressionWhereCreatedToDt = createdToDt == null ?
                    order => (order.Created <= DateTime.Now.ToUniversalTime()) :
                    order => order.Created <= createdToDt;

            return _dbContext.Orders.AsNoTracking()
                .Where(order => order.BuyerId == buyerId)
                .Where(order => order.Created >= createdFromDt)
                .Where(expressionWhereCreatedToDt);
        }

        private async Task<Entities.Order?> GetOrderEntity(uint orderId, bool asNoTracking)
        {
            var query = asNoTracking ?
                _dbContext.Orders.AsNoTracking().Where(order => order.Id == orderId) :
                _dbContext.Orders.Where(order => order.Id == orderId);

            var orderEntity = await query.SingleOrDefaultAsync();

            return orderEntity;
        }
    }
}
