using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Services
{
    public interface ITradeService
    {
        /// <summary> Добавление платежа (оплаты) совершенной ранее покупки (заказа/чека/квитанции) </summary>
        public Task<Result> AddPayment(Trade trade);
        
        /// <summary> Добавление возврата </summary>
        public Task<Result> AddRefund(Trade trade);

        /// <summary> Получение информации о транзакции оплаты/возврата по её id </summary>
        public Task<Trade> GetTransactionInfoById(
            long transactionId,
            uint? buyerId);
        public Task<IEnumerable<Trade>> GetTransactionInfosByBuyerId(
            uint buyerId,
            uint? buyerIdFromClaim,
            DateTime createdFromDt,
            DateTime? createdToDt,
            uint byPage,
            uint page);
        public Task<IEnumerable<Trade>> GetTransactionInfosByOrderId(
            uint orderId,
            uint? buyerId,
            uint? buyerIdFromClaim);
    }
}
