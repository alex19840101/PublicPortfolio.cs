using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopServices.Core;
using ShopServices.Core.Auth;
using ShopServices.Core.Repositories;
using ShopServices.Abstractions.Auth;
using ShopServices.Abstractions;
using ShopServices.Core.Models;
using System.Collections.Generic;
using ShopServices.Core.Enums;

namespace ShopServices.DataAccess.Repositories
{
    public class BuyersRepository : IBuyersRepository
    {
        private readonly ShopServicesDbContext _dbContext;

        public BuyersRepository(ShopServicesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AuthResult> AddUser(Buyer buyer)
        {
            ArgumentNullException.ThrowIfNull(buyer);

            var newBuyerEntity = new Entities.Buyer(
                id: buyer.Id,
                login: buyer.Login,
                name: buyer.Name,
                surname: buyer.Surname,
                address: buyer.Address,
                email: buyer.Email,
                passwordHash: buyer.PasswordHash,
                nick: buyer.Nick,
                phone: buyer.Phones,
                telegramChatId: buyer.TelegramChatId,
                notificationMethods: buyer.NotificationMethods.Select(n => (byte)n).ToList(),
                discountGroups: null,
                granterId: buyer.GranterId,
                createdDt: buyer.Created.ToUniversalTime(),
                lastUpdateDt: buyer.Updated?.ToUniversalTime(),
                blocked: buyer.Blocked);

            await _dbContext.Buyers.AddAsync(newBuyerEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newBuyerEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id
            return new AuthResult
            {
                Id = newBuyerEntity.Id,
                StatusCode = HttpStatusCode.Created,
                Message = ResultMessager.OK
            };
        }

        public async Task<Result> DeleteUser(uint id)
        {
            var buyerEntity = await GetBuyerEntity(id, asNoTracking: false);

            if (buyerEntity is null)
                return new Result(ResultMessager.USER_NOT_FOUND, HttpStatusCode.NotFound);

            buyerEntity.UpdateName(newName: "");
            buyerEntity.UpdateSurname(newSurname: "");
            buyerEntity.UpdateNick(newNick: "");
            buyerEntity.UpdateAddress(newAddress: null);
            buyerEntity.UpdateEmail(newEmail: "");
            buyerEntity.UpdatePhone(newPhone: "");

            buyerEntity.UpdateBlocked(blocked: true);

            await _dbContext.SaveChangesAsync();

            return new Result(ResultMessager.OK, HttpStatusCode.OK);
        }

        public async Task<Buyer?> GetUser(uint id)
        {
            var buyerEntity = await GetBuyerEntity(id, asNoTracking: true);
            if (buyerEntity is null)
                return null;

            return Buyer(buyerEntity);
        }

        public async Task<Buyer?> GetUserForUpdate(uint id)
        {
            var buyerEntity = await GetBuyerEntity(id, asNoTracking: false);
            if (buyerEntity is null)
                return null;

            return Buyer(buyerEntity);
        }

        public async Task<Buyer?> GetUser(string login)
        {
            var buyerEntity = await GetBuyerEntity(login);
            if (buyerEntity is null)
                return null;

            return Buyer(buyerEntity);
        }

        public async Task<Result> ChangeDiscountGroups(uint id, List<uint> discountGroups, uint granterId)
        {
            var granterUserEntity = await GetBuyerEntity(granterId, asNoTracking: false);
            if (granterUserEntity is null)
                return new Result(ResultMessager.GRANTER_NOT_FOUND, HttpStatusCode.Unauthorized);

            var query = _dbContext.Buyers.Where(b => b.Id == id);
            var buyerEntity = await query.SingleOrDefaultAsync();
            if (buyerEntity is null)
                return new Result(ResultMessager.USER_NOT_FOUND, HttpStatusCode.NotFound);

            buyerEntity.UpdateDiscountGroups(discountGroups);
            if (!Equals(buyerEntity.GranterId, granterId))
                buyerEntity.UpdateGranterId(granterId);

            buyerEntity.UpdateLastUpdateDt(DateTime.Now.ToUniversalTime());

            await _dbContext.SaveChangesAsync();

            return new Result(ResultMessager.USER_UPDATED, HttpStatusCode.OK);
        }

        public async Task<Result> UpdateUser(UpdateAccountData upd)
        {
            ArgumentNullException.ThrowIfNull(upd);

            var buyerEntity = await _dbContext.Buyers
                .SingleOrDefaultAsync(b => b.Id == upd.Id);

            if (buyerEntity is null)
                return new Result(ResultMessager.USER_NOT_FOUND, HttpStatusCode.NotFound);

            if (!string.Equals(upd.PasswordHash, buyerEntity.PasswordHash))
                return new Result(ResultMessager.PASSWORD_HASH_MISMATCH, HttpStatusCode.Forbidden);

            if (!string.Equals(upd.Login, buyerEntity.Login)) buyerEntity.UpdateLogin(upd.Login);
            if (!string.Equals(upd.Name, buyerEntity.Name)) buyerEntity.UpdateName(upd.Name);
            if (!string.Equals(upd.Surname, buyerEntity.Surname)) buyerEntity.UpdateSurname(upd.Surname);
            if (!string.Equals(upd.Email, buyerEntity.Email)) buyerEntity.UpdateEmail(upd.Email);
            if (!string.Equals(upd.NewPasswordHash, buyerEntity.PasswordHash)) buyerEntity.UpdatePasswordHash(upd.PasswordHash);
            if (!string.Equals(upd.Nick, buyerEntity.Nick)) buyerEntity.UpdateNick(upd.Nick);
            if (!string.Equals(upd.Phone, buyerEntity.Phone)) buyerEntity.UpdatePhone(upd.Phone);
            if (!string.Equals(upd.Address, buyerEntity.Address)) buyerEntity.UpdateAddress(upd.Address);

            if (_dbContext.ChangeTracker.HasChanges())
            {
                buyerEntity.UpdateLastUpdateDt(DateTime.Now.ToUniversalTime());
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.USER_UPDATED, HttpStatusCode.OK);
            }

            return new Result(ResultMessager.USER_IS_ACTUAL, HttpStatusCode.OK);
        }

        private async Task<Entities.Buyer?> GetBuyerEntity(uint id, bool asNoTracking)
        {
            var query = asNoTracking ?
                _dbContext.Buyers.AsNoTracking().Where(b => b.Id == id) :
                _dbContext.Buyers.Where(b => b.Id == id);

            var buyerEntity = await query.SingleOrDefaultAsync();

            return buyerEntity;
        }

        private async Task<Entities.Buyer?> GetBuyerEntity(string login)
        {
            var query = _dbContext.Buyers.AsNoTracking().Where(b => b.Login.Equals(login));
            var buyerEntity = await query.SingleOrDefaultAsync();

            return buyerEntity;
        }

        private static Buyer Buyer(Entities.Buyer userEntity) =>
            new Buyer(
                id: userEntity.Id,
                login: userEntity.Login,
                name: userEntity.Name,
                surname: userEntity.Surname,
                address: userEntity.Address,
                email: userEntity.Email,
                passwordHash: userEntity.PasswordHash,
                nick: userEntity.Nick,
                phones: userEntity.Phone,
                discountGroups: userEntity.DiscountGroups,
                granterId: userEntity.GranterId,
                created: userEntity.CreatedDt.ToLocalTime(),
                updated: userEntity.LastUpdateDt?.ToLocalTime());

    }
}
