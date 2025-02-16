using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProjectTasksTrackService.Core.Auth;
using ProjectTasksTrackService.Core.Repositories;
using ProjectTasksTrackService.Core.Results;
using ProjectTasksTrackService.Core.Services;

namespace ProjectTasksTrackService.BusinessLogic
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            IAuthRepository _authRepository = authRepository;
        }

        public async Task<AuthResult> Register(AuthUser authUser)
        {
            throw new NotImplementedException();
        }
        public async Task<AuthResult> Login(LoginData loginData)
        {
            throw new NotImplementedException();
        }
        public Task<AuthResult> Logout(LogoutData logoutData)
        {
            throw new NotImplementedException();
        }
        public Task<AuthResult> GrantRole(GrantRoleData grantRoleData)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResult> UpdateAccount(UpdateAccountData updateAccountData)
        {
            throw new NotImplementedException();
        }
        public async Task<DeleteResult> DeleteAccount(DeleteAccountData deleteAccountData)
        {
            throw new NotImplementedException();
        }
    }
}
