using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Abstractions.Auth;
using ShopServices.Core.Auth;
using ShopServices.Core.Models;
using ShopServices.Core.Models.Requests;

namespace ShopServices.Core.Repositories
{
    public interface ICouriersRepository
    {
        /// <summary> Получение данных по курьеру без отслеживания изменений </summary>
        /// <param name="id"> id работника (курьера) </param>
        /// <returns></returns>
        Task<Courier> GetCourier(uint id);

        Task<Courier> GetCourier(string login);

        /// <summary>
        /// Добавление цифрового аккаунта работника-курьера
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        Task<AuthResult> AddCourier(Employee employee);
        Task<Result> UpdateCourier(UpdateCourierRequest updateCourierRequest);
    }
}
