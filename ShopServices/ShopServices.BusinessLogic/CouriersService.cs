using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ShopServices.Abstractions;
using ShopServices.Core.Models;
using ShopServices.Core.Models.Requests;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class CouriersService : ICouriersService
    {
        private readonly ICouriersRepository _couriersRepository;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly string _key;
        private const int LOGIN_DEFAULT_TIMEOUT = 60;

        public CouriersService(ICouriersRepository authRepository, TokenValidationParameters tokenValidationParameters, string key)
        {
            _couriersRepository = authRepository;
            _tokenValidationParameters = tokenValidationParameters;
            _key = key;
        }

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
