using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsFeedSystem.Core;
using NewsFeedSystem.Core.Auth;
using NewsFeedSystem.Core.Repositories;
using NewsFeedSystem.Core.Results;

namespace NewsFeedSystem.DataAccess.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly NewsFeedSystemDbContext _dbContext;

        public AuthRepository(NewsFeedSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AuthResult> AddUser(AuthUser authUser)
        {
            ArgumentNullException.ThrowIfNull(authUser);

            var newAuthUserEntity = new Entities.AuthUser(
                id: authUser.Id,
                login: authUser.Login,
                userName: authUser.UserName,
                email: authUser.Email,
                passwordHash: authUser.PasswordHash,
                nick: authUser.Nick,
                phone: authUser.Phone,
                role: authUser.Role,
                granterId: authUser.GranterId,
                createdDt: authUser.CreatedDt.ToUniversalTime(),
                lastUpdateDt: authUser.LastUpdateDt?.ToUniversalTime());

            await _dbContext.AuthUsers.AddAsync(newAuthUserEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newAuthUserEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id
            return new AuthResult
            {
                Id = newAuthUserEntity.Id,
                StatusCode = HttpStatusCode.Created,
            };

        }

        public async Task<DeleteResult> DeleteUser(int id)
        {
            var authUserEntity = await GetAuthUserEntity(id);

            if (authUserEntity is null)
                return new DeleteResult(ErrorStrings.USER_NOT_FOUND, HttpStatusCode.NotFound);

            _dbContext.AuthUsers.Remove(authUserEntity);
            await _dbContext.SaveChangesAsync();

            return new DeleteResult(ErrorStrings.OK, HttpStatusCode.OK);
        }

        public async Task<AuthUser> GetUser(int id)
        {
            var authUserEntity = await GetAuthUserEntity(id);
            if (authUserEntity is null)
                return null;

            return AuthUser(authUserEntity);
        }

        public async Task<AuthUser> GetUser(string login)
        {
            var authUserEntity = await GetAuthUserEntity(login);
            if (authUserEntity is null)
                return null;

            return AuthUser(authUserEntity);
        }

        public async Task<UpdateResult> GrantRole(int id, string role, int granterId)
        {
            var granterUserEntity = await GetAuthUserEntity(granterId);
            if (granterUserEntity is null)
                return new UpdateResult(ErrorStrings.GRANTER_NOT_FOUND, HttpStatusCode.Unauthorized);

            var query = _dbContext.AuthUsers.Where(u => u.Id == id);
            var authUserEntity = await query.SingleOrDefaultAsync();
            if (authUserEntity is null)
                return new UpdateResult(ErrorStrings.USER_NOT_FOUND, HttpStatusCode.NotFound);

            authUserEntity.UpdateRole(role);
            if (!string.Equals(authUserEntity.GranterId, granterId))
                authUserEntity.UpdateGranterId(granterId);

            authUserEntity.UpdateLastUpdateDt(DateTime.Now.ToUniversalTime());

            await _dbContext.SaveChangesAsync();

            return new UpdateResult(ErrorStrings.USER_UPDATED, HttpStatusCode.OK);
        }

        public async Task<UpdateResult> UpdateUser(UpdateAccountData upd)
        {
            ArgumentNullException.ThrowIfNull(upd);

            var authUserEntity = await _dbContext.AuthUsers
                .SingleOrDefaultAsync(u => u.Id == upd.Id);

            if (authUserEntity is null)
                return new UpdateResult(ErrorStrings.USER_NOT_FOUND, HttpStatusCode.NotFound);

            if (!string.Equals(upd.PasswordHash, authUserEntity.PasswordHash))
                return new UpdateResult(ErrorStrings.PASSWORD_HASH_MISMATCH, HttpStatusCode.Forbidden);

            if (!string.Equals(upd.Login, authUserEntity.Login)) authUserEntity.UpdateLogin(upd.Login);
            if (!string.Equals(upd.UserName, authUserEntity.UserName)) authUserEntity.UpdateName(upd.UserName);
            if (!string.Equals(upd.Email, authUserEntity.Email)) authUserEntity.UpdateEmail(upd.Email);
            if (!string.Equals(upd.NewPasswordHash, authUserEntity.PasswordHash)) authUserEntity.UpdatePasswordHash(upd.PasswordHash);
            if (!string.Equals(upd.Nick, authUserEntity.Nick)) authUserEntity.UpdateNick(upd.Nick);
            if (!string.Equals(upd.Phone, authUserEntity.Phone)) authUserEntity.UpdatePhone(upd.Phone);
            if (!string.Equals(upd.RequestedRole, authUserEntity.Role)) authUserEntity.UpdateRole(newRole: $"?{upd.RequestedRole}"); //? - запрошенная пользователем роль утверждается администратором

            if (_dbContext.ChangeTracker.HasChanges())
            {
                authUserEntity.UpdateLastUpdateDt(DateTime.Now.ToUniversalTime());
                await _dbContext.SaveChangesAsync();
                return new UpdateResult(ErrorStrings.USER_UPDATED, HttpStatusCode.OK);
            }

            return new UpdateResult(ErrorStrings.USER_IS_ACTUAL, HttpStatusCode.OK);
        }

        private async Task<Entities.AuthUser> GetAuthUserEntity(int id)
        {
            var query = _dbContext.AuthUsers.AsNoTracking().Where(u => u.Id == id);
            var authUserEntity = await query.SingleOrDefaultAsync();

            return authUserEntity;
        }

        private async Task<Entities.AuthUser> GetAuthUserEntity(string login)
        {
            var query = _dbContext.AuthUsers.AsNoTracking().Where(u => u.Login.Equals(login));
            var authUserEntity = await query.SingleOrDefaultAsync();

            return authUserEntity;
        }

        private static AuthUser AuthUser(Entities.AuthUser userEntity) =>
            new AuthUser(
                id: userEntity.Id,
                login: userEntity.Login,
                userName: userEntity.UserName,
                email: userEntity.Email,
                passwordHash: userEntity.PasswordHash,
                nick: userEntity.Nick,
                phone: userEntity.Phone,
                role: userEntity.Role,
                granterId: userEntity.GranterId,
                createdDt: userEntity.CreatedDt.ToLocalTime(),
                lastUpdateDt: userEntity.LastUpdateDt?.ToLocalTime());

    }
}
