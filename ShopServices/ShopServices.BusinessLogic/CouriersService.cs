using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Auth;
using ShopServices.Core.Models;
using ShopServices.Core.Models.Requests;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class CouriersService : ICouriersService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly ICouriersRepository _couriersRepository;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly string _key;
        private const int LOGIN_DEFAULT_TIMEOUT = 60;

        public CouriersService(
            IEmployeesRepository employeesRepository,
            ICouriersRepository couriersRepository, TokenValidationParameters tokenValidationParameters, string key)
        {
            _employeesRepository = employeesRepository;
            _couriersRepository = couriersRepository;
            _tokenValidationParameters = tokenValidationParameters;
            _key = key;
        }

        public async Task<Courier> GetUserInfo(uint id)
        {
            var user = await _couriersRepository.GetUser(id);

            if (user == null)
            {
                var employee = await GetEmployee(id);
                if (employee != null)
                    user = new Courier
                    {
                        Employee = employee
                    };
            }

            return user;
        }

        public async Task<Courier> GetUserInfo(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                return default!; //throw new ArgumentNullException(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY);

            var user = await _couriersRepository.GetUser(login);

            return user;
        }

        public async Task<Employee> GetEmployee(uint id)
        {
            var user = await _employeesRepository.GetUser(id);

            return user;
        }
        public async Task<Employee> GetEmployee(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                return default!; //throw new ArgumentNullException(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY);

            var user = await _employeesRepository.GetUser(login);

            return user;
        }

        public async Task<Result> UpdateCourier(UpdateCourierRequest updateCourierRequest)
        {
            if (updateCourierRequest == null)
                throw new ArgumentNullException(ResultMessager.UPDATECOURIERREQUEST_PARAM_NAME);

            if (string.IsNullOrWhiteSpace(updateCourierRequest.Transport))
                return new Result
                {
                    Message = ResultMessager.TRANSPORT_FIELD_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            if (string.IsNullOrWhiteSpace(updateCourierRequest.Areas))
                return new Result
                {
                    Message = ResultMessager.AREAS_FIELD_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            if (string.IsNullOrWhiteSpace(updateCourierRequest.DeliveryTimeSchedule))
                return new Result
                {
                    Message = ResultMessager.DELIVERYTIMESCHEDULE_FIELD_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            return await _couriersRepository.UpdateCourier(updateCourierRequest);
        }
    }
}
