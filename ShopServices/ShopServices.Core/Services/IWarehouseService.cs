using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Services
{
    public interface IWarehouseService
    {
        /// <summary> Добавление склада </summary>
        public Task<Result> AddWarehouse(Warehouse warehouse);

        /// <summary> Архивация (удаление) склада по id </summary>
        public Task<Result> ArchiveWarehouse(uint warehouseId);

        /// <summary> Получение информации о складе </summary>
        public Task<Warehouse> GetWarehouseById(uint warehouseId);

        /// <summary> Обновление информации о складе </summary>
        public Task<Result> UpdateWarehouse(Warehouse warehouse);
    }
}
