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
    public class ManagersService : IManagersService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IManagersRepository _managersRepository;

        public ManagersService(
            IEmployeesRepository employeesRepository,
            IManagersRepository managersRepository)
        {
            _employeesRepository = employeesRepository;
            _managersRepository = managersRepository;
        }

        public async Task<Manager> GetManager(uint id)
        {
            var user = await _managersRepository.GetManager(id);

            if (user == null)
            {
                var employee = await GetEmployee(id);
                if (employee != null)
                    user = new Manager
                    {
                        Employee = employee
                    };
            }

            return user;
        }

        public async Task<Manager> GetManager(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                return default!; //throw new ArgumentNullException(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY);

            var user = await _managersRepository.GetManager(login);

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

        public async Task<Result> UpdateManager(UpdateManagerRequest updateManagerRequest)
        {
            if (updateManagerRequest == null)
                throw new ArgumentNullException(ResultMessager.UPDATECOURIERREQUEST_PARAM_NAME);

            return await _managersRepository.UpdateManager(updateManagerRequest);
        }
    }
}
