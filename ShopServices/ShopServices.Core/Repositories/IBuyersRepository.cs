using System.Collections.Generic;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Abstractions.Auth;
using ShopServices.Core.Auth;
using ShopServices.Core.Models;

namespace ShopServices.Core.Repositories
{
    public interface IBuyersRepository
    {
        Task<AuthResult> AddUser(Buyer buyer);
        Task<Buyer> GetUser(uint id);
        Task<Buyer> GetUser(string login);
        Task<Result> UpdateUser(UpdateAccountData buyer);
        Task<Result> ChangeDiscountGroups(uint buyerId, List<uint> discountGroups, uint granterId);
        Task<Result> DeleteUser(uint id);
    }
}
