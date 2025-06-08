using System;
using System.Threading.Tasks;
using ShopServices.Core.Models;
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

        public Task<Manager> GetUserInfo(uint id)
        {
            throw new NotImplementedException();
        }

        public Task<Manager> GetUserInfo(string login)
        {
            throw new NotImplementedException();
        }
    }
}
