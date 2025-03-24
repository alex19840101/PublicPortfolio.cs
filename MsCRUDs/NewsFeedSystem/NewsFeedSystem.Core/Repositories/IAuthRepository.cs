using System.Threading.Tasks;
using NewsFeedSystem.Core.Auth;
using NewsFeedSystem.Core.Results;

namespace NewsFeedSystem.Core.Repositories
{
    public interface IAuthRepository
    {
        Task<AuthResult> AddUser(AuthUser authUser);
        Task<AuthUser> GetUser(uint id);
        Task<AuthUser> GetUser(string login);
        Task<UpdateResult> UpdateUser(UpdateAccountData authUser);
        Task<UpdateResult> GrantRole(uint id, string role, uint granterId);
        Task<DeleteResult> DeleteUser(uint id);
    }
}
