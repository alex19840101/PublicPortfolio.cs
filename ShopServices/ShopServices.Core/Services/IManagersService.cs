using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Core.Models;

namespace ShopServices.Core.Services
{
    public interface IManagersService
    {
        Task<Manager> GetUserInfo(uint id);
        Task<Manager> GetUserInfo(string login);
    }
}
