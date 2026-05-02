using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class TradeService : ITradeService
    {
        private readonly ITradeRepository _tradeRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IBuyersRepository _buyersRepository;

        /// <summary> Допустимая погрешность рассинхронизации времени, с </summary>
        const int PERMISSIBLE_TIME_DESYNC_SECONDS = 36000;

        public TradeService(
            ITradeRepository tradeRepository,
            IBuyersRepository buyersRepository,
            IProductsRepository productsRepository)
        {
            _tradeRepository = tradeRepository;
            _buyersRepository = buyersRepository;
            _productsRepository = productsRepository;
        }

        public async Task<Result> AddPayment(Trade trade)
        {
            var errorResult = UnValidatedTradeResult(trade);
            if (errorResult != null)
                return errorResult;

            if (trade.BuyerId != null)
            {
                var buyer = await _buyersRepository.GetUser(trade.BuyerId.Value);

                errorResult = UnValidatedBuyerResult(buyer);
                if (errorResult != null)
                    return errorResult;
            }

            for (int pos = 0; pos < trade.Positions.Count; pos++)
            {
                var position = trade.Positions[pos];

                var product = await _productsRepository.GetProductById(position.ProductId);
                errorResult = UnValidatedProductResult(product);
                if (errorResult != null)
                    return errorResult;

                position.FillByProduct(product);
            }

            var createResult = await _tradeRepository.AddPayment(trade);

            return createResult;
        }

        public async Task<Result> AddRefund(Trade trade)
        {
            var errorResult = UnValidatedRefundResult(trade);
            if (errorResult != null)
                return errorResult;

            if (trade.BuyerId != null)
            {
                var buyer = await _buyersRepository.GetUser(trade.BuyerId.Value);

                errorResult = UnValidatedBuyerResult(buyer);
                if (errorResult != null)
                    return errorResult;
            }

            for (int pos = 0; pos < trade.Positions.Count; pos++)
            {
                var position = trade.Positions[pos];

                var product = await _productsRepository.GetProductById(position.ProductId);
                errorResult = UnValidatedProductResult(product);
                if (errorResult != null)
                    return errorResult;

                position.FillByProduct(product);
            }

            var createResult = await _tradeRepository.AddRefund(trade);

            return createResult;
        }

        public async Task<Trade> GetTransactionInfoById(
            long transactionId,
            uint? buyerId)
        {
            var transaction = await _tradeRepository.GetTransactionInfoById(transactionId);

            if (transaction == null)
                return null;

            if (buyerId != null && buyerId != transaction.BuyerId)
                return null;

            return transaction;
        }

        public async Task<IEnumerable<Trade>> GetTransactionInfosByBuyerId(
            uint buyerId,
            DateTime createdFromDt,
            DateTime? createdToDt,
            uint byPage,
            uint page)
        {
            var take = byPage;
            var skip = page > 1 ? (page - 1) * byPage : 0;

            return await _tradeRepository.GetTransactionInfosByBuyerId(
                buyerId: buyerId,
                createdFromDt: createdFromDt,
                createdToDt: createdToDt,
                take: take,
                skipCount: skip);
        }

        public async Task<IEnumerable<Trade>> GetTransactionInfosByOrderId(
            uint orderId,
            uint? buyerId)
        {
            return await _tradeRepository.GetTransactionInfosByOrderId(
                orderId: orderId,
                buyerId: buyerId);
        }

        #region private - validators
        /// <summary>
        /// Валидация данных покупателя
        /// </summary>
        /// <param name="buyer"> Buyer buyer - данные покупателя из БД </param>
        /// <returns> Result - при ошибке | null, если проверка пройдена </returns>
        /// <exception cref="ArgumentNullException"></exception>
        private Result UnValidatedBuyerResult(Buyer buyer)
        {
            if (buyer == null)
                return new Result(ResultMessager.BUYER_NOT_FOUND, System.Net.HttpStatusCode.NotFound);

            if (buyer.Blocked)
                return new Result(ResultMessager.BUYER_IS_BLOCKED, System.Net.HttpStatusCode.Forbidden);

            return null;
        }


        /// <summary>
        /// Валидация данных запроса на транзакцию, общих для оплаты и возврата
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private Result UnValidatedTradeResult(Trade trade, bool isPayment = true)
        {
            if (trade == null)
                throw new ArgumentNullException(ResultMessager.TRADE_RARAM_NAME);

            if (trade.BuyerId == 0)
                return new Result(ResultMessager.BUYER_NOT_FOUND, System.Net.HttpStatusCode.NotFound);

            if (trade.Amount <= 0)
                return new Result(ResultMessager.AMOUNT_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(trade.Currency))
                return new Result(ResultMessager.CURRENCY_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(trade.PaymentInfo))
                return new Result(ResultMessager.PAYMENT_INFO_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(trade.ExtraInfo))
                return new Result(ResultMessager.EXTRA_INFO_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (isPayment && trade.Archived)
                return new Result(ResultMessager.IMPOSSIBLE_TO_ADD_ARCHIVED_PAYMENT, System.Net.HttpStatusCode.BadRequest);

            if (Math.Abs((DateTime.Now - trade.Created).TotalSeconds) > PERMISSIBLE_TIME_DESYNC_SECONDS)
                return new Result($"{ResultMessager.TIME_DESYNCHRONIZATION_MORE_THAN_PERMISSIBLE_TIME_DESYNC_SECONDS} {PERMISSIBLE_TIME_DESYNC_SECONDS}",
                    statusCode: System.Net.HttpStatusCode.Conflict);

            var errorResult = UnValidatedPositionResult(trade.Positions);
            if (errorResult != null)
                return errorResult;

            return null;
        }

        /// <summary>
        /// Валидация данных запроса на возврат
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private Result UnValidatedRefundResult(Trade trade)
        {
            var unValidatedResult = UnValidatedTradeResult(trade);
            if (unValidatedResult != null)
                return unValidatedResult;

            if (trade.RefundAmount == null || trade.RefundAmount <= 0)
                return new Result(ResultMessager.REFUNDAMOUNT_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(trade.RefundInfo))
                return new Result(ResultMessager.EXTRA_INFO_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

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
        /// Валидация данных товара перед оплатой заказа/чека/квитанции
        /// </summary>
        /// <param name="orderPositions"> List(OrderPosition) - список позиций заказа </param>
        /// <returns> Result - при ошибке | null, если проверка пройдена </returns>
        private Result UnValidatedProductResult(Product product, bool checkProductFiels = false)
        {
            if (product == null || product.Id == 0)
                return new Result(ResultMessager.PRODUCT_NOT_FOUND, System.Net.HttpStatusCode.NotFound);

            if (!checkProductFiels)
                return null;

            //после заказа/покупки могут меняться для новых заказов/покупок:
            if (product.Archived)
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
