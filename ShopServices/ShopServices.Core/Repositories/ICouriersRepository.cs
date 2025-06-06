using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Core.Models;

namespace ShopServices.Core.Repositories
{
    public interface ICouriersRepository
    {
        /// <summary> Получение данных по курьеру без отслеживания изменений </summary>
        /// <param name="id"> id работника (курьера) </param>
        /// <returns></returns>
        Task<Courier> GetUser(uint id);

        Task<Courier> GetUser(string login);
    }
}
