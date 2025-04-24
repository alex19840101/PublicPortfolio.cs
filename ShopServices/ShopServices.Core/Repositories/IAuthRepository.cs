using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Auth;

namespace ShopServices.Core.Repositories
{
    public interface IAuthRepository
    {
        Task<AuthResult> AddUser(AuthUser authUser);
        Task<AuthUser> GetUser(uint id);
        Task<AuthUser> GetUser(string login);
        Task<Result> UpdateUser(UpdateAccountData authUser);
        Task<Result> GrantRole(uint id, string role, uint granterId);
        Task<Result> DeleteUser(uint id);
    }
}
