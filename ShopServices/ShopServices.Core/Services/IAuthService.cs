using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Auth;

namespace ShopServices.Core.Services
{
    public interface IAuthService
    {
        Task<AuthResult> Register(AuthUser authUser);
        Task<AuthResult> Login(LoginData loginData);
        Task<Result> DeleteAccount(DeleteAccountData deleteAccountData);
        Task<Result> GrantRole(GrantRoleData grantRoleData);
        Task<Result> UpdateAccount(UpdateAccountData updateAccountData);
        Task<AuthUser> GetUserInfo(uint id);
        Task<AuthUser> GetUserInfo(string login);
    }
}
