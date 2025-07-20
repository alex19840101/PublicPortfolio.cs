using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;
using ShopServices.Core.Models.Requests;

namespace ShopServices.Core.Services
{
    public interface ICouriersService
    {
        Task<Courier> GetCourier(uint id);
        Task<Courier> GetCourier(string login);
        Task<Result> UpdateCourier(UpdateCourierRequest updateCourierRequest);
    }
}
