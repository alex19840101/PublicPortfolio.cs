using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectTasksTrackService.Core.Auth;
using ProjectTasksTrackService.Core.Repositories;
using ProjectTasksTrackService.Core.Results;

namespace ProjectTasksTrackService.DataAccess.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        public async Task<CreateResult> AddUser(AuthUser authUser)
        {
            throw new NotImplementedException();
        }

        public async Task<DeleteResult> DeleteUser(int id, string login, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthUser> GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthUser> GetUser(string login)
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateResult> GrantRole(int id, string role, int granterId)
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateResult> UpdateUser(AuthUser authUser)
        {
            throw new NotImplementedException();
        }
    }
}
