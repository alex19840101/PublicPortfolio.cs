using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Repositories
{
    public interface IAvailabilityRepository
    {
        Task<Result> Create(Availability newAvailability);
        Task<Result> DeleteAvailability(ulong availabilityId);
        Task<IEnumerable<Availability>> GetAvailabilitiesByProductId(uint productId);
        Task<Availability> GetAvailabilityById(ulong availabilityId);
        Task<Result> UpdateAvailability(Availability availability);
    }
}
