using ShopServices.Core.Auth;
using ShopServices.Core.Models;

namespace Buyers.API
{
    internal static class BuyerMapper
    {
        internal static Contracts.Buyer GetBuyerDto(Buyer coreBuyer)
        {
            return new Contracts.Buyer
            {
                Id = coreBuyer.Id,
                Login = coreBuyer.Login,
                Name = coreBuyer.Name,
                Surname = coreBuyer.Surname,
                Address = coreBuyer.Address,
                Email = coreBuyer.Email,
                Phones = coreBuyer.Phones,
                Created = coreBuyer.Created,
                Updated = coreBuyer.Updated
            };
        }

        internal static Buyer GetCoreBuyer(Contracts.Buyer buyerDto)
        {
            return new Buyer(
                id: buyerDto.Id ?? 0,
                login: buyerDto.Login,
                name: buyerDto.Name,
                surname: buyerDto.Surname,
                address: buyerDto.Address,
                email: buyerDto.Email,
                phones: buyerDto.Phones,
                passwordHash: SHA256Hasher.GeneratePasswordHash(buyerDto.Password, buyerDto.RepeatPassword),
                created: buyerDto.Created,
                updated: buyerDto.Updated);
        }
    }
}
