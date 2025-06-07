using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class ManagersService : IManagersService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IManagersRepository _managersRepository;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly string _key;
        private const int LOGIN_DEFAULT_TIMEOUT = 60;

        public ManagersService(
            IEmployeesRepository employeesRepository,
            IManagersRepository managersRepository, TokenValidationParameters tokenValidationParameters, string key)
        {
            _managersRepository = managersRepository;
            _tokenValidationParameters = tokenValidationParameters;
            _key = key;
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
