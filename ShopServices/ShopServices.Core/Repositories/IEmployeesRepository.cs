using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Abstractions.Auth;
using ShopServices.Core.Auth;

namespace ShopServices.Core.Repositories
{
    public interface IEmployeesRepository
    {
        Task<AuthResult> AddUser(Employee employee);
        /// <summary> Получение данных по работнику без отслеживания изменений </summary>
        /// <param name="id"> id работника </param>
        /// <returns></returns>
        Task<Employee> GetUser(uint id);

        /// <summary> Получение данных по работнику (с отслеживанием изменений для дальнейшего обновления) </summary>
        /// <param name="id"> id работника </param>
        /// <returns></returns>
        Task<Employee> GetUserForUpdate(uint id);
        Task<Employee> GetUser(string login);
        Task<Result> UpdateUser(UpdateAccountData employee);
        Task<Result> GrantRole(uint id, string role, uint granterId);
        Task<Result> DeleteUser(uint id);
    }
}
