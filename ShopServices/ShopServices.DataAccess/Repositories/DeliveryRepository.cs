using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Enums;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;

namespace ShopServices.DataAccess.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly ShopServicesDbContext _dbContext;

        public DeliveryRepository(ShopServicesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Create(Delivery newDelivery)
        {
            ArgumentNullException.ThrowIfNull(newDelivery);

            var newDeliveryEntity = new Entities.Delivery
            (
                id: 0,
                buyerId: newDelivery.BuyerId,
                orderId: newDelivery.OrderId,
                regionCode: newDelivery.RegionCode,
                address: newDelivery.Address,
                managerId: newDelivery.ManagerId,
                courierId: newDelivery.CourierId,
                paymentInfo: newDelivery.PaymentInfo,
                massInGrams: newDelivery.MassInGrams,
                dimensions: newDelivery.Dimensions,
                fromWarehouseId: newDelivery.FromWarehouseId,
                toWarehouseId: newDelivery.ToWarehouseId,
                fromShopId: newDelivery.FromShopId,
                toShopId: newDelivery.ToShopId,
                comment: newDelivery.Comment,
                status: (uint)newDelivery.Status,
                transferId: newDelivery.TransferId,
                created: DateTime.Now.ToUniversalTime()
            );

            await _dbContext.Deliveries.AddAsync(newDeliveryEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newDeliveryEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id заказа

            return new Result
            {
                Id = newDeliveryEntity.Id,
                StatusCode = HttpStatusCode.Created,
                Message = ResultMessager.OK
            };
        }
        public async Task<Delivery?> GetDeliveryById(uint deliveryId)
        {
            var deliveryEntity = await GetDeliveryEntity(deliveryId, asNoTracking: true);
            if (deliveryEntity is null)
                return null;

            return GetCoreDelivery(deliveryEntity);
        }

        public async Task<IEnumerable<Delivery>> GetDeliveries(
            uint? regionCode,
            string addressSubStr,
            uint? buyerId,
            uint take,
            uint skipCount,
            bool ignoreCase = true)
        {
            var limitCount = take > 100 ? 100 : take;
            List<Entities.Delivery> entityDeliverysLst;

            if (string.IsNullOrWhiteSpace(addressSubStr) && buyerId == null)
            {
                entityDeliverysLst = regionCode == null ?
                    await _dbContext.Deliveries.AsNoTracking()
                    .Skip((int)skipCount).Take((int)limitCount).ToListAsync() :

                    await _dbContext.Deliveries.AsNoTracking().Where(delivery => delivery.RegionCode == regionCode)
                    .Skip((int)skipCount).Take((int)limitCount).ToListAsync();

                if (entityDeliverysLst.Count == 0)
                    return [];

                return entityDeliverysLst.Select(delivery => GetCoreDelivery(delivery));
            }
            Expression<Func<Entities.Delivery, bool>> expressionWhereBuyerId = delivery => delivery.BuyerId == buyerId;

            if (string.IsNullOrWhiteSpace(addressSubStr))
            {
                entityDeliverysLst = await _dbContext.Deliveries
                        .AsNoTracking()
                        .Where(delivery => delivery.RegionCode == regionCode)
                        .Where(expressionWhereBuyerId).Skip((int)skipCount).Take((int)limitCount).ToListAsync();

                if (entityDeliverysLst.Count == 0)
                    return [];

                return entityDeliverysLst.Select(delivery => GetCoreDelivery(delivery));
            }

            //addressSubStr задан
            Expression<Func<Entities.Delivery, bool>> expressionWhereAddress = ignoreCase ?
                s => EF.Functions.Like(s.Address!.ToLower(), $"%{addressSubStr.ToLower()}%") :
                s => s.Address!.Contains(addressSubStr);

            entityDeliverysLst = buyerId == null ?
                await _dbContext.Deliveries
                        .AsNoTracking()
                        .Where(s => s.RegionCode == regionCode)
                        .Where(expressionWhereAddress).Skip((int)skipCount).Take((int)limitCount).ToListAsync() :
                await _dbContext.Deliveries
                        .AsNoTracking()
                        .Where(s => s.RegionCode == regionCode)
                        .Where(expressionWhereAddress)
                        .Where(expressionWhereBuyerId).Skip((int)skipCount).Take((int)limitCount)
                        .ToListAsync();

            if (entityDeliverysLst.Count == 0)
                return [];

            return entityDeliverysLst.Select(delivery => GetCoreDelivery(delivery));
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesForOrder(
            uint orderId,
            uint take,
            uint skipCount)
        {
            var limitCount = take > 100 ? 100 : take;

            List<Entities.Delivery> entityDeliverysLst = await _dbContext.Deliveries.AsNoTracking().Where(delivery => delivery.OrderId == orderId)
                    .Skip((int)skipCount).Take((int)limitCount).ToListAsync();

            return entityDeliverysLst.Select(delivery => GetCoreDelivery(delivery));
        }

        public async Task<Result> UpdateDelivery(Delivery upd)
        {
            ArgumentNullException.ThrowIfNull(upd);

            var deliveryEntity = await _dbContext.Deliveries
                .SingleOrDefaultAsync(delivery => delivery.Id == upd.Id);

            if (deliveryEntity is null)
                return new Result(ResultMessager.NOT_FOUND, HttpStatusCode.NotFound);

            if (upd.RegionCode != deliveryEntity.RegionCode) deliveryEntity.UpdateRegionCode(upd.RegionCode);
            if (!string.Equals(upd.Address, deliveryEntity.Address)) deliveryEntity.UpdateAddress(upd.Address);
            if (upd.ManagerId != deliveryEntity.ManagerId) deliveryEntity.UpdateManagerId(upd.ManagerId);
            if (upd.CourierId != deliveryEntity.CourierId) deliveryEntity.UpdateCourierId(upd.CourierId);
            if (upd.MassInGrams != deliveryEntity.MassInGrams) deliveryEntity.UpdateMassInGrams(upd.MassInGrams);
            if (!string.Equals(upd.Dimensions, deliveryEntity.Dimensions)) deliveryEntity.UpdateDimensions(upd.Dimensions);
            if (!string.Equals(upd.PaymentInfo, deliveryEntity.PaymentInfo)) deliveryEntity.UpdatePaymentInfo(upd.PaymentInfo);

            if (upd.FromWarehouseId != deliveryEntity.FromWarehouseId) deliveryEntity.UpdateFromWarehouseId(upd.FromWarehouseId);
            if (upd.ToWarehouseId != deliveryEntity.ToWarehouseId) deliveryEntity.UpdateToWarehouseId(upd.ToWarehouseId);
            
            if (upd.FromShopId != deliveryEntity.FromShopId) deliveryEntity.UpdateFromShopId(upd.FromShopId);
            if (upd.ToShopId != deliveryEntity.ToShopId) deliveryEntity.UpdateToShopId(upd.ToShopId);
            if (upd.TransferId != deliveryEntity.TransferId) deliveryEntity.UpdateTransferId(upd.TransferId);
            if (!string.Equals(upd.Comment, deliveryEntity.Comment)) deliveryEntity.UpdateComment(upd.Comment);
            
            if (deliveryEntity.Status != (uint)upd.Status)
                deliveryEntity.UpdateStatus((uint)upd.Status);
            
            if (_dbContext.ChangeTracker.HasChanges())
            {
                deliveryEntity.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.DELIVERY_UPDATED, HttpStatusCode.OK);
            }

            return new Result(ResultMessager.DELIVERY_IS_ACTUAL, HttpStatusCode.OK);
        }
        
        public async Task<Result> ArchiveDeliveryById(uint deliveryId)
        {
            var entityDelivery = await _dbContext.Deliveries
                .SingleOrDefaultAsync(delivery => delivery.Id == deliveryId);

            if (entityDelivery is null)
                return new Result(ResultMessager.NOT_FOUND, HttpStatusCode.NotFound);

            if (entityDelivery.Status != (uint)DeliveryStatus.Canceled)
            {
                entityDelivery.UpdateStatus((uint)DeliveryStatus.Canceled);

                await _dbContext.SaveChangesAsync();
            }

            return new Result(ResultMessager.OK, HttpStatusCode.OK);
        }

        /// <summary> Маппер Entities.Delivery - Core.Models.Delivery </summary>
        /// <param name="deliveryEntity"> Entities.Delivery - перевозка(доставка) (из БД) </param>
        /// <returns> Core.Models.Delivery - перевозка(доставка) </returns>
        private static Delivery GetCoreDelivery(Entities.Delivery deliveryEntity)
        {
            return new Delivery(
                id: deliveryEntity.Id,
                buyerId: deliveryEntity.BuyerId,
                orderId: deliveryEntity.OrderId,
                regionCode: deliveryEntity.RegionCode,
                address: deliveryEntity.Address,
                managerId: deliveryEntity.ManagerId!.Value,
                courierId: deliveryEntity.CourierId,
                paymentInfo: deliveryEntity.PaymentInfo,
                massInGrams: deliveryEntity.MassInGrams,
                dimensions: deliveryEntity.Dimensions,
                fromWarehouseId: deliveryEntity.FromWarehouseId,
                toWarehouseId: deliveryEntity.ToWarehouseId,
                fromShopId: deliveryEntity.FromShopId,
                toShopId: deliveryEntity.ToShopId,
                comment: deliveryEntity.Comment,
                status: (DeliveryStatus)deliveryEntity.Status,
                transferId: deliveryEntity.TransferId,
                createdDt: deliveryEntity.Created,
                updated: deliveryEntity.Updated);
        }

        private async Task<Entities.Delivery?> GetDeliveryEntity(uint deliveryId, bool asNoTracking)
        {
            var query = asNoTracking ?
                _dbContext.Deliveries.AsNoTracking().Where(delivery => delivery.Id == deliveryId) :
                _dbContext.Deliveries.Where(delivery => delivery.Id == deliveryId);

            var deliveryEntity = await query.SingleOrDefaultAsync();

            return deliveryEntity;
        }
    }
}
