using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;

namespace ShopServices.DataAccess.Repositories
{
    public class TradeRepository : ITradeRepository
    {
        private readonly ShopServicesDbContext _dbContext;

        public TradeRepository(ShopServicesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> AddPayment(Trade newTrade)
        {
            ArgumentNullException.ThrowIfNull(newTrade);

            var newTradeEntity = new Entities.Trade
            (
                id: newTrade.Id,
                orderId: newTrade.OrderId,
                buyerId: newTrade.BuyerId,
                amount: newTrade.Amount,
                currency: newTrade.Currency,
                created: newTrade.Created,
                positions: newTrade.GetPositionsStr(),
                paymentInfo: newTrade.PaymentInfo,
                extraInfo: newTrade.ExtraInfo,
                comment: newTrade.Comment,
                shopId: newTrade.ShopId,
                managerId: newTrade.ManagerId,
                courierId: newTrade.CourierId,
                refundDateTime: null,
                refundAmount: null,
                refundInfo: null,
                archived: false
            );

            await _dbContext.Trades.AddAsync(newTradeEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newTradeEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id транзакции

            await _dbContext.SaveChangesAsync();

            return new Result
            {
                Id = (ulong)newTradeEntity.Id,
                BuyerId = newTradeEntity.BuyerId,
                StatusCode = HttpStatusCode.Created,
                Message = ResultMessager.OK
            };
        }

        public async Task<Result> AddRefund(Trade newTrade)
        {
            ArgumentNullException.ThrowIfNull(newTrade);

            var newTradeEntity = new Entities.Trade
            (
                id: newTrade.Id,
                orderId: newTrade.OrderId,
                buyerId: newTrade.BuyerId,
                amount: newTrade.Amount,
                currency: newTrade.Currency,
                created: newTrade.Created,
                positions: newTrade.GetPositionsStr(),
                paymentInfo: newTrade.PaymentInfo,
                extraInfo: newTrade.ExtraInfo,
                comment: newTrade.Comment,
                shopId: newTrade.ShopId,
                managerId: newTrade.ManagerId,
                courierId: newTrade.CourierId,
                refundDateTime: newTrade.RefundDateTime,
                refundAmount: newTrade.RefundAmount,
                refundInfo: newTrade.RefundInfo,
                archived: newTrade.Archived
            );

            await _dbContext.Trades.AddAsync(newTradeEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newTradeEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id транзакции

            await _dbContext.SaveChangesAsync();

            return new Result
            {
                Id = (ulong)newTradeEntity.Id,
                BuyerId = newTradeEntity.BuyerId,
                StatusCode = HttpStatusCode.Created,
                Message = ResultMessager.OK
            };
        }

        public async Task<Trade?> GetTransactionInfoById(long transactionId)
        {
            var tradeEntity = await GetTradeEntity(transactionId, asNoTracking: true);
            if (tradeEntity is null)
                return null;

            return GetCoreTrade(tradeEntity);
        }

        public async Task<IEnumerable<Trade>> GetTransactionInfosByBuyerId(
            uint buyerId,
            DateTime createdFromDt,
            DateTime? createdToDt,
            uint take,
            uint skipCount)
        {
            List<Entities.Trade> entitiesTrades = await GetIQueryableTradesByByyer(buyerId, createdFromDt, createdToDt)
                .Skip((int)skipCount)
                .Take((int)take)
                .ToListAsync();

            return entitiesTrades.Select(order => GetCoreTrade(order));
        }
    
        /// <summary> Маппер Entities.Trade - Core.Models.Trade </summary>
        /// <param name="tradeEntity"> Entities.Trade - транзакция оплаты/возврата (из БД) </param>
        /// <returns> Core.Models.Trade - транзакция оплаты/возврата </returns>
        private static Trade GetCoreTrade(Entities.Trade tradeEntity)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
            return new Trade(
                id: tradeEntity.Id,
                orderId: tradeEntity.OrderId,
                buyerId: tradeEntity.BuyerId,
                positions: JsonSerializer.Deserialize<List<OrderPosition>>(tradeEntity.Positions, options),
                amount: tradeEntity.Amount,
                currency: tradeEntity.Currency,
                created: tradeEntity.Created,
                paymentInfo: tradeEntity.PaymentInfo,
                extraInfo: tradeEntity.ExtraInfo,
                comment: tradeEntity.Comment,
                shopId: tradeEntity.ShopId,
                managerId: tradeEntity.ManagerId,
                courierId: tradeEntity.CourierId,
                refundDateTime: tradeEntity.RefundDateTime,
                refundAmount: tradeEntity.RefundAmount,
                refundInfo: tradeEntity.RefundInfo,
                archived: tradeEntity.Archived);
        }

        /// <summary> Получение информации о транзакциях покупателя для указанного временного интервала </summary>
        /// <param name="buyerId"> Id покупателя </param>
        /// <param name="createdFromDt"> Создан от какого времени </param>
        /// <param name="createdToDt"> Создан до какого времени </param>
        /// <returns> IQueryable(Entities.Order) </returns>
        private IQueryable<Entities.Trade> GetIQueryableTradesByByyer(
            uint buyerId,
            DateTime createdFromDt,
            DateTime? createdToDt)
        {
            createdFromDt = createdFromDt.ToUniversalTime();
            createdToDt = createdToDt?.ToUniversalTime();

            Expression<Func<Entities.Trade, bool>> expressionWhereCreatedToDt = createdToDt == null ?
                    trade => (trade.Created <= DateTime.Now.ToUniversalTime()) :
                    trade => trade.Created <= createdToDt;

            return _dbContext.Trades.AsNoTracking()
                .Where(trade => trade.BuyerId == buyerId)
                .Where(trade => trade.Created >= createdFromDt)
                .Where(expressionWhereCreatedToDt);
        }

        private async Task<Entities.Trade?> GetTradeEntity(long tradeId, bool asNoTracking)
        {
            var query = asNoTracking ?
                _dbContext.Trades.AsNoTracking().Where(trade => trade.Id == tradeId) :
                _dbContext.Trades.Where(trade => trade.Id == tradeId);

            var tradeEntity = await query.SingleOrDefaultAsync();

            return tradeEntity;
        }
    }
}