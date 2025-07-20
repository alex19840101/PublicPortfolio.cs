using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using NewsFeedSystem.Core;
using NewsFeedSystem.Core.Auth;
using NewsFeedSystem.Core.Repositories;
using NewsFeedSystem.Core.Results;
using NewsFeedSystem.Core.Services;

namespace NewsFeedSystem.BusinessLogic
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly string _key;
        private const int LOGIN_DEFAULT_TIMEOUT = 60;

        public AuthService(IAuthRepository authRepository, TokenValidationParameters tokenValidationParameters, string key)
        {
            _authRepository = authRepository;
            _tokenValidationParameters = tokenValidationParameters;
            _key = key;
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
                return new AuthResult(ErrorStrings.EMAIL_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(authUser.PasswordHash))
                return new AuthResult(ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            authUser.UpdateRole(newRole: $"?{authUser.Role}"); //? - запрошенная пользователем роль утверждается администратором

            var existingUser = await _authRepository.GetUser(authUser.Login);
            if (existingUser != null)
            {
                if (!existingUser.IsEqualIgnoreIdAndDt(authUser))
                    return new AuthResult(ErrorStrings.CONFLICT, System.Net.HttpStatusCode.Conflict);

                return new AuthResult(ErrorStrings.ALREADY_EXISTS, System.Net.HttpStatusCode.Created, id: existingUser.Id);
            }

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
                return new AuthResult(message: ErrorStrings.USER_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);

            if (!string.Equals(loginData.PasswordHash, user.PasswordHash))
                return new AuthResult(message: ErrorStrings.PASSWORD_HASH_MISMATCH, statusCode: System.Net.HttpStatusCode.Unauthorized);

            var claims = new List<Claim>
            {
                //new Claim(ClaimTypes.Name, loginData.Login),
                new Claim(ClaimTypes.Role, user.Role ?? string.Empty)
            };

            var jwt = new JwtSecurityToken(
                issuer: _tokenValidationParameters.ValidIssuer,
                audience: _tokenValidationParameters.ValidAudience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(loginData.TimeoutMinutes ?? LOGIN_DEFAULT_TIMEOUT)),
                signingCredentials: new SigningCredentials(key: GetSymmetricSecurityKey(), algorithm: SecurityAlgorithms.HmacSha256));

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new AuthResult(id: user.Id, message: ErrorStrings.OK, statusCode: System.Net.HttpStatusCode.Created, token: token);
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
                return new UpdateResult(message: ErrorStrings.USER_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);

            if (!string.Equals(user.Login, grantRoleData.Login))
                return new UpdateResult(message: ErrorStrings.LOGIN_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);

            AuthUser granter = await _authRepository.GetUser(grantRoleData.GranterId);

            if (granter is null)
                return new UpdateResult(message: ErrorStrings.GRANTER_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);

            if (!string.Equals(granter.Login, grantRoleData.GranterLogin))
                return new UpdateResult(message: ErrorStrings.GRANTERLOGIN_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);

            if (!string.Equals(granter.PasswordHash, grantRoleData.PasswordHash))
                return new UpdateResult(message: ErrorStrings.PASSWORD_HASH_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);


            var updateResult = await _authRepository.GrantRole(
                id: grantRoleData.Id,
                role: grantRoleData.NewRole,
                granterId: grantRoleData.GranterId);

            return updateResult;
        }

        public async Task<UpdateResult> UpdateAccount(UpdateAccountData updateAccountData)
        {
            if (updateAccountData == null)
                throw new ArgumentNullException(ErrorStrings.UPDATEACCOUNTDATA_PARAM_NAME);

            if (string.IsNullOrWhiteSpace(updateAccountData.Login))
                return new UpdateResult
                {
                    Message = ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            if (string.IsNullOrWhiteSpace(updateAccountData.UserName))
                return new UpdateResult
                {
                    Message = ErrorStrings.USERNAME_SHOULD_NOT_BE_EMPTY,
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
            if (deleteAccountData == null)
                throw new ArgumentNullException(ErrorStrings.DELETEACCOUNTDATA_PARAM_NAME);

            if (string.IsNullOrWhiteSpace(deleteAccountData.Login))
                return new DeleteResult(ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(deleteAccountData.PasswordHash))
                return new DeleteResult(ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (deleteAccountData.GranterId == null && !string.IsNullOrWhiteSpace(deleteAccountData.GranterLogin))
                return new DeleteResult(ErrorStrings.GRANTERLOGIN_SHOULD_BE_EMPTY_DELETE, System.Net.HttpStatusCode.BadRequest);

            if (deleteAccountData.GranterId != null && string.IsNullOrWhiteSpace(deleteAccountData.GranterLogin))
                return new DeleteResult(ErrorStrings.GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_DELETE, System.Net.HttpStatusCode.BadRequest);

            var user = await _authRepository.GetUser(deleteAccountData.Id);

            if (user is null)
                return new DeleteResult(message: ErrorStrings.USER_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);

            if (!string.Equals(user.Login, deleteAccountData.Login))
                return new DeleteResult(message: ErrorStrings.LOGIN_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);

            if (deleteAccountData.GranterId != null)
            {
                AuthUser granter = await _authRepository.GetUser(deleteAccountData.GranterId.Value);

                if (granter is null)
                    return new DeleteResult(message: ErrorStrings.GRANTER_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);

                if (!string.Equals(granter.Login, deleteAccountData.GranterLogin))
                    return new DeleteResult(message: ErrorStrings.GRANTERLOGIN_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);

                if (!string.Equals(granter.PasswordHash, deleteAccountData.PasswordHash))
                    return new DeleteResult(message: ErrorStrings.PASSWORD_HASH_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);
            }
            else
            {
                if (!string.Equals(user.PasswordHash, deleteAccountData.PasswordHash))
                    return new DeleteResult(message: ErrorStrings.PASSWORD_HASH_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);
            }

            return await _authRepository.DeleteUser(id: deleteAccountData.Id);
        }
        public async Task<AuthUser> GetUserInfo(uint id)
        {
            var user = await _authRepository.GetUser(id);

            return user;
        }
        public async Task<AuthUser> GetUserInfo(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                return default!; //throw new ArgumentNullException(ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY);

            var user = await _authRepository.GetUser(login);

            return user;
        }

        private SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(_key));
    }
}
