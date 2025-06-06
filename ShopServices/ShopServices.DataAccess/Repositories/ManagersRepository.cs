using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;

namespace ShopServices.DataAccess.Repositories
{
    public class ManagersRepository : IManagersRepository
    {
        private readonly ShopServicesDbContext _dbContext;

        public ManagersRepository(ShopServicesDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //TODO: ManagersRepository
        public Task<Manager> GetUser(uint id)
        {
            throw new NotImplementedException();
        }

        public Task<Manager> GetUser(string login)
        {
            throw new NotImplementedException();
        }

        private async Task<Entities.Manager?> GetManagerEntity(uint id, bool asNoTracking)
        {
            var query = asNoTracking ?
                _dbContext.Managers.AsNoTracking().Where(m => m.Id == id) :
                _dbContext.Managers.Where(m => m.Id == id);

            var managerEntity = await query.SingleOrDefaultAsync();

            return managerEntity;
        }

        private async Task<Entities.Manager?> GetManagerEntity(string login)
        {
            var query = _dbContext.Managers.AsNoTracking().Where(m => m.Login.Equals(login));
            var managerEntity = await query.SingleOrDefaultAsync();

            return managerEntity;
        }
    }
}
