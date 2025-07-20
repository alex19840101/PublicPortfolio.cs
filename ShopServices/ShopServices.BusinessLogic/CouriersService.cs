using System;
using System.Threading.Tasks;
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

        public CouriersService(
            IEmployeesRepository employeesRepository,
            ICouriersRepository couriersRepository)
        {
            _employeesRepository = employeesRepository;
            _couriersRepository = couriersRepository;
        }

        public async Task<Courier> GetCourier(uint id)
        {
            var user = await _couriersRepository.GetCourier(id);

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

        public async Task<Courier> GetCourier(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                return default!; //throw new ArgumentNullException(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY);

            var user = await _couriersRepository.GetCourier(login);

            return user;
        }

        public async Task<Employee> GetEmployee(uint id)
        {
            var user = await _employeesRepository.GetEmployee(id);

            return user;
        }
        public async Task<Employee> GetEmployee(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                return default!; //throw new ArgumentNullException(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY);

            var user = await _employeesRepository.GetEmployee(login);

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
