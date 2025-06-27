using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Enums;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IOrdersRepository _ordersRepository;

        public DeliveryService(
            IDeliveryRepository deliveryRepository,
            IOrdersRepository ordersRepository)
        {
            _deliveryRepository = deliveryRepository;
            _ordersRepository = ordersRepository;
        }

        public async Task<Result> AddDelivery(Delivery delivery)
        {
            var errorResult = UnValidatedDeliveryResult(delivery, checkDeliveryId: false, statusShouldBeToAdd: true);
            if (errorResult != null)
                return errorResult;

            var order = await _ordersRepository.GetOrderInfoById(delivery.OrderId);

            if (order == null)
                return new Result(ResultMessager.ORDER_NOT_FOUND, System.Net.HttpStatusCode.NotFound);

            if (delivery.BuyerId != order.BuyerId)
                return new Result(ResultMessager.BUYER_ID_MISMATCH, System.Net.HttpStatusCode.Conflict);

            if (order.Received != null)
                return new Result(ResultMessager.ORDER_RECEIVED_EARLIER, System.Net.HttpStatusCode.Conflict);

            var createResult = await _deliveryRepository.Create(delivery);

            return createResult;
        }

        public async Task<IEnumerable<Delivery>> GetDeliveries(
            uint? regionCode,
            uint? buyerId,
            string addressSubString = null,
            uint byPage = 10,
            uint page = 1,
            bool ignoreCase = true)
        {
            var take = byPage;
            var skip = page > 1 ? (page - 1) * byPage : 0;

            return await _deliveryRepository.GetDeliveries(
                regionCode: regionCode,
                addressSubString: addressSubString,
                buyerId: buyerId,
                take: take,
                skip: skip,
                ignoreCase: ignoreCase);
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesForOrder(
            uint orderId,
            uint byPage = 10,
            uint page = 1)
        {
            var take = byPage;
            var skip = page > 1 ? (page - 1) * byPage : 0;

            return await _deliveryRepository.GetDeliveriesForOrder(
                orderId: orderId,
                take: take,
                skip: skip);
        }

        public async Task<Delivery> GetDeliveryById(uint deliveryId)
        {
            return await _deliveryRepository.GetDeliveryById(deliveryId);
        }

        public async Task<Result> UpdateDelivery(Delivery delivery)
        {
            var errorResult = UnValidatedDeliveryResult(delivery);
            if (errorResult != null)
                return errorResult;

            var order = await _ordersRepository.GetOrderInfoById(delivery.OrderId);

            if (order == null)
                return new Result(ResultMessager.ORDER_NOT_FOUND, System.Net.HttpStatusCode.NotFound);

            if (delivery.BuyerId != order.BuyerId)
                return new Result(ResultMessager.BUYER_ID_MISMATCH, System.Net.HttpStatusCode.Conflict);

            if (order.Received != null)
                return new Result(ResultMessager.ORDER_RECEIVED_EARLIER, System.Net.HttpStatusCode.Conflict);


            return await _deliveryRepository.UpdateDelivery(delivery);
        }
        public async Task<Result> ArchiveDelivery(uint deliveryId)
        {
            return await _deliveryRepository.ArchiveDeliveryById(deliveryId);
        }


        /// <summary> Валидация данных перевозки(доставки) </summary>
        /// <param name="delivery"> Данные перевозки(доставки) </param>
        /// <param name="checkDeliveryId"> Проверять ли, что delivery.Id > 0 </param>
        /// <param name="statusShouldBeToAdd"> Проверять ли, что delivery.Status == ToAdd </param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private Result UnValidatedDeliveryResult(Delivery delivery, bool checkDeliveryId = true, bool statusShouldBeToAdd = false)
        {
            if (delivery == null)
                throw new ArgumentNullException(ResultMessager.DELIVERY_RARAM_NAME);

            if (checkDeliveryId && delivery.Id == 0)
                return new Result(ResultMessager.DELIVERY_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.BadRequest);

            if (statusShouldBeToAdd && delivery.Status != DeliveryStatus.ToAdd)
                return new Result(ResultMessager.DELIVERY_STATUS_SHOULD_BE_TOADD, System.Net.HttpStatusCode.BadRequest);

            if (delivery.BuyerId == 0)
                return new Result(ResultMessager.BUYER_NOT_FOUND, System.Net.HttpStatusCode.BadRequest);

            if (delivery.OrderId == 0)
                return new Result(ResultMessager.ORDER_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.BadRequest);

            if (delivery.RegionCode == 0)
                return new Result(ResultMessager.REGIONCODE_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.BadRequest);

            if (delivery.ManagerId == 0)
                return new Result(ResultMessager.MANAGER_ID_IS_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(delivery.Address))
                return new Result(ResultMessager.ADDRESS_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(delivery.PaymentInfo))
                return new Result(ResultMessager.PAYMENT_INFO_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (delivery.MassInGrams == 0)
                return new Result(ResultMessager.MASS_IN_GRAMS_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(delivery.Dimensions))
                return new Result(ResultMessager.DIMENSIONS_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(delivery.Comment))
                return new Result(ResultMessager.COMMENT_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            return null;
        }
    }
}
