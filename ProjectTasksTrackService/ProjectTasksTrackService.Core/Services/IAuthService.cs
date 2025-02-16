using System.Threading.Tasks;
using ProjectTasksTrackService.Core.Auth;
using ProjectTasksTrackService.Core.Results;

namespace ProjectTasksTrackService.Core.Services
{
    public interface IAuthService
    {
        Task<AuthResult> Register(AuthUser authUser);
        Task<AuthResult> Login(LoginData loginData);
        Task<DeleteResult> DeleteAccount(DeleteAccountData deleteAccountData);
        Task<AuthResult> Logout(LogoutData logoutData);
        Task<UpdateResult> GrantRole(GrantRoleData grantRoleData);
        Task<UpdateResult> UpdateAccount(UpdateAccountData updateAccountData);
    }
}
