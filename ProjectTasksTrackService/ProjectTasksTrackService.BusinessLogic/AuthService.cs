using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ProjectTasksTrackService.Core.Auth;
using ProjectTasksTrackService.Core.Repositories;
using ProjectTasksTrackService.Core.Results;
using ProjectTasksTrackService.Core.Services;

namespace ProjectTasksTrackService.BusinessLogic
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private const string ISSUER = "MyAuthServer";
        private const string AUDIENCE = "MyAuthClient";
        private const string KEY = "ProjectTasksTrackService:Auth/Key{)(ws;lkfj43";
        private const int LOGIN_DEFAULT_TIMEOUT = 60;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<AuthResult> Register(AuthUser authUser)
        {
            if (authUser == null)
                throw new ArgumentNullException(ErrorStrings.AUTHUSER_PARAM_NAME);

            if (authUser.Id != 0)
                return new AuthResult(ErrorStrings.USER_ID_SHOULD_BE_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(authUser.Login))
                return new AuthResult(ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(authUser.UserName))
                return new AuthResult(ErrorStrings.USERNAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(authUser.Email))
                return new AuthResult(ErrorStrings.USERNAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(authUser.PasswordHash))
                return new AuthResult(ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            authUser.UpdateRole(newRole: $"?{authUser.Role}"); //? - запрошенная пользователем роль утверждается администратором

            var registerResult = await _authRepository.AddUser(authUser);

            return registerResult;
        }
        public async Task<AuthResult> Login(LoginData loginData)
        {
            if (loginData == null)
                throw new ArgumentNullException(ErrorStrings.LOGINDATA_PARAM_NAME);

            if (string.IsNullOrWhiteSpace(loginData.Login))
                return new AuthResult(ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(loginData.PasswordHash))
                return new AuthResult(ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);


            var user = await _authRepository.GetUser(loginData.Login);

            if (user == null)
                return new AuthResult(message: Core.ErrorStrings.USER_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);

            if (!string.Equals(loginData.PasswordHash, user.PasswordHash))
                return new AuthResult(message: Core.ErrorStrings.PASSWORD_HASH_MISMATCH, statusCode: System.Net.HttpStatusCode.Unauthorized);

            var claims = new List<Claim>
            {
                //new Claim(ClaimTypes.Name, loginData.Login),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var jwt = new JwtSecurityToken(
                issuer: ISSUER,
                audience: AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(loginData.TimeoutMinutes ?? LOGIN_DEFAULT_TIMEOUT)),
                signingCredentials: new SigningCredentials(key: GetSymmetricSecurityKey(), algorithm: SecurityAlgorithms.HmacSha256));

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new AuthResult(id: user.Id, message: Core.ErrorStrings.OK, statusCode: System.Net.HttpStatusCode.Created, token: token);
        }
        public Task<AuthResult> Logout(LogoutData logoutData)
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateResult> GrantRole(GrantRoleData grantRoleData)
        {
            if (grantRoleData == null)
                throw new ArgumentNullException(ErrorStrings.GRANTROLEDATA_PARAM_NAME);

            if (string.IsNullOrWhiteSpace(grantRoleData.Login))
                return new UpdateResult(ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(grantRoleData.PasswordHash))
                return new UpdateResult(ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(grantRoleData.GranterLogin))
                return new UpdateResult(ErrorStrings.GRANTERLOGIN_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            var user = await _authRepository.GetUser(grantRoleData.Id);

            if (user is null)
                return new UpdateResult(message: Core.ErrorStrings.USER_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);

            if (!string.Equals(user.Login, grantRoleData.Login))
                return new UpdateResult(message: Core.ErrorStrings.LOGIN_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);

            AuthUser granter = await _authRepository.GetUser(grantRoleData.GranterId);

            if (granter is null)
                return new UpdateResult(message: Core.ErrorStrings.USER_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);

            if (!string.Equals(granter.Login, grantRoleData.GranterLogin))
                return new UpdateResult(message: Core.ErrorStrings.GRANTERLOGIN_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);

            var updateResult = await _authRepository.GrantRole(
                id: grantRoleData.Id,
                role: grantRoleData.NewRole,
                granterId: grantRoleData.GranterId);

            return updateResult;
        }

        public async Task<UpdateResult> UpdateAccount(UpdateAccountData updateAccountData)
        {
            if (updateAccountData == null)
                throw new ArgumentNullException(ErrorStrings.LOGINDATA_PARAM_NAME);

            if (string.IsNullOrWhiteSpace(updateAccountData.Login))
                return new UpdateResult
                {
                    Message = ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            if (string.IsNullOrWhiteSpace(updateAccountData.UserName))
                return new UpdateResult
                {
                    Message = ErrorStrings.TASK_NAME_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            if (string.IsNullOrWhiteSpace(updateAccountData.Email))
                return new UpdateResult
                {
                    Message = ErrorStrings.EMAIL_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            if (string.IsNullOrWhiteSpace(updateAccountData.PasswordHash))
                return new UpdateResult
                {
                    Message = ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            return await _authRepository.UpdateUser(updateAccountData);
        }
        public async Task<DeleteResult> DeleteAccount(DeleteAccountData deleteAccountData)
        {
            throw new NotImplementedException();
        }

        private static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(KEY));
    }
}
