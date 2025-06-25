using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Services
{
    public interface IShopsService
    {
        /// <summary> Добавление магазина </summary>
        public Task<Result> AddShop(Shop shop);

        /// <summary> Архивация (удаление) магазина по id </summary>
        public Task<Result> ArchiveShop(uint shopId);

        /// <summary> Получение информации о магазине </summary>
        public Task<Shop> GetShopById(uint shopId);

        /// <summary> Обновление информации о магазине </summary>
        public Task<Result> UpdateShop(Shop shop);
    }
}
