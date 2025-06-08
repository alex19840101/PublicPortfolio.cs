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
    public interface IManagersRepository
    {
        /// <summary> Получение данных по менеджеру без отслеживания изменений </summary>
        /// <param name="id"> id работника (менеджера) </param>
        /// <returns></returns>
        Task<Manager> GetManager(uint id);

        Task<Manager> GetManager(string login);

        /// <summary>
        /// Добавление цифрового аккаунта работника-менеджера
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        Task<AuthResult> AddManager(Employee employee);
        Task<Result> UpdateManager(UpdateManagerRequest updateCourierRequest);
    }
}
