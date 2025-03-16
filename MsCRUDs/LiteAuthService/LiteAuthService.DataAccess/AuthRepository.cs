using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dapper;
using LiteAuthService.Core.Auth;
using LiteAuthService.Core.Repositories;
using LiteAuthService.Core.Results;
using LiteAuthService.DataAccess.Interfaces;

namespace LiteAuthService.DataAccess
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IDapperAsyncExecutor _dapperSqlExecutor;

        public AuthRepository(IDapperAsyncExecutor dapperSqlExecutor)
        {
            _dapperSqlExecutor = dapperSqlExecutor;
        }

        /// <summary>
        /// Добавление (регистрация) пользователя
        /// </summary>
        /// <param name="authUser"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<AuthResult> AddUser(AuthUser authUser)
        {
            var sql = @"INSERT INTO AuthUsers (
                Login,
                UserName,
                Email,
                PasswordHash,
                Nick,
                Phone,
                Role,
                GranterId,
                CreatedDt,
                LastUpdateDt)
                VALUES
                (@login, @userName, @email, @passwordHash, @nick, @phone, @role, @granterId, @creadedDt, @lastUpdateDt);
                SELECT u.* FROM AuthUsers u WHERE u.Id = SCOPE_IDENTITY();";
            var dp = new DynamicParameters(new
            {
                @login = authUser.Login,
                @userName = authUser.UserName,
                @email = authUser.Email,
                @passwordHash = authUser.PasswordHash,
                @nick = authUser.Nick,
                @phone = authUser.Phone,
                @role = authUser.Role,
                @granterId = authUser.GranterId,
                @creadedDt = DateTime.Now,
                @lastUpdateDt = DateTime.Now
            });

            var addedUser = (await _dapperSqlExecutor.QueryAsync<AuthUser>(sql, dp)).SingleOrDefault();
            if (addedUser is null)
                return new AuthResult($"Error: {addedUser} is null", HttpStatusCode.InternalServerError);
            
            return new AuthResult
            {
                Id = addedUser.Id,
                StatusCode = HttpStatusCode.Created,
            };
        }

        public async Task<DeleteResult> DeleteUser(int id)
        {
            var sql = @"DELETE FROM AuthUsers WHERE Id = @id";
            var dp = new DynamicParameters(new { id });
            var affectedRowsCount = await _dapperSqlExecutor.ExecuteAsync(sql, dp);
            if (affectedRowsCount != 1)
                throw new InvalidOperationException($"{Core.ErrorStrings.AFFECTED_DELETED_ROWS_COUNT_SHOULD_BE_ONE}{affectedRowsCount}");

            return new DeleteResult(Core.ErrorStrings.OK, HttpStatusCode.OK);
        }

        public async Task<AuthUser> GetUser(int id)
        {
            var sql = @"SELECT u.* FROM AuthUsers u WHERE u.Id = @id";
            var dp = new DynamicParameters(new { id });

            var authUser = (await _dapperSqlExecutor.QueryAsync<AuthUser>(sql, dp)).SingleOrDefault();
            
            return authUser;
        }

        public async Task<AuthUser> GetUser(string login)
        {
            var sql = @"SELECT u.* FROM AuthUsers u WHERE u.Login = @login";
            var dp = new DynamicParameters(new { login });

            var authUser = (await _dapperSqlExecutor.QueryAsync<AuthUser>(sql, dp)).SingleOrDefault();

            return authUser;
        }

        public async Task<UpdateResult> GrantRole(int id, string role, int granterId)
        {
            var sql = @"UPDATE AuthUsers SET Role = @role, GranterId = @granterId, LastUpdateDt = GETDATE() WHERE Id = @id";
            var dp = new DynamicParameters(new { role, granterId, id });
            var affectedRowsCount = await _dapperSqlExecutor.ExecuteAsync(sql, dp);
            if (affectedRowsCount != 1)
                throw new InvalidOperationException($"{Core.ErrorStrings.AFFECTED_UPDATED_ROWS_COUNT_SHOULD_BE_ONE}{affectedRowsCount}");
            
            return new UpdateResult(Core.ErrorStrings.USER_UPDATED, HttpStatusCode.OK);
        }

        public async Task<UpdateResult> UpdateUser(UpdateAccountData upd)
        {
            ArgumentNullException.ThrowIfNull(upd);

            var authUser = await GetUser(upd.Id);

            if (authUser is null)
                return new UpdateResult(Core.ErrorStrings.USER_NOT_FOUND, HttpStatusCode.NotFound);

            if (!string.Equals(upd.PasswordHash, authUser.PasswordHash))
                return new UpdateResult(Core.ErrorStrings.PASSWORD_HASH_MISMATCH, HttpStatusCode.Forbidden);
            
            bool update = false;
            if (!string.Equals(upd.Login, authUser.Login))          { authUser.UpdateLogin(upd.Login); update = true; }
            if (!string.Equals(upd.UserName, authUser.UserName))    { authUser.UpdateName(upd.UserName); update = true; }
            if (!string.Equals(upd.Email, authUser.Email))          { authUser.UpdateEmail(upd.Email); update = true; }
            if (!string.Equals(upd.NewPasswordHash, authUser.PasswordHash)) { authUser.UpdatePasswordHash(upd.PasswordHash); update = true; }
            if (!string.Equals(upd.Nick, authUser.Nick))            { authUser.UpdateNick(upd.Nick); update = true; }
            if (!string.Equals(upd.Phone, authUser.Phone))          { authUser.UpdatePhone(upd.Phone); update = true; }
            if (!string.Equals(upd.RequestedRole, authUser.Role))   { authUser.UpdateRole(newRole: $"?{upd.RequestedRole}"); update = true; } //? - запрошенная пользователем роль утверждается администратором

            if (!update)
                return new UpdateResult(Core.ErrorStrings.USER_IS_ACTUAL, HttpStatusCode.OK);
            
            authUser.UpdateLastUpdateDt(DateTime.Now.ToUniversalTime());

            var sql = @"UPDATE AuthUsers SET Login = @login, UserName = @userName, Email = @email, PasswordHash = @passwordHash, Role = @role,
                Nick = @nick,
                Phone = @phone,
                LastUpdateDt = GETDATE()
                WHERE Id = @id";
            var dp = new DynamicParameters(new
            {
                @id = authUser.Id,
                @login = authUser.Login,
                @userName = authUser.UserName,
                @email = authUser.Email,
                @passwordHash = authUser.PasswordHash,
                @nick = authUser.Nick,
                @phone = authUser.Phone,
                @role = authUser.Role,
                @lastUpdateDt = DateTime.Now
            });
            var affectedRowsCount = await _dapperSqlExecutor.ExecuteAsync(sql, dp);
            if (affectedRowsCount != 1)
                throw new InvalidOperationException($"{Core.ErrorStrings.AFFECTED_UPDATED_ROWS_COUNT_SHOULD_BE_ONE}{affectedRowsCount}");

            return new UpdateResult(Core.ErrorStrings.USER_UPDATED, HttpStatusCode.OK);
        }
    }
}
