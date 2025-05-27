using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Abstractions.Auth;
using ShopServices.Core.Auth;

namespace ShopServices.Core.Repositories
{
    public interface IEmployeesRepository
    {
        Task<AuthResult> AddUser(Employee employee);
        Task<Employee> GetUser(uint id);
        Task<Employee> GetUser(string login);
        Task<Result> UpdateUser(UpdateAccountData employee);
        Task<Result> GrantRole(uint id, string role, uint granterId);
        Task<Result> DeleteUser(uint id);
    }
}
