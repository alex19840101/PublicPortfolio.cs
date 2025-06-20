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

            var orderPositionsEntities = from op in newOrder.Positions
                select new Entities.OrderPosition(
                id: 0,
                productId: op.ProductId,
                articleNumber: op.ArticleNumber,
                brand: op.Brand,
                name: op.Name,
                parameters: op.Params,
                price: op.Price,
                quantity: op.Quantity,
                cost: op.Cost,
                currency: op.Currency);

            ArgumentNullException.ThrowIfNull(orderPositionsEntities);

            var coreBuyer = newOrder.Buyer;

            var buyerEntity = new Entities.Buyer(
                id: coreBuyer.Id,
                login: coreBuyer.Login,
                name: coreBuyer.Name,
                surname: coreBuyer.Surname,
                address: coreBuyer.Address,
                email: coreBuyer.Email,
                passwordHash: coreBuyer.PasswordHash,
                nick: coreBuyer.Nick,
                phone: coreBuyer.Phones,
                discountGroups: coreBuyer.DiscountGroups,
                granterId: coreBuyer.GranterId,
                createdDt: coreBuyer.Created,
                lastUpdateDt: coreBuyer.Updated,
                blocked: coreBuyer.Blocked
            );

            var newOrderEntity = new Entities.Order
            (
                id: 0,
                buyerId: newOrder.BuyerId,
                buyer: buyerEntity,
                positions: orderPositionsEntities.ToList(),
                cost: newOrder.Cost,
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
            await _dbContext.OrderPositions.AddRangeAsync(orderPositionsEntities);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newOrderEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id
            return new Result
            {
                Id = newOrderEntity.Id,
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
            string confirmationString)
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
                orderEntity.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.ORDER_CANCELED, HttpStatusCode.OK);
            }
            return new Result(ResultMessager.ORDER_CANCELED_EARLIER, HttpStatusCode.OK);
        }

        public async Task<Result> CancelOrderByManager(
            uint? managerId,
            uint orderId,
            string confirmationString)
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
                orderEntity.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.ORDER_CANCELED, HttpStatusCode.OK);
            }
            return new Result(ResultMessager.ORDER_CANCELED_EARLIER, HttpStatusCode.OK);
        }

        public async Task<Result> ConfirmOrderByByer(
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

            orderEntity.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.ORDER_CONFIRMED, HttpStatusCode.OK);
            }
            return new Result(ResultMessager.ORDER_CONFIRMED_EARLIER, HttpStatusCode.OK);
        }

        public async Task<Result> MarkAsDeliveredToShop(
            uint? managerId,
            uint orderId,
            string comment)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> MarkAsReceived(
            uint orderId,
            string comment,
            uint? managerId,
            uint? courierId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UpdateCourierId(
            uint orderId,
            uint? courierId,
            string comment)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UpdateDeliveryAddressByBuyer(
            uint buyerIdFromRequest,
            uint orderId,
            string deliveryAddress)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UpdateDeliveryId(
            uint orderId,
            uint? deliveryId,
            uint? managerId,
            uint? courierId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UpdateExtraInfoByBuyer(
            uint buyerIdFromRequest,
            uint orderId,
            string extraInfo)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UpdateManagerId(
            uint managerId,
            uint orderId,
            string comment)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UpdateMassInGramsDimensions(
            uint orderId,
            uint massInGrams,
            string dimensions,
            uint? managerId,
            uint? courierId,
            string comment)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UpdatePaymentInfo(
            uint orderId,
            string paymentInfo,
            string comment,
            uint? managerId,
            uint? courierId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UpdatePlannedDeliveryTimeByManager(
            uint orderId,
            DateTime plannedDeliveryTime,
            uint managerId,
            string comment)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UpdateShopIdByBuyer(
            uint buyerId,
            uint orderId,
            uint? shopId)
        {
            throw new NotImplementedException();
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

        public async Task<Result> MarkAsDeliveredToBuyer(uint orderId, string comment, uint? courierId)
        {
            throw new NotImplementedException();
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
                                         parameters: op.Params,
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
                archieved: orderEntity.Archieved,
                massInGrams: orderEntity.MassInGrams,
                dimensions: orderEntity.Dimensions);
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
                    order => (order.Created <= DateTime.Now) :
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
