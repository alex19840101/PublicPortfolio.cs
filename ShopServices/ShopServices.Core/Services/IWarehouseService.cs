using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Services
{
    public interface IWarehouseService
    {
        /// <summary> Добавление склада </summary>
        Task<Result> AddWarehouse(Warehouse warehouse);
    }
}
