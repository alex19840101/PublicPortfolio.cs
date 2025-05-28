using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Abstractions.Auth;
using ShopServices.Core.Auth;
using ShopServices.Core.Models;

namespace ShopServices.Core.Services
{
    public interface IBuyersService
    {
        Task<AuthResult> Register(Buyer employee);
        Task<AuthResult> Login(LoginData loginData);
        Task<Result> DeleteAccount(DeleteAccountData deleteAccountData);
        Task<Result> ChangeDiscountGroups(ChangeDiscountGroupsData grantRoleData);
        Task<Result> UpdateAccount(UpdateAccountData updateAccountData);
        Task<Buyer> GetUserInfo(uint id);
        Task<Buyer> GetUserInfo(string login);
    }
}
