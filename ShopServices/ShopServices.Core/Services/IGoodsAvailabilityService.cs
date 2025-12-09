using System.Collections.Generic;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Services
{
    /// <summary> Интерфейс для контроля и учета наличия товаров </summary>
    public interface IGoodsAvailabilityService
    {
        public Task<Result> AddAvailability(Availability availability);
        public Task<Availability> GetAvailabilityById(uint id);
        public Task<IEnumerable<Availability>> GetAvailabilitiesByProductId(
            uint productId,
            uint byPage = 10,
            uint page = 1);
        public Task<Result> UpdateAvailability(Availability availability);
    }
}
