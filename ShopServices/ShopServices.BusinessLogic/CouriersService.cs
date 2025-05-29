using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;
using ShopServices.Core.Models.Requests;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class CouriersService : ICouriersService
    {
        public Task<Courier> GetUserInfo(uint id)
        {
            throw new NotImplementedException();
        }

        public Task<Courier> GetUserInfo(string login)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateCourier(UpdateCourierRequest updateCourierRequest)
        {
            throw new NotImplementedException();
        }
    }
}
