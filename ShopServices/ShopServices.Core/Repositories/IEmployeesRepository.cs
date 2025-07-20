using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Abstractions.Auth;
using ShopServices.Core.Auth;

namespace ShopServices.Core.Repositories
{
    public interface IEmployeesRepository
    {
        /// <summary>
        /// Добавление цифрового аккаунта абстрактного работника
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        Task<AuthResult> AddEmployee(Employee employee);
        /// <summary> Получение данных по работнику без отслеживания изменений </summary>
        /// <param name="employeeId"> id работника </param>
        /// <returns></returns>
        Task<Employee> GetEmployee(uint employeeId);

        /// <summary> Получение данных по работнику (с отслеживанием изменений для дальнейшего обновления) </summary>
        /// <param name="employeeId"> id работника </param>
        /// <returns></returns>
        Task<Employee> GetEmployeeForUpdate(uint employeeId);
        Task<Employee> GetEmployee(string employeeLogin);
        Task<Result> UpdateEmployee(UpdateAccountData employee);
        Task<Result> GrantRole(uint id, string role, uint granterId);
        Task<Result> DeleteEmployee(uint id);
    }
}
