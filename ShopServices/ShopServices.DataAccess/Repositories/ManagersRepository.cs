using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopServices.Abstractions;
using ShopServices.Abstractions.Auth;
using ShopServices.Core;
using ShopServices.Core.Auth;
using ShopServices.Core.Enums;
using ShopServices.Core.Models;
using ShopServices.Core.Models.Requests;
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

        public async Task<AuthResult> AddManager(Employee employee)
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
                telegramChatId: employee.TelegramChatId,
                notificationMethods: employee.NotificationMethods.Select(n => (byte)n).ToList(),
                role: employee.Role,
                granterId: employee.GranterId,
                createdDt: employee.CreatedDt.ToUniversalTime(),
                lastUpdateDt: employee.LastUpdateDt?.ToUniversalTime(),
                workStatus: "?",
                shopId: employee.ShopId,
                warehouseId: employee.WarehouseId
                );

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


        public async Task<Manager?> GetManager(uint id)
        {
            var managerEntity = await GetManagerEntity(id, asNoTracking: true);
            if (managerEntity is null)
                return null;

            return GetCoreManagerModel(managerEntity);
        }

        public async Task<Manager?> GetManager(string login)
        {
            var managerEntity = await GetManagerEntity(login);
            if (managerEntity is null)
                return null;

            return GetCoreManagerModel(managerEntity);
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

        public async Task<Result> UpdateManager(UpdateManagerRequest upd)
        {
            ArgumentNullException.ThrowIfNull(upd);

            var managerEntity = await _dbContext.Managers
                .SingleOrDefaultAsync(c => c.Id == upd.Id);

            if (managerEntity == null)
            {
                var employeeEntity = await _dbContext.Employees
                    .SingleOrDefaultAsync(e => e.Id == upd.Id);

                if (employeeEntity is null)
                    return new Result(ResultMessager.USER_NOT_FOUND, HttpStatusCode.NotFound);

                return new Result(ResultMessager.EMPLOYEE_IS_NOT_MANAGER, System.Net.HttpStatusCode.Conflict);
            }

            if (!string.Equals(upd.WorkStatus, managerEntity.WorkStatus)) managerEntity.WorkStatus = upd.WorkStatus;
            if (!string.Equals(upd.ClientGroups, managerEntity.ClientGroups)) managerEntity.ClientGroups = upd.ClientGroups;
            if (!string.Equals(upd.GoodsCategories, managerEntity.GoodsCategories)) managerEntity.GoodsCategories = upd.GoodsCategories;
            if (!string.Equals(upd.Services, managerEntity.Services)) managerEntity.Services = upd.Services;
            if (_dbContext.ChangeTracker.HasChanges())
            {
                managerEntity.UpdateLastUpdateDt(DateTime.Now.ToUniversalTime());
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.USER_UPDATED, HttpStatusCode.OK);
            }

            return new Result(ResultMessager.USER_IS_ACTUAL, HttpStatusCode.OK);
        }



        /// <summary>
        /// Маппинг Entities.Manager - Core.Models.Manager
        /// </summary>
        /// <param name="managerEntity"> Entities.Manager-данные менеджера из БД </param>
        /// <returns> Core.Models.Manager-объект </returns>
        private static Manager GetCoreManagerModel(Entities.Manager managerEntity) =>
            new Manager
            {
                WorkStatus = managerEntity.WorkStatus,
                ClientGroups = managerEntity.ClientGroups,
                GoodsCategories = managerEntity.GoodsCategories,
                Services = managerEntity.Services,

                Employee = new Core.Auth.Employee(
                    id: managerEntity.Id,
                    login: managerEntity.Login,
                    name: managerEntity.Name,
                    surname: managerEntity.Surname,
                    address: managerEntity.Address,
                    email: managerEntity.Email,
                    passwordHash: managerEntity.PasswordHash,
                    nick: managerEntity.Nick,
                    phone: managerEntity.Phone,
                    telegramChatId: managerEntity.TelegramChatId,
                    notificationMethods: managerEntity.NotificationMethods?.Select(nm => (NotificationMethod)nm).ToList(),
                    role: managerEntity.Role,
                    granterId: managerEntity.GranterId,
                    createdDt: managerEntity.CreatedDt.ToLocalTime(),
                    lastUpdateDt: managerEntity.LastUpdateDt?.ToLocalTime(),
                    shopId: managerEntity.ShopId,
                    warehouseId: managerEntity.WarehouseId)

            };
    }
}
