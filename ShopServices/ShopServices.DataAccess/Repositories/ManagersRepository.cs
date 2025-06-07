using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopServices.Abstractions.Auth;
using ShopServices.Core;
using ShopServices.Core.Auth;
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

        public async Task<AuthResult> AddUser(Employee employee)
        {
            ArgumentNullException.ThrowIfNull(employee);

            var newManagerEntity = new Entities.Manager(
                id: employee.Id,
                login: employee.Login,
                name: employee.Name,
                surname: employee.Surname,
                address: employee.Address,
                email: employee.Email,
                passwordHash: employee.PasswordHash,
                nick: employee.Nick,
                phone: employee.Phone,
                role: employee.Role,
                granterId: employee.GranterId,
                createdDt: employee.CreatedDt.ToUniversalTime(),
                lastUpdateDt: employee.LastUpdateDt?.ToUniversalTime());

            await _dbContext.Managers.AddAsync(newManagerEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newManagerEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id
            return new AuthResult
            {
                Id = newManagerEntity.Id,
                StatusCode = HttpStatusCode.Created,
                Message = ResultMessager.OK
            };
        }

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
