using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Repositories
{
    public interface IOrdersRepository
    {
        public Task<Result> CancelOrderByBuyer(
            uint buyerIdFromRequest,
            uint orderId,
            string confirmationString,
            string comment);
        
        public Task<Result> CancelOrderByManager(
            uint managerId,
            uint orderId,
            string confirmationString,
            string comment);
        
        public Task<Result> ConfirmOrderByBuyer(
            uint buyerIdFromRequest,
            uint orderId,
            string confirmationString);
        
        public Task<Result> Create(Order newOrder);
        
        public Task<Order> GetOrderInfoById(uint orderId);
        
        public Task<IEnumerable<Order>> GetOrdersByBuyerId(
            uint buyerId,
            DateTime createdFromDt,
            DateTime? createdToDt,
            uint take,
            uint skipCount);
        
        public Task<Result> MarkAsDeliveredToBuyer(
            uint orderId,
            string comment,
            uint courierId);

        public Task<Result> MarkAsDeliveredToShop(
            uint managerId,
            uint orderId,
            string comment);
        
        public Task<Result> MarkAsReceived(
            uint orderId,
            string comment,
            uint? managerId,
            uint? courierId);
        
        public Task<Result> UpdateCourierId(
            uint orderId,
            uint? courierId,
            string comment);
        
        public Task<Result> UpdateDeliveryAddressByBuyer(
            uint buyerIdFromRequest,
            uint orderId,
            string deliveryAddress);
        
        public Task<Result> UpdateDeliveryId(
            uint orderId,
            uint? deliveryId,
            uint? managerId,
            uint? courierId
            , string comment);
        public Task<Result> UpdateExtraInfoByBuyer(
            uint buyerIdFromRequest,
            uint orderId,
            string extraInfo);

        public Task<Result> UpdateManagerId(
            uint managerId,
            uint orderId,
            string comment);

        public Task<Result> UpdateMassInGramsDimensions(
            uint orderId,
            uint massInGrams,
            string dimensions,
            uint? managerId,
            uint? courierId,
            string comment);

        public Task<Result> UpdatePaymentInfo(
            uint orderId,
            string paymentInfo,
            string comment,
            uint? managerId,
            uint? courierId);

        public Task<Result> UpdatePlannedDeliveryTimeByManager(
            uint orderId,
            DateTime plannedDeliveryTime,
            uint managerId,
            string comment);

        public Task<Result> UpdateShopIdByBuyer(
            uint buyerId,
            uint orderId,
            uint? shopId);
    }
}
