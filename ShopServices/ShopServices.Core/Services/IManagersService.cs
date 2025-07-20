using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;
using ShopServices.Core.Models.Requests;

namespace ShopServices.Core.Services
{
    public interface IManagersService
    {
        Task<Manager> GetManager(uint id);
        Task<Manager> GetManager(string login);
        Task<Result> UpdateManager(UpdateManagerRequest updateManagerRequest);
    }
}
