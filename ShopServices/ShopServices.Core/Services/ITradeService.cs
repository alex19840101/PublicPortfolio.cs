using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Services
{
    public interface ITradeService
    {
        /// <summary> Добавление платежа (оплаты) совершенной ранее покупки (заказа/чека/квитанции) </summary>
        public Task<Result> AddTrade(Trade trade);
        
        /// <summary> Добавление возврата </summary>
        public Task<Result> AddRefund(Trade trade);
    }
}
