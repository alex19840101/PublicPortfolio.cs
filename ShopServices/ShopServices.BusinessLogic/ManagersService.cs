using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Core.Models;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class ManagersService : IManagersService
    {
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
