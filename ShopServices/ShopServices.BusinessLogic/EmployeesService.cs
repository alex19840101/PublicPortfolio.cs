using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ShopServices.Abstractions;
using ShopServices.Abstractions.Auth;
using ShopServices.Core;
using ShopServices.Core.Auth;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly ICouriersRepository _couriersRepository;
        private readonly IManagersRepository _managersRepository;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly string _key;
        private const int LOGIN_DEFAULT_TIMEOUT = 60;

        public EmployeesService(
            IEmployeesRepository employeesRepository,
            ICouriersRepository couriersRepository,
            IManagersRepository managersRepository,
            TokenValidationParameters tokenValidationParameters,
            string key)
        {
            _employeesRepository = employeesRepository;
            _couriersRepository = couriersRepository;
            _managersRepository = managersRepository;
            _tokenValidationParameters = tokenValidationParameters;
            _key = key;
        }

        public async Task<AuthResult> Register(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(ResultMessager.EMPLOYEE_PARAM_NAME);

            if (employee.Id != 0)
                return new AuthResult(ResultMessager.USER_ID_SHOULD_BE_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(employee.Login))
                return new AuthResult(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(employee.Name))
                return new AuthResult(ResultMessager.NAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(employee.Surname))
                return new AuthResult(ResultMessager.SURNAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(employee.Address))
                return new AuthResult(ResultMessager.ADDRESS_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(employee.Email))
                return new AuthResult(ResultMessager.EMAIL_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(employee.PasswordHash))
                return new AuthResult(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);
            var rolePost = employee.Role;
            employee.UpdateRole(newRole: $"?{employee.Role}"); //? - запрошенная пользователем роль утверждается администратором

            var existingUser = await _employeesRepository.GetEmployee(employee.Login);
            if (existingUser != null)
            {
                if (!existingUser.IsEqualIgnoreIdAndDt(employee))
                    return new AuthResult(ResultMessager.CONFLICT, System.Net.HttpStatusCode.Conflict);

                return new AuthResult(ResultMessager.ALREADY_EXISTS, System.Net.HttpStatusCode.Created, id: existingUser.Id);
            }

            AuthResult registerResult;

            if (string.Equals(rolePost?.ToLowerInvariant().Trim(), Roles.Manager))
                return await _managersRepository.AddManager(employee);

            if (string.Equals(rolePost?.ToLowerInvariant().Trim(), Roles.Courier))
                return await _couriersRepository.AddCourier(employee);

            registerResult = await _employeesRepository.AddEmployee(employee);

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


            var user = await _employeesRepository.GetEmployee(loginData.Login);

            if (user == null)
                return new AuthResult(message: ResultMessager.USER_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);

            if (!string.Equals(loginData.PasswordHash, user.PasswordHash))
                return new AuthResult(message: ResultMessager.PASSWORD_HASH_MISMATCH, statusCode: System.Net.HttpStatusCode.Unauthorized);

            var claims = new List<Claim>
            {
                //new Claim(ClaimTypes.Name, loginData.Login),
                new Claim(ClaimTypes.Role, user.Role ?? string.Empty),
                new Claim(ClaimTypes.UserData, user.Id.ToString())
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

        public async Task<Result> GrantRole(GrantRoleData grantRoleData)
        {
            if (grantRoleData == null)
                throw new ArgumentNullException(ResultMessager.GRANTROLEDATA_PARAM_NAME);

            if (string.IsNullOrWhiteSpace(grantRoleData.Login))
                return new Result(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(grantRoleData.PasswordHash))
                return new Result(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(grantRoleData.GranterLogin))
                return new Result(ResultMessager.GRANTERLOGIN_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            var user = await _employeesRepository.GetEmployeeForUpdate(grantRoleData.Id);

            if (user is null)
                return new Result(message: ResultMessager.USER_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);

            if (!string.Equals(user.Login, grantRoleData.Login))
                return new Result(message: ResultMessager.LOGIN_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);

            Employee granter = await _employeesRepository.GetEmployee(grantRoleData.GranterId);

            if (granter is null)
                return new Result(message: ResultMessager.GRANTER_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);

            if (!string.Equals(granter.Login, grantRoleData.GranterLogin))
                return new Result(message: ResultMessager.GRANTERLOGIN_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);

            if (!string.Equals(granter.PasswordHash, grantRoleData.PasswordHash))
                return new Result(message: ResultMessager.PASSWORD_HASH_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);


            var updateResult = await _employeesRepository.GrantRole(
                id: grantRoleData.Id,
                role: grantRoleData.NewRole,
                granterId: grantRoleData.GranterId);

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

            return await _employeesRepository.UpdateEmployee(updateAccountData);
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

            var user = await _employeesRepository.GetEmployeeForUpdate(deleteAccountData.Id);

            if (user is null)
                return new Result(message: ResultMessager.USER_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);

            if (!string.Equals(user.Login, deleteAccountData.Login))
                return new Result(message: ResultMessager.LOGIN_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);

            if (deleteAccountData.GranterId != null)
            {
                Employee granter = await _employeesRepository.GetEmployee(deleteAccountData.GranterId.Value);

                if (granter is null)
                    return new Result(message: ResultMessager.GRANTER_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);

                if (!string.Equals(granter.Login, deleteAccountData.GranterLogin))
                    return new Result(message: ResultMessager.GRANTERLOGIN_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);

                if (!string.Equals(granter.PasswordHash, deleteAccountData.PasswordHash))
                    return new Result(message: ResultMessager.PASSWORD_HASH_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);
            }
            else
            {
                if (!string.Equals(user.PasswordHash, deleteAccountData.PasswordHash))
                    return new Result(message: ResultMessager.PASSWORD_HASH_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);
            }

            return await _employeesRepository.DeleteEmployee(id: deleteAccountData.Id);
        }
        public async Task<Employee> GetUserInfo(uint id)
        {
            var user = await _employeesRepository.GetEmployee(id);

            return user;
        }
        public async Task<Employee> GetUserInfo(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                return default!; //throw new ArgumentNullException(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY);

            var user = await _employeesRepository.GetEmployee(login);

            return user;
        }

        private SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(_key));
    }
}
