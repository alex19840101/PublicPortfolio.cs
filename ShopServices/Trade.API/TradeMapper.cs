using System;
using Microsoft.AspNetCore.Http;
using ShopServices.Core.Models;
using Trade.API.Contracts.Requests;

namespace Trade.API
{
    internal static class TradeMapper
    {
        internal static ShopServices.Core.Models.Trade PrepareCoreTrade(
            AddPaymentRequest addPaymentRequestDto)
        {
            throw new NotImplementedException();
        }

        internal static ShopServices.Core.Models.Trade PrepareCoreTrade(
            AddRefundRequest addRefundRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}
