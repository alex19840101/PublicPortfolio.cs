using System.Threading.Tasks;
using LiteAuthService.Core.Auth;
using LiteAuthService.Core.Results;

namespace LiteAuthService.Core.Services
{
    public interface IAuthService
    {
        Task<AuthResult> Register(AuthUser authUser);
        Task<AuthResult> Login(LoginData loginData);
        Task<DeleteResult> DeleteAccount(DeleteAccountData deleteAccountData);
        //Task<AuthResult> Logout(LogoutData logoutData);
        Task<UpdateResult> GrantRole(GrantRoleData grantRoleData);
        Task<UpdateResult> UpdateAccount(UpdateAccountData updateAccountData);
        Task<AuthUser> GetUserInfo(int id);
        Task<AuthUser> GetUserInfo(string login);
    }
}
