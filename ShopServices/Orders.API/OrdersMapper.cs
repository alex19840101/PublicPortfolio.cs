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
using ShopServices.Core.Enums;

namespace Orders.API
{
    internal static class OrdersMapper
    {
        /// <summary> Маппер Core.Models.Order - Contracts.Responses.OrderResponseDto </summary>
        /// <param name="coreOrder"> Core.Models.Order - заказ </param>
        /// <returns> Contracts.Responses.OrderResponseDto - данные заказа </returns>
        internal static OrderResponseDto GetOrderResponseDto(Order coreOrder)
        {
            return new Contracts.Responses.OrderResponseDto
            {
                Id = coreOrder.Id,
                BuyerId = coreOrder.BuyerId,

                Positions = coreOrder.Positions.Select(op => new OrderPositionResponseDto
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

                Cost = coreOrder.Cost,
                Currency = coreOrder.Currency,
                Created = coreOrder.Created,
                PlannedDeliveryTime = coreOrder.PlannedDeliveryTime,
                Delivered = coreOrder.Delivered,
                Received = coreOrder.Received,
                Updated = coreOrder.Updated,
                CourierId = coreOrder.CourierId,
                DeliveryId = coreOrder.DeliveryId,
                DeliveryAddress = coreOrder.DeliveryAddress,
                ShopId = coreOrder.ShopId,
                ManagerId = coreOrder.ManagerId,
                ExtraInfo = coreOrder.ExtraInfo,
                Archived = coreOrder.Archived,
                MassInGrams = coreOrder.MassInGrams,
                Dimensions = coreOrder.Dimensions,
                Comment = coreOrder.Comment,
            };
        }

        /// <summary> Создание предварительного Core.Models.Order-заказа для добавления заказа </summary>
        /// <param name="addOrderRequestDto"> Запрос на добавление заказа </param>
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
                telegramChatId: null,
                notificationMethods: [NotificationMethod.Email, NotificationMethod.SMS],
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
                archived: false,
                massInGrams: addOrderRequestDto.MassInGrams ?? 0,
                dimensions: addOrderRequestDto.Dimensions);
        }

        /// <summary> Маппинг IEnumerable(Core.Models.Order) - IEnumerable(Contracts.Responses.OrderResponseDto) </summary>
        /// <param name="ordersList"> список заказов IEnumerable(Core.Models.Order) </param>
        /// <returns></returns>
        internal static IEnumerable<Contracts.Responses.OrderResponseDto> GetOrderDtos(this IEnumerable<Order> ordersList)
        {
            return ordersList.Select(coreOrder => new Contracts.Responses.OrderResponseDto
            {
                Id = coreOrder.Id,
                BuyerId = coreOrder.BuyerId,

                Positions = coreOrder.Positions.Select(op => new OrderPositionResponseDto
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

                Cost = coreOrder.Cost,
                Currency = coreOrder.Currency,
                Created = coreOrder.Created,
                PlannedDeliveryTime = coreOrder.PlannedDeliveryTime,
                Delivered = coreOrder.Delivered,
                Received = coreOrder.Received,
                Updated = coreOrder.Updated,
                CourierId = coreOrder.CourierId,
                DeliveryId = coreOrder.DeliveryId,
                DeliveryAddress = coreOrder.DeliveryAddress,
                ShopId = coreOrder.ShopId,
                ManagerId = coreOrder.ManagerId,
                ExtraInfo = coreOrder.ExtraInfo,
                Archived = coreOrder.Archived,
                MassInGrams = coreOrder.MassInGrams,
                Dimensions = coreOrder.Dimensions
            });
        }
    }
}
