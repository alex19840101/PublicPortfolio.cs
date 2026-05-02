using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Repositories
{
    public interface ITradeRepository
    {
        Task<Result> AddPayment(Trade trade);
        Task<Result> AddRefund(Trade trade);
        Task<Trade> GetTransactionInfoById(long transactionId);
        Task<IEnumerable<Trade>> GetTransactionInfosByBuyerId(
            uint buyerId,
            DateTime createdFromDt,
            DateTime? createdToDt,
            uint take,
            uint skipCount);
        Task<IEnumerable<Trade>> GetTransactionInfosByOrderId(
            uint orderId,
            uint? buyerId);
    }
}
