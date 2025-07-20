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
        /// <summary> Получение данных по покупателю без отслеживания изменений </summary>
        /// <param name="id"> id покупателя </param>
        /// <returns></returns>
        Task<Buyer> GetUser(uint id);

        /// <summary> Получение данных по покупателю (с отслеживанием изменений для дальнейшего обновления) </summary>
        /// <param name="id"> id покупателя </param>
        /// <returns></returns>
        Task<Buyer> GetUserForUpdate(uint id);
        Task<Buyer> GetUser(string login);
        Task<Result> UpdateUser(UpdateAccountData buyer);
        Task<Result> ChangeDiscountGroups(uint buyerId, List<uint> discountGroups, uint granterId);
        Task<Result> DeleteUser(uint id);
    }
}
