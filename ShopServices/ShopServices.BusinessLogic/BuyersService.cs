using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ShopServices.Abstractions;
using ShopServices.Abstractions.Auth;
using ShopServices.Core;
using ShopServices.Core.Auth;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class BuyersService : IBuyersService
    {
        private readonly IBuyersRepository _buyersRepository;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly string _key;
        private const int LOGIN_DEFAULT_TIMEOUT = 60;

        public BuyersService(IBuyersRepository authRepository, TokenValidationParameters tokenValidationParameters, string key)
        {
            _buyersRepository = authRepository;
            _tokenValidationParameters = tokenValidationParameters;
            _key = key;
        }

        public async Task<AuthResult> Register(Buyer buyer)
        {
            if (buyer == null)
                throw new ArgumentNullException(ResultMessager.EMPLOYEE_PARAM_NAME);

            if (buyer.Id != 0)
                return new AuthResult(ResultMessager.USER_ID_SHOULD_BE_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(buyer.Login))
                return new AuthResult(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(buyer.Name))
                return new AuthResult(ResultMessager.NAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(buyer.Surname))
                return new AuthResult(ResultMessager.SURNAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(buyer.Address))
                return new AuthResult(ResultMessager.ADDRESS_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(buyer.Email))
                return new AuthResult(ResultMessager.EMAIL_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(buyer.PasswordHash))
                return new AuthResult(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            var existingUser = await _buyersRepository.GetUser(buyer.Login);
            if (existingUser != null)
            {
                if (!existingUser.IsEqualIgnoreIdAndDt(buyer))
                    return new AuthResult(ResultMessager.CONFLICT, System.Net.HttpStatusCode.Conflict);

                return new AuthResult(ResultMessager.ALREADY_EXISTS, System.Net.HttpStatusCode.Created, id: existingUser.Id);
            }

            var registerResult = await _buyersRepository.AddUser(buyer);

            return registerResult;
        }
        public async Task<AuthResult> Login(LoginData loginData)
        {
            if (loginData == null)
                throw new ArgumentNullException(ResultMessager.LOGINDATA_PARAM_NAME);

            if (string.IsNullOrWhiteSpace(loginData.Login))
                return new AuthResult(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(loginData.PasswordHash))
                return new AuthResult(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);


            var user = await _buyersRepository.GetUser(loginData.Login);

            if (user == null)
                return new AuthResult(message: ResultMessager.USER_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);

            if (!string.Equals(loginData.PasswordHash, user.PasswordHash))
                return new AuthResult(message: ResultMessager.PASSWORD_HASH_MISMATCH, statusCode: System.Net.HttpStatusCode.Unauthorized);

            var claims = new List<Claim>
            {
                //new Claim(ClaimTypes.Name, loginData.Login),
                new Claim(ClaimTypes.Role, "buyer")
            };

            var jwt = new JwtSecurityToken(
                issuer: _tokenValidationParameters.ValidIssuer,
                audience: _tokenValidationParameters.ValidAudience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(loginData.TimeoutMinutes ?? LOGIN_DEFAULT_TIMEOUT)),
                signingCredentials: new SigningCredentials(key: GetSymmetricSecurityKey(), algorithm: SecurityAlgorithms.HmacSha256));

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new AuthResult(id: user.Id, message: ResultMessager.OK, statusCode: System.Net.HttpStatusCode.Created, token: token);
        }

        public async Task<Result> ChangeDiscountGroups(ChangeDiscountGroupsData changeDiscountGroupsData)
        {
            if (changeDiscountGroupsData == null)
                throw new ArgumentNullException(ResultMessager.CHANGEDISCOUNTGROUPSDATA_PARAM_NAME);

            if (string.IsNullOrWhiteSpace(changeDiscountGroupsData.Login))
                return new Result(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(changeDiscountGroupsData.GranterLogin))
                return new Result(ResultMessager.GRANTERLOGIN_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            var user = await _buyersRepository.GetUserForUpdate(changeDiscountGroupsData.BuyerId);

            if (user is null)
                return new Result(message: ResultMessager.USER_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);

            if (!string.Equals(user.Login, changeDiscountGroupsData.Login))
                return new Result(message: ResultMessager.LOGIN_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);

            var updateResult = await _buyersRepository.ChangeDiscountGroups(
                buyerId: changeDiscountGroupsData.BuyerId,
                discountGroups: changeDiscountGroupsData.DiscountGroups,
                granterId: changeDiscountGroupsData.GranterId);

            return updateResult;
        }

        public async Task<Result> UpdateAccount(UpdateAccountData updateAccountData)
        {
            if (updateAccountData == null)
                throw new ArgumentNullException(ResultMessager.UPDATEACCOUNTDATA_PARAM_NAME);

            if (string.IsNullOrWhiteSpace(updateAccountData.Login))
                return new Result
                {
                    Message = ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            if (string.IsNullOrWhiteSpace(updateAccountData.Name))
                return new Result
                {
                    Message = ResultMessager.NAME_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            if (string.IsNullOrWhiteSpace(updateAccountData.Surname))
                return new Result
                {
                    Message = ResultMessager.SURNAME_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            if (string.IsNullOrWhiteSpace(updateAccountData.Address))
                return new Result
                {
                    Message = ResultMessager.ADDRESS_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            if (string.IsNullOrWhiteSpace(updateAccountData.Email))
                return new Result
                {
                    Message = ResultMessager.EMAIL_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            if (string.IsNullOrWhiteSpace(updateAccountData.PasswordHash))
                return new Result
                {
                    Message = ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            return await _buyersRepository.UpdateUser(updateAccountData);
        }
        public async Task<Result> DeleteAccount(DeleteAccountData deleteAccountData)
        {
            if (deleteAccountData == null)
                throw new ArgumentNullException(ResultMessager.DELETEACCOUNTDATA_PARAM_NAME);

            if (string.IsNullOrWhiteSpace(deleteAccountData.Login))
                return new Result(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(deleteAccountData.PasswordHash))
                return new Result(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (deleteAccountData.GranterId == null && !string.IsNullOrWhiteSpace(deleteAccountData.GranterLogin))
                return new Result(ResultMessager.GRANTERLOGIN_SHOULD_BE_EMPTY_DELETE, System.Net.HttpStatusCode.BadRequest);

            if (deleteAccountData.GranterId != null && string.IsNullOrWhiteSpace(deleteAccountData.GranterLogin))
                return new Result(ResultMessager.GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_DELETE, System.Net.HttpStatusCode.BadRequest);

            var user = await _buyersRepository.GetUserForUpdate(deleteAccountData.Id);

            if (user is null)
                return new Result(message: ResultMessager.USER_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);

            if (!string.Equals(user.Login, deleteAccountData.Login))
                return new Result(message: ResultMessager.LOGIN_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);

            if (deleteAccountData.GranterId == null) //если же указан GranterId, то данные (Id, хэш пароля) админа/менеджера не проверяем, т.к. не сможем получить их из репозитория покупателей
            {
                if (!string.Equals(user.PasswordHash, deleteAccountData.PasswordHash))
                    return new Result(message: ResultMessager.PASSWORD_HASH_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);
            }

            return await _buyersRepository.DeleteUser(id: deleteAccountData.Id);
        }
        public async Task<Buyer> GetUserInfo(uint id)
        {
            var user = await _buyersRepository.GetUser(id);

            return user;
        }
        public async Task<Buyer> GetUserInfo(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                return default!; //throw new ArgumentNullException(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY);

            var user = await _buyersRepository.GetUser(login);

            return user;
        }

        private SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(_key));
    }
}
