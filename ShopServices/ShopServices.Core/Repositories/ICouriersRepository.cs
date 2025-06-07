using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;
using ShopServices.Core.Models.Requests;

namespace ShopServices.Core.Repositories
{
    public interface ICouriersRepository
    {
        /// <summary> Получение данных по курьеру без отслеживания изменений </summary>
        /// <param name="id"> id работника (курьера) </param>
        /// <returns></returns>
        Task<Courier> GetUser(uint id);

        Task<Courier> GetUser(string login);

        Task<Result> UpdateCourier(UpdateCourierRequest updateCourierRequest);
    }
}
