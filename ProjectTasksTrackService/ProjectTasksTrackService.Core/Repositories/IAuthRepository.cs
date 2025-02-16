﻿using System.Threading.Tasks;
using ProjectTasksTrackService.Core.Auth;
using ProjectTasksTrackService.Core.Results;

namespace ProjectTasksTrackService.Core.Repositories
{
    public interface IAuthRepository
    {
        Task<CreateResult> AddUser(AuthUser authUser);
        Task<AuthUser> GetUser(int id);
        Task<AuthUser> GetUser(string login);
        Task<UpdateResult> UpdateUser(AuthUser authUser);
        Task<UpdateResult> GrantRole(int id, string role, int granterId);
        Task<DeleteResult> DeleteUser(int id, string login, string passwordHash);
    }
}
