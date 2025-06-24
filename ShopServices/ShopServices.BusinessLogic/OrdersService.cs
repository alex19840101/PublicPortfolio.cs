using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IBuyersRepository _buyersRepository;
        private readonly IPricesRepository _pricesRepository;
        
        /// <summary> 100% </summary>
        const int HUNDRED_PROCENTS = 100;
        /// <summary> Допустимая погрешность стоимости </summary>
        const decimal PERMISSIBLE_PRICE_ERROR = 1m;

        /// <summary> Допустимая погрешность рассинхронизации времени, с </summary>
        const int PERMISSIBLE_TIME_DESYNC_SECONDS = 36000; //TODO: временно, убрать перед merge в main

        public OrdersService(
            IOrdersRepository ordersRepository,
            IBuyersRepository buyersRepository,
            IPricesRepository pricesRepository,
            IProductsRepository productsRepository)
        {
            _ordersRepository = ordersRepository;
            _buyersRepository = buyersRepository;
            _pricesRepository = pricesRepository;
            _productsRepository = productsRepository;
        }

        public async Task<Result> AddOrder(Order order)
        {
            var errorResult = UnValidatedOrderResult(order);
            if (errorResult != null)
                return errorResult;

            var buyer = await _buyersRepository.GetUser(order.BuyerId);

            errorResult = UnValidatedBuyerResult(buyer, order.Buyer);
            if (errorResult != null)
                return errorResult;
            order.UpdateBuyer(buyer);

            decimal totalCost = 0;
            for (int pos = 0;  pos < order.Positions.Count; pos++)
            {
                var position = order.Positions[pos];
                
                var product = await _productsRepository.GetProductById(position.ProductId);
                errorResult = UnValidatedProductResult(product);
                if (errorResult != null)
                    return errorResult;

                var price = await _pricesRepository.GetPriceById(product.PriceId.Value);

                if (!string.Equals(position.Currency, price.Currency))
                    return new Result(ResultMessager.POSITION_CURRENCY_IS_NOT_PRICE_CURRENCY, System.Net.HttpStatusCode.Conflict);

                if (!string.Equals(order.Currency, price.Currency))
                    return new Result(ResultMessager.ORDER_CURRENCY_IS_NOT_PRICE_CURRENCY, System.Net.HttpStatusCode.Conflict);

                if (price.PricePerUnit != product.PricePerUnit)
                    return new Result(ResultMessager.PRICE_PRICEPERUNIT_IS_NOT_EQUAL_PRODUCT_PRICEPERUNIT, System.Net.HttpStatusCode.Conflict);

                if (DateTime.Now < price.ActualFromDt)
                    return new Result(ResultMessager.PRICE_ACTUAL_FROM_DT_IS_NOT_ACTUAL, System.Net.HttpStatusCode.Conflict);

                if (price.ActualToDt < DateTime.Now)
                    return new Result(ResultMessager.PRICE_ACTUAL_TO_DT_IS_NOT_ACTUAL, System.Net.HttpStatusCode.Conflict);

                var dicountPricePerUnit = (buyer.DiscountGroups != null && buyer.DiscountGroups.Any()) ?
                    price.PricePerUnit * (1 - buyer.DiscountGroups.Max() / HUNDRED_PROCENTS) :
                    price.PricePerUnit;

                if (position.Price < dicountPricePerUnit)
                    return new Result(ResultMessager.POSITION_PRICE_IS_LESS_THAN_DISCOUNT_PRICEPERUNIT, System.Net.HttpStatusCode.Conflict);

                var cost = dicountPricePerUnit * (decimal)position.Quantity;

                if (Math.Abs(cost - position.Cost) > PERMISSIBLE_PRICE_ERROR)
                    return new Result($"{ResultMessager.COST_MISMATCH_MORE_THAN_PERMISSIBLE_PRICE_ERROR} : {cost} - {position.Cost} > {PERMISSIBLE_PRICE_ERROR}",
                        statusCode: System.Net.HttpStatusCode.Conflict);
                totalCost += cost;

                //дополнение полей данными товара
                position.FillByProduct(product);
            }
            if (Math.Abs(totalCost - order.Cost) > PERMISSIBLE_PRICE_ERROR)
                return new Result($"{ResultMessager.COST_MISMATCH_MORE_THAN_PERMISSIBLE_PRICE_ERROR} : {totalCost} - {order.Cost} > {PERMISSIBLE_PRICE_ERROR}",
                    statusCode: System.Net.HttpStatusCode.Conflict);

            var createResult = await _ordersRepository.Create(order);

            return createResult;
        }

        public async Task<Order> GetOrderInfoById(uint orderId, uint? buyerId)
        {
            var order = await _ordersRepository.GetOrderInfoById(orderId);

            if (order == null)
                return null;
            
            if (buyerId != null && buyerId != order.BuyerId)
                return null;

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersByBuyerId(
            uint buyerId,
            uint? buyerIdFromClaim,
            DateTime createdFromDt,
            DateTime? createdToDt,
            uint byPage = 10,
            uint page = 1)
        {
            var take = byPage;
            var skip = page > 1 ? (page - 1) * byPage : 0;

            if (buyerIdFromClaim != buyerId)
                return new List<Order>();

            return await _ordersRepository.GetOrdersByBuyerId(
            buyerId: buyerId,
            createdFromDt: createdFromDt,
            createdToDt: createdToDt,
            take: take,
            skipCount: skip);
        }

        public async Task<Result> UpdateShopIdByBuyer(
            uint buyerIdFromClaim,
            uint buyerIdFromRequest,
            uint orderId,
            uint? shopId)
        {
            if (buyerIdFromClaim != buyerIdFromRequest)
                return new Result(ResultMessager.BUYER_ID_FROM_CLAIM_IS_NOT_FROM_REQUEST, System.Net.HttpStatusCode.Forbidden);

            if (orderId == 0)
                return new Result(ResultMessager.ORDER_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.NotFound);

            if (shopId == 0)
                return new Result(ResultMessager.SHOP_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.NotFound);

            return await _ordersRepository.UpdateShopIdByBuyer(
                buyerId: buyerIdFromClaim,
                orderId: orderId,
                shopId: shopId);
        }

        public async Task<Result> CancelOrderByBuyer(
            uint buyerIdFromClaim,
            uint buyerIdFromRequest,
            uint orderId,
            string confirmationString,
            string comment)
        {
            if (buyerIdFromClaim != buyerIdFromRequest)
                return new Result(ResultMessager.BUYER_ID_FROM_CLAIM_IS_NOT_FROM_REQUEST, System.Net.HttpStatusCode.Forbidden);

            if (orderId == 0)
                return new Result(ResultMessager.ORDER_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.NotFound);

            if (string.IsNullOrWhiteSpace(confirmationString))
                return new Result(ResultMessager.CONFIRMATION_STRING_SHOULD_BE_NOT_NULL_OR_EMPTY, System.Net.HttpStatusCode.NotFound);

            return await _ordersRepository.CancelOrderByBuyer(
                buyerIdFromRequest,
                orderId,
                confirmationString,
                comment);
        }

        public async Task<Result> ConfirmOrderByBuyer(
            uint buyerIdFromClaim,
            uint buyerIdFromRequest,
            uint orderId,
            string confirmationString)
        {
            if (buyerIdFromClaim != buyerIdFromRequest)
                return new Result(ResultMessager.BUYER_ID_FROM_CLAIM_IS_NOT_FROM_REQUEST, System.Net.HttpStatusCode.Forbidden);

            if (orderId == 0)
                return new Result(ResultMessager.ORDER_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.NotFound);

            if (string.IsNullOrWhiteSpace(confirmationString))
                return new Result(ResultMessager.CONFIRMATION_STRING_SHOULD_BE_NOT_NULL_OR_EMPTY, System.Net.HttpStatusCode.NotFound);

            return await _ordersRepository.ConfirmOrderByBuyer(
                buyerIdFromRequest,
                orderId,
                confirmationString);
        }

        public async Task<Result> UpdateDeliveryAddressByBuyer(
            uint buyerIdFromClaim,
            uint buyerIdFromRequest,
            uint orderId,
            string deliveryAddress)
        {
            if (buyerIdFromClaim != buyerIdFromRequest)
                return new Result(ResultMessager.BUYER_ID_FROM_CLAIM_IS_NOT_FROM_REQUEST, System.Net.HttpStatusCode.Forbidden);

            if (orderId == 0)
                return new Result(ResultMessager.ORDER_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.NotFound);
            
            return await _ordersRepository.UpdateDeliveryAddressByBuyer(
               buyerIdFromRequest,
               orderId,
               deliveryAddress);
        }

        public async Task<Result> UpdateExtraInfoByBuyer(
            uint buyerIdFromClaim,
            uint buyerIdFromRequest,
            uint orderId,
            string extraInfo)
        {
            if (buyerIdFromClaim != buyerIdFromRequest)
                return new Result(ResultMessager.BUYER_ID_FROM_CLAIM_IS_NOT_FROM_REQUEST, System.Net.HttpStatusCode.Forbidden);

            if (orderId == 0)
                return new Result(ResultMessager.ORDER_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.NotFound);

            if (string.IsNullOrWhiteSpace(extraInfo))
                return new Result(ResultMessager.EXTRA_INFO_SHOULD_BE_NOT_NULL_OR_EMPTY, System.Net.HttpStatusCode.NotFound);

            return await _ordersRepository.UpdateExtraInfoByBuyer(
               buyerIdFromRequest,
               orderId,
               extraInfo);
        }

        public async Task<Result> CancelOrderByManager(
            uint? managerId,
            uint orderId,
            string confirmationString,
            string comment)
        {
            if (managerId == null)
                return new Result(ResultMessager.MANAGER_ID_IS_NULL, System.Net.HttpStatusCode.BadRequest);

            if (managerId == 0)
                return new Result(ResultMessager.MANAGER_ID_IS_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (orderId == 0)
                return new Result(ResultMessager.ORDER_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.NotFound);

            if (string.IsNullOrWhiteSpace(confirmationString))
                return new Result(ResultMessager.CONFIRMATION_STRING_SHOULD_BE_NOT_NULL_OR_EMPTY, System.Net.HttpStatusCode.NotFound);

            return await _ordersRepository.CancelOrderByManager(
                managerId.Value,
                orderId,
                confirmationString,
                comment);
        }

        public async Task<Result> MarkAsDeliveredToBuyer(uint orderId, string comment, uint? courierId)
        {
            if (courierId == null)
                return new Result(ResultMessager.COURIER_ID_IS_NULL, System.Net.HttpStatusCode.BadRequest);
            
            if (courierId == 0)
                return new Result(ResultMessager.COURIER_ID_IS_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (orderId == 0)
                return new Result(ResultMessager.ORDER_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.NotFound);

            return await _ordersRepository.MarkAsDeliveredToBuyer(
                orderId: orderId,
                comment: comment,
                courierId: courierId.Value);
        }

        public async Task<Result> MarkAsDeliveredToShop(uint orderId, string comment, uint? managerId)
        {
            if (managerId == null)
                return new Result(ResultMessager.MANAGER_ID_IS_NULL, System.Net.HttpStatusCode.BadRequest);

            if (managerId == 0)
                return new Result(ResultMessager.MANAGER_ID_IS_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (orderId == 0)
                return new Result(ResultMessager.ORDER_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.NotFound);

            return await _ordersRepository.MarkAsDeliveredToShop(
                managerId.Value,
                orderId,
                comment);
        }

        public async Task<Result> MarkAsReceived(uint orderId, string comment, uint? managerId, uint? courierId)
        {
            if (managerId == 0)
                return new Result(ResultMessager.MANAGER_ID_IS_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (courierId == 0)
                return new Result(ResultMessager.COURIER_ID_IS_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (orderId == 0)
                return new Result(ResultMessager.ORDER_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.NotFound);

            return await _ordersRepository.MarkAsReceived(
                orderId: orderId,
                comment: comment,
                managerId: managerId,
                courierId: courierId);
        }

        public async Task<Result> UpdateCourierId(uint orderId, string comment, uint? courierId)
        {
            if (courierId == 0)
                return new Result(ResultMessager.COURIER_ID_IS_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (orderId == 0)
                return new Result(ResultMessager.ORDER_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.NotFound);

            return await _ordersRepository.UpdateCourierId(
                orderId: orderId,
                courierId: courierId,
                comment: comment);
        }

        public async Task<Result> UpdateDeliveryId(uint orderId, uint? deliveryId, uint? managerId, uint? courierId, string comment)
        {
            if (managerId == 0)
                return new Result(ResultMessager.MANAGER_ID_IS_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (courierId == 0)
                return new Result(ResultMessager.COURIER_ID_IS_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (orderId == 0)
                return new Result(ResultMessager.ORDER_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.NotFound);

            return await _ordersRepository.UpdateDeliveryId(
                orderId: orderId,
                deliveryId: deliveryId,
                managerId: managerId,
                courierId: courierId,
                comment: comment);
        }

        public async Task<Result> UpdateManagerId(uint orderId, string comment, uint managerId)
        {
            if (managerId == 0)
                return new Result(ResultMessager.MANAGER_ID_IS_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (orderId == 0)
                return new Result(ResultMessager.ORDER_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.NotFound);

            return await _ordersRepository.UpdateManagerId(
                managerId: managerId,
                orderId: orderId,
                comment: comment);
        }

        public async Task<Result> UpdateMassInGramsDimensions(
            uint orderId,
            uint massInGrams,
            string dimensions,
            string comment,
            uint? managerId,
            uint? courierId,
            uint? deliveryId)
        {
            if (managerId == 0)
                return new Result(ResultMessager.MANAGER_ID_IS_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (courierId == 0)
                return new Result(ResultMessager.COURIER_ID_IS_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (orderId == 0)
                return new Result(ResultMessager.ORDER_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.NotFound);

            return await _ordersRepository.UpdateMassInGramsDimensions(
                orderId: orderId,
                massInGrams: massInGrams,
                dimensions: dimensions,
                managerId: managerId,
                courierId: courierId,
                comment: comment,
                deliveryId: deliveryId);
        }

        public async Task<Result> UpdatePaymentInfo(uint orderId, string paymentInfo, string comment, uint? managerId, uint? courierId)
        {
            if (managerId == 0)
                return new Result(ResultMessager.MANAGER_ID_IS_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (courierId == 0)
                return new Result(ResultMessager.COURIER_ID_IS_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (orderId == 0)
                return new Result(ResultMessager.ORDER_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.NotFound);

            return await _ordersRepository.UpdatePaymentInfo(
                orderId: orderId,
                paymentInfo: paymentInfo,
                comment: comment,
                managerId:managerId,
                courierId: courierId);

        }

        public async Task<Result> UpdatePlannedDeliveryTimeByManager(uint orderId, DateTime plannedDeliveryTime, string comment, uint managerId)
        {
            if (managerId == 0)
                return new Result(ResultMessager.MANAGER_ID_IS_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (orderId == 0)
                return new Result(ResultMessager.ORDER_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.NotFound);

            if (plannedDeliveryTime <= DateTime.Now)
                return new Result(ResultMessager.PLANNED_DELIVERY_TIME_SHOULD_BE_MORE_THAN_NOW, System.Net.HttpStatusCode.BadRequest);

            return await _ordersRepository.UpdatePlannedDeliveryTimeByManager(
                orderId: orderId,
                plannedDeliveryTime: plannedDeliveryTime,
                managerId: managerId,
                comment: comment);
        }

        #region private - validators
        /// <summary>
        /// Валидация данных зазаза
        /// </summary>
        /// <param name="order"> Core.Models.Order - заказ (предварительный) </param>
        /// <returns></returns>
        private Result UnValidatedOrderResult(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(ResultMessager.ORDER_RARAM_NAME);

            if (order.BuyerId <= 0)
                return new Result(ResultMessager.BUYER_NOT_FOUND, System.Net.HttpStatusCode.NotFound);

            if (order.Cost <= 0)
                return new Result(ResultMessager.COST_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(order.Currency))
                return new Result(ResultMessager.CURRENCY_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(order.PaymentInfo))
                return new Result(ResultMessager.PAYMENT_INFO_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(order.DeliveryAddress))
                return new Result(ResultMessager.DELIVERY_ADDRESS_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(order.ExtraInfo))
                return new Result(ResultMessager.EXTRA_INFO_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (order.Archieved)
                return new Result(ResultMessager.IMPOSSIBLE_TO_ADD_ARCHIVED_ORDER, System.Net.HttpStatusCode.BadRequest);

            if (Math.Abs((DateTime.Now - order.Created).TotalSeconds) > PERMISSIBLE_TIME_DESYNC_SECONDS)
                return new Result($"{ResultMessager.TIME_DESYNCHRONIZATION_MORE_THAN_PERMISSIBLE_TIME_DESYNC_SECONDS} {PERMISSIBLE_TIME_DESYNC_SECONDS}",
                    statusCode: System.Net.HttpStatusCode.Conflict);

            if (order.PlannedDeliveryTime < DateTime.Now)
                return new Result(ResultMessager.PLANNEDDELIVERYTIME_IS_LESS_THEN_NOW, statusCode: System.Net.HttpStatusCode.BadRequest);

            var errorResult = UnValidatedPositionResult(order.Positions);
            if (errorResult != null)
                return errorResult;

            return null;
        }


        /// <summary>
        /// Валидация данных покупателя
        /// </summary>
        /// <param name="buyer"> Buyer buyer - данные покупателя из БД </param>
        /// <param name="buyerFromOrder"> Buyer buyer.Order - данные покупателя из заказа </param>
        /// <returns> Result - при ошибке | null, если проверка пройдена </returns>
        /// <exception cref="ArgumentNullException"></exception>
        private Result UnValidatedBuyerResult(Buyer buyer, Buyer buyerFromOrder)
        {
            if (buyer == null)
                return new Result(ResultMessager.BUYER_NOT_FOUND, System.Net.HttpStatusCode.NotFound);

            if (buyerFromOrder == null)
                throw new ArgumentNullException(ResultMessager.ORDER_BUYER);

            if (buyer.Id != buyerFromOrder.Id)
                return new Result(ResultMessager.BUYER_ID_IS_NOT_ORDER_BUYER_ID, System.Net.HttpStatusCode.Conflict);

            if (!string.Equals(buyer.Login, buyerFromOrder.Login))
                return new Result(ResultMessager.BUYER_LOGIN_IS_NOT_ORDER_BUYER_LOGIN, System.Net.HttpStatusCode.Conflict);

            if (!string.Equals(buyer.Email, buyerFromOrder.Email))
                return new Result(ResultMessager.BUYER_EMAIL_IS_NOT_ORDER_BUYER_EMAIL, System.Net.HttpStatusCode.Conflict);

            if (buyer.Blocked)
                return new Result(ResultMessager.BUYER_IS_BLOCKED, System.Net.HttpStatusCode.Forbidden);

            return null;
        }


        /// <summary>
        /// Валидация позиций заказа
        /// </summary>
        /// <param name="orderPositions"> List(OrderPosition) - список позиций заказа </param>
        /// <returns> Result - при ошибке | null, если проверка пройдена </returns>
        private Result UnValidatedPositionResult(List<OrderPosition> orderPositions)
        {
            if (orderPositions == null)
                return new Result(ResultMessager.ORDER_POSITIONS_MUST_BE_NOT_NULL, System.Net.HttpStatusCode.BadRequest);

            if (!orderPositions.Any())
                return new Result(ResultMessager.ORDER_POSITIONS_MUST_HAVE_POSITION, System.Net.HttpStatusCode.BadRequest);

            foreach (var position in orderPositions)
            {
                if (position.ProductId <= 0)
                    return new Result(ResultMessager.PRODUCT_NOT_FOUND, System.Net.HttpStatusCode.NotFound);

                if (position.Price <= 0)
                    return new Result(ResultMessager.PRICE_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.BadRequest);

                if (position.Quantity <= 0)
                    return new Result(ResultMessager.QUANTITY_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.BadRequest);

                if (position.Cost <= 0)
                    return new Result(ResultMessager.COST_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.BadRequest);

                if (string.IsNullOrWhiteSpace(position.Currency))
                    return new Result(ResultMessager.CURRENCY_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);
            }

            return null;
        }

        /// <summary>
        /// Валидация товара перед заказом
        /// </summary>
        /// <param name="orderPositions"> List(OrderPosition) - список позиций заказа </param>
        /// <returns> Result - при ошибке | null, если проверка пройдена </returns>
        private Result UnValidatedProductResult(Product product)
        {
            if (product == null)
                return new Result(ResultMessager.PRODUCT_NOT_FOUND, System.Net.HttpStatusCode.NotFound);

            if (product.Archieved)
                return new Result(ResultMessager.PRODUCT_IS_ARCHIEVED, System.Net.HttpStatusCode.NotFound);

            if (product.PriceId == null)
                return new Result(ResultMessager.PRICE_ID_IS_NULL, System.Net.HttpStatusCode.NotFound);

            if (product.PricePerUnit == null)
                return new Result(ResultMessager.PRICE_PER_UNIT_IS_NULL, System.Net.HttpStatusCode.NotFound);

            return null;
        }
        #endregion private - validators
    }
}
