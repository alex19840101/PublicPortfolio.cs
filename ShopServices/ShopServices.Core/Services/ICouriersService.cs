using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Core.Models;

namespace ShopServices.Core.Services
{
    public interface ICouriersService
    {
        Task<Courier> GetUserInfo(uint id);
        Task<Courier> GetUserInfo(string login);
    }
}
