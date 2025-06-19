using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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

        public Task<Order> GetOrderInfoById(uint orderId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> CancelOrderByBuyer(uint buyerIdFromRequest, uint orderId, string confirmationString)
        {
            throw new NotImplementedException();
        }

        public Task<Result> CancelOrderByManager(uint? managerId, uint orderId, string confirmationString)
        {
            throw new NotImplementedException();
        }

        public Task<Result> ConfirmOrderByByer(uint buyerIdFromRequest, uint orderId, string confirmationString)
        {
            throw new NotImplementedException();
        }

        public Task<Result> MarkAsDeliveredToShop(uint? managerId, uint orderId, string comment)
        {
            throw new NotImplementedException();
        }

        public Task<Result> MarkAsReceived(uint orderId, string comment, uint? managerId, uint? courierId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateCourierId(uint orderId, uint? courierId, string comment)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateDeliveryAddressByBuyer(uint buyerIdFromRequest, uint orderId, string deliveryAddress)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateDeliveryId(uint orderId, uint? deliveryId, uint? managerId, uint? courierId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateExtraInfoByBuyer(uint buyerIdFromRequest, uint orderId, string extraInfo)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateManagerId(uint managerId, uint orderId, string comment)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateMassInGramsDimensions(uint orderId, uint massInGrams, string dimensions, uint? managerId, uint? courierId, string comment)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdatePaymentInfo(uint orderId, string paymentInfo, string comment, uint? managerId, uint? courierId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdatePlannedDeliveryTimeByManager(uint orderId, DateTime plannedDeliveryTime, uint managerId, string comment)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateShopIdByBuyer(uint buyerId, uint orderId, uint? shopId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetOrdersByBuyerId(uint buyerId, DateTime actualFromDt, DateTime? actualToDt, uint take, uint skipCount)
        {
            throw new NotImplementedException();
        }
    }
}
