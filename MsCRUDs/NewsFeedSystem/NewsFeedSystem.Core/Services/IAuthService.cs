using System.Threading.Tasks;
using NewsFeedSystem.Core.Auth;
using NewsFeedSystem.Core.Results;

namespace NewsFeedSystem.Core.Services
{
    public interface IAuthService
    {
        Task<AuthResult> Register(AuthUser authUser);
        Task<AuthResult> Login(LoginData loginData);
        Task<DeleteResult> DeleteAccount(DeleteAccountData deleteAccountData);
        //Task<AuthResult> Logout(LogoutData logoutData);
        Task<UpdateResult> GrantRole(GrantRoleData grantRoleData);
        Task<UpdateResult> UpdateAccount(UpdateAccountData updateAccountData);
        Task<AuthUser> GetUserInfo(uint id);
        Task<AuthUser> GetUserInfo(string login);
    }
}
