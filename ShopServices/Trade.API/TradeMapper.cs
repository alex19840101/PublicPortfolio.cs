using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ShopServices.Core.Models;
using Trade.API.Contracts.Requests;
using Trade.API.Contracts.Responses;

namespace Trade.API
{
    internal static class TradeMapper
    {
        internal static TransactionInfoResponseDto GetTransactionInfoResponseDto(ShopServices.Core.Models.Trade coreTrade)
        {
            return new Contracts.Responses.TransactionInfoResponseDto
            {
                Id = coreTrade.Id,
                OrderId = coreTrade.OrderId,
                BuyerId = coreTrade.BuyerId,
                Positions = coreTrade.Positions.Select(op => new OrderPositionResponseDto
                {
                    Id = op.Id,
                    ProductId = op.ProductId,
                    ArticleNumber = op.ArticleNumber,
                    Brand = op.Brand,
                    Name = op.Name,
                    Params = op.Params,
                    Price = op.Price,
                    Quantity = op.Quantity,
                    Cost = op.Cost,
                    Currency = op.Currency
                }).ToList(),
                PositionsStr = coreTrade.GetPositionsStr(),
                Amount = coreTrade.Amount,
                Currency = coreTrade.Currency,
                Created = coreTrade.Created,
                PaymentInfo = coreTrade.PaymentInfo,
                ExtraInfo = coreTrade.ExtraInfo,
                Comment = coreTrade.Comment,
                ShopId = coreTrade.ShopId,
                ManagerId = coreTrade.ManagerId,
                CourierId = coreTrade.CourierId,
                RefundDateTime = coreTrade.RefundDateTime,
                RefundAmount = coreTrade.RefundAmount,
                RefundInfo = coreTrade.RefundInfo,
                Archived = coreTrade.Archived
            };
        }

        /// <summary> Маппер AddPaymentRequest - ShopServices.Core.Models.Trade </summary>
        /// <param name="addPaymentRequestDto"></param>
        /// <returns> ShopServices.Core.Models.Trade - информация о транзакции </returns>
        internal static ShopServices.Core.Models.Trade PrepareCorePayment(
            AddPaymentRequest addPaymentRequestDto)
        {
            var coreOrderPositions = from op in addPaymentRequestDto.Positions
                                     select new OrderPosition
                                     (
                                         id: 0,
                                         productId: op.ProductId,
                                         articleNumber: null,
                                         brand: null,
                                         name: null,
                                         @params: null,
                                         price: op.Price,
                                         quantity: op.Quantity,
                                         cost: op.Cost,
                                         currency: op.Currency
                                     );

            return new ShopServices.Core.Models.Trade(
                id: 0,
                orderId: addPaymentRequestDto.OrderId,
                buyerId: addPaymentRequestDto.BuyerId,
                positions: coreOrderPositions.ToList(),
                amount: addPaymentRequestDto.Amount,
                currency: addPaymentRequestDto.Currency,
                created: addPaymentRequestDto.Created,
                paymentInfo: addPaymentRequestDto.PaymentInfo,
                extraInfo: addPaymentRequestDto.ExtraInfo,
                comment: addPaymentRequestDto.Comment,
                shopId: addPaymentRequestDto.ShopId,
                managerId: addPaymentRequestDto.ManagerId,
                courierId: addPaymentRequestDto.CourierId,
                refundDateTime: null,
                refundAmount: null,
                refundInfo: null,
                archived: false);
        }

        /// <summary> Маппер AddRefundRequest - ShopServices.Core.Models.Trade </summary>
        /// <param name="addRefundRequestDto"></param>
        /// <returns> ShopServices.Core.Models.Trade - информация о транзакции </returns>
        internal static ShopServices.Core.Models.Trade PrepareCoreRefund(
            AddRefundRequest addRefundRequestDto)
        {
            var coreOrderPositions = from op in addRefundRequestDto.Positions
                                     select new OrderPosition
                                     (
                                         id: 0,
                                         productId: op.ProductId,
                                         articleNumber: null,
                                         brand: null,
                                         name: null,
                                         @params: null,
                                         price: op.Price,
                                         quantity: op.Quantity,
                                         cost: op.Cost,
                                         currency: op.Currency
                                     );

            return new ShopServices.Core.Models.Trade(
                id: 0,
                orderId: addRefundRequestDto.OrderId,
                buyerId: addRefundRequestDto.BuyerId,
                positions: coreOrderPositions.ToList(),
                amount: addRefundRequestDto.Amount,
                currency: addRefundRequestDto.Currency,
                created: addRefundRequestDto.Created,
                paymentInfo: addRefundRequestDto.PaymentInfo,
                extraInfo: addRefundRequestDto.ExtraInfo,
                comment: addRefundRequestDto.Comment,
                shopId: addRefundRequestDto.ShopId,
                managerId: addRefundRequestDto.ManagerId,
                courierId: addRefundRequestDto.CourierId,
                refundDateTime: addRefundRequestDto.RefundDateTime,
                refundAmount: addRefundRequestDto.RefundAmount,
                refundInfo: addRefundRequestDto.RefundInfo,
                archived: addRefundRequestDto.Archived);
        }

        /// <summary> Маппинг IEnumerable(Core.Models.Trade) - IEnumerable(Contracts.Responses.TransactionInfoResponseDto) </summary>
        /// <param name="trxList"> список транзакций IEnumerable(Core.Models.Trade) </param>
        /// <returns></returns>
        internal static IEnumerable<Contracts.Responses.TransactionInfoResponseDto> GetTrxDtos(this IEnumerable<ShopServices.Core.Models.Trade> trxList)
        {
            return trxList.Select(coreTrade => new Contracts.Responses.TransactionInfoResponseDto
            {
                Id = coreTrade.Id,
                OrderId = coreTrade.OrderId,
                BuyerId = coreTrade.BuyerId,
                Positions = coreTrade.Positions.Select(op => new OrderPositionResponseDto
                {
                    Id = op.Id,
                    ProductId = op.ProductId,
                    ArticleNumber = op.ArticleNumber,
                    Brand = op.Brand,
                    Name = op.Name,
                    Params = op.Params,
                    Price = op.Price,
                    Quantity = op.Quantity,
                    Cost = op.Cost,
                    Currency = op.Currency
                }).ToList(),
                PositionsStr = coreTrade.GetPositionsStr(),
                Amount = coreTrade.Amount,
                Currency = coreTrade.Currency,
                Created = coreTrade.Created,
                PaymentInfo = coreTrade.PaymentInfo,
                ExtraInfo = coreTrade.ExtraInfo,
                Comment = coreTrade.Comment,
                ShopId = coreTrade.ShopId,
                ManagerId = coreTrade.ManagerId,
                CourierId = coreTrade.CourierId,
                RefundDateTime = coreTrade.RefundDateTime,
                RefundAmount = coreTrade.RefundAmount,
                RefundInfo = coreTrade.RefundInfo,
                Archived = coreTrade.Archived
            });
        }
    }
}
