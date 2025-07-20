using System;
using System.Collections.Generic;
using System.Linq;
using Deliveries.API.Contracts;
using ShopServices.Core.Models;

namespace Deliveries.API
{
    internal static class DeliveriesMapper
    {
        internal static Delivery PrepareCoreDelivery(DeliveryDto newDeliveryDto)
        {
            return new Delivery(
                id: 0,
                buyerId: newDeliveryDto.BuyerId,
                orderId: newDeliveryDto.OrderId,
                regionCode: newDeliveryDto.RegionCode,
                address: newDeliveryDto.Address,
                managerId: newDeliveryDto.ManagerId,
                courierId: newDeliveryDto.CourierId,
                paymentInfo: newDeliveryDto.PaymentInfo,
                massInGrams: newDeliveryDto.MassInGrams,
                dimensions: newDeliveryDto.Dimensions,
                fromWarehouseId: newDeliveryDto.FromWarehouseId,
                toWarehouseId: newDeliveryDto.ToWarehouseId,
                fromShopId: newDeliveryDto.FromShopId,
                toShopId: newDeliveryDto.ToShopId,
                comment: newDeliveryDto.Comment,
                status: newDeliveryDto.Status,
                transferId: newDeliveryDto.TransferId,
                createdDt: DateTime.Now);
            }

        internal static DeliveryDto GetDeliveryDto(Delivery coreDelivery)
        {
            return new Contracts.DeliveryDto
            {
                Id = coreDelivery.Id,
                BuyerId = coreDelivery.BuyerId,
                OrderId = coreDelivery.OrderId,
                PaymentInfo = coreDelivery.PaymentInfo,
                ManagerId = coreDelivery.ManagerId,
                CourierId = coreDelivery.CourierId,
                RegionCode = coreDelivery.RegionCode,
                Address = coreDelivery.Address,
                MassInGrams = coreDelivery.MassInGrams,
                Dimensions = coreDelivery.Dimensions,
                Created = coreDelivery.CreatedDt!.Value,
                Updated = coreDelivery.Updated,
                FromWarehouseId = coreDelivery.FromWarehouseId,
                ToWarehouseId = coreDelivery.ToWarehouseId,
                FromShopId = coreDelivery.FromShopId,
                ToShopId = coreDelivery.ToShopId,
                TransferId = coreDelivery.TransferId,
                Comment = coreDelivery.Comment,
                Status = coreDelivery.Status
            };
        }

        internal static Delivery GetCoreDelivery(DeliveryDto updateDeliveryRequestDto)
        {
            return new Delivery(
                id: updateDeliveryRequestDto.Id,
                buyerId: updateDeliveryRequestDto.BuyerId,
                orderId: updateDeliveryRequestDto.OrderId,
                regionCode: updateDeliveryRequestDto.RegionCode,
                address: updateDeliveryRequestDto.Address,
                managerId: updateDeliveryRequestDto.ManagerId,
                courierId: updateDeliveryRequestDto.CourierId,
                paymentInfo: updateDeliveryRequestDto.PaymentInfo,
                massInGrams: updateDeliveryRequestDto.MassInGrams,
                dimensions: updateDeliveryRequestDto.Dimensions,
                fromWarehouseId: updateDeliveryRequestDto.FromWarehouseId,
                toWarehouseId: updateDeliveryRequestDto.ToWarehouseId,
                fromShopId: updateDeliveryRequestDto.FromShopId,
                toShopId: updateDeliveryRequestDto.ToShopId,
                comment: updateDeliveryRequestDto.Comment,
                status: updateDeliveryRequestDto.Status,
                transferId: updateDeliveryRequestDto.TransferId);
        }


        /// <summary>
        /// Маппинг IEnumerable(Core.Models.Delivery) - IEnumerable(Contracts.DeliveryDto)
        /// </summary>
        /// <param name="deliveriesList"> список перевозок (доставок) IEnumerable(Core.Models.Delivery) </param>
        /// <returns></returns>
        internal static IEnumerable<DeliveryDto> GetDeliveriesDtos(this IEnumerable<Delivery> deliveriesList)
        {
            return deliveriesList.Select(coreDelivery => new Contracts.DeliveryDto
            {
                Id = coreDelivery.Id,
                BuyerId = coreDelivery.BuyerId,
                OrderId = coreDelivery.OrderId,
                PaymentInfo = coreDelivery.PaymentInfo,
                ManagerId = coreDelivery.ManagerId,
                CourierId = coreDelivery.CourierId,
                RegionCode = coreDelivery.RegionCode,
                Address = coreDelivery.Address,
                MassInGrams = coreDelivery.MassInGrams,
                Dimensions = coreDelivery.Dimensions,
                Created = coreDelivery.CreatedDt!.Value,
                Updated = coreDelivery.Updated,
                FromWarehouseId = coreDelivery.FromWarehouseId,
                ToWarehouseId = coreDelivery.ToWarehouseId,
                FromShopId = coreDelivery.FromShopId,
                ToShopId = coreDelivery.ToShopId,
                TransferId = coreDelivery.TransferId,
                Comment = coreDelivery.Comment,
                Status = coreDelivery.Status
            });
        }
    }
}
