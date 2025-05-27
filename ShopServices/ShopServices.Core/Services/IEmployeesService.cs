using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Abstractions.Auth;
using ShopServices.Core.Auth;

namespace ShopServices.Core.Services
{
    public interface IEmployeesService
    {
        Task<AuthResult> Register(Employee employee);
        Task<AuthResult> Login(LoginData loginData);
        Task<Result> DeleteAccount(DeleteAccountData deleteAccountData);
        Task<Result> GrantRole(GrantRoleData grantRoleData);
        Task<Result> UpdateAccount(UpdateAccountData updateAccountData);
        Task<Employee> GetUserInfo(uint id);
        Task<Employee> GetUserInfo(string login);
    }
}
