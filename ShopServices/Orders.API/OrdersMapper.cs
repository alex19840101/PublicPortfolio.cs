using System.Collections.Generic;
using System.Linq;
using ShopServices.Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using Orders.API.Contracts.Responses;

namespace Orders.API
{
    internal static class OrdersMapper
    {
        internal static OrderResponseDto GetOrderResponseDto(Order coreOrder)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Создание предварительного Core.Models.Order-заказа для добавления заказа
        /// </summary>
        /// <param name="addOrderRequestDto"></param>
        /// <param name="httpContext"> HttpContext для получения Claims для проверки покупателя </param>
        /// <returns> Предварительный Core.Models.Order-заказ для добавления заказа </returns>
        internal static Order PrepareCoreOrder(
            Contracts.Requests.AddOrderRequest addOrderRequestDto,
            HttpContext httpContext)
        {
            var idFromClaimParsed = uint.TryParse(httpContext.User.FindFirst(ClaimTypes.UserData)!.Value, out var idFromClaim);
            
            var buyerStub = new Buyer(
                id: idFromClaim,
                login: httpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                name: null,
                surname: null,
                address: null,
                email: httpContext.User.FindFirst(ClaimTypes.Email)!.Value,
                phones: null,
                passwordHash: null,
                created: DateTime.Now,
                updated: null);

            var coreOrderPositions = from op in addOrderRequestDto.Positions
                select new OrderPosition
                (
                    id: 0,
                    productId: op.ProductId,
                    articleNumber: null,
                    brand: null,
                    name: null,
                    parameters: null,
                    price: op.Price,
                    quantity: op.Quantity,
                    cost: op.Cost,
                    currency: op.Currency
                );

            return new Order(
                id: 0,
                buyerId: addOrderRequestDto.BuyerId,
                buyer: buyerStub,
                positions: coreOrderPositions.ToList(),
                cost: addOrderRequestDto.Cost,
                currency: addOrderRequestDto.Currency,
                created: addOrderRequestDto.Created,
                plannedDeliveryTime: addOrderRequestDto.PlannedDeliveryTime,
                paymentInfo: addOrderRequestDto.PaymentInfo,
                deliveryAddress: addOrderRequestDto.DeliveryAddress,
                shopId: addOrderRequestDto.ShopId,
                extraInfo: addOrderRequestDto.ExtraInfo,
                archieved: false,
                massInGrams: addOrderRequestDto.MassInGrams ?? 0,
                dimensions: addOrderRequestDto.Dimensions);
        }
    }
}
