using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Core.Models;

namespace ShopServices.Core.Repositories
{
    public interface IManagersRepository
    {
        /// <summary> Получение данных по менеджеру без отслеживания изменений </summary>
        /// <param name="id"> id работника (менеджера) </param>
        /// <returns></returns>
        Task<Manager> GetUser(uint id);

        Task<Manager> GetUser(string login);
    }
}
