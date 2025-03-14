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
                throw new ArgumentNullException(nameof(addedUser));
            
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

        public Task<UpdateResult> GrantRole(int id, string role, int granterId)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateResult> UpdateUser(UpdateAccountData authUser)
        {
            throw new NotImplementedException();
        }
    }
}
