using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;

namespace ShopServices.DataAccess.Repositories
{
    public class CouriersRepository : ICouriersRepository
    {
        private readonly ShopServicesDbContext _dbContext;

        public CouriersRepository(ShopServicesDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //TODO: CouriersRepository

        public Task<Courier> GetUser(uint id)
        {
            throw new NotImplementedException();
        }

        public Task<Courier> GetUser(string login)
        {
            throw new NotImplementedException();
        }

        private async Task<Entities.Courier?> GetCourierEntity(uint id, bool asNoTracking)
        {
            var query = asNoTracking ?
                _dbContext.Couriers.AsNoTracking().Where(c => c.Id == id) :
                _dbContext.Couriers.Where(c => c.Id == id);

            var courierEntity = await query.SingleOrDefaultAsync();

            return courierEntity;
        }

        private async Task<Entities.Courier?> GetCourierEntity(string login)
        {
            var query = _dbContext.Couriers.AsNoTracking().Where(c => c.Login.Equals(login));
            var courierEntity = await query.SingleOrDefaultAsync();

            return courierEntity;
        }
    }
}
