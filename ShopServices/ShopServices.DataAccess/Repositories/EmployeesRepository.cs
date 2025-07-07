using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopServices.Core;
using ShopServices.Core.Auth;
using ShopServices.Core.Repositories;
using ShopServices.Abstractions.Auth;
using ShopServices.Abstractions;
using ShopServices.Core.Enums;

namespace ShopServices.DataAccess.Repositories
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly ShopServicesDbContext _dbContext;

        public EmployeesRepository(ShopServicesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AuthResult> AddEmployee(Employee employee)
        {
            ArgumentNullException.ThrowIfNull(employee);

            var newEmployeeEntity = new Entities.Employee(
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
                notificationMethods: employee.NotificationMethods?.Select(n => (byte)n).ToList(),
                role: employee.Role,
                granterId: employee.GranterId,
                createdDt: employee.CreatedDt.ToUniversalTime(),
                lastUpdateDt: employee.LastUpdateDt?.ToUniversalTime(),
                shopId: employee.ShopId,
                warehouseId: employee.WarehouseId);

            await _dbContext.Employees.AddAsync(newEmployeeEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newEmployeeEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id
            return new AuthResult
            {
                Id = newEmployeeEntity.Id,
                StatusCode = HttpStatusCode.Created,
                Message = ResultMessager.OK
            };
        }

        public async Task<Result> DeleteEmployee(uint id)
        {
            var employeeEntity = await GetEmployeeEntity(id, asNoTracking: false);

            if (employeeEntity is null)
                return new Result(ResultMessager.USER_NOT_FOUND, HttpStatusCode.NotFound);

            employeeEntity.UpdateRole($"DELETED {employeeEntity.Role}");
            await _dbContext.SaveChangesAsync();

            return new Result(ResultMessager.OK, HttpStatusCode.OK);
        }

        public async Task<Employee?> GetEmployee(uint id)
        {
            var employeeEntity = await GetEmployeeEntity(id, asNoTracking: true);
            if (employeeEntity is null)
                return null;

            return Employee(employeeEntity);
        }

        public async Task<Employee?> GetEmployeeForUpdate(uint id)
        {
            var employeeEntity = await GetEmployeeEntity(id, asNoTracking: false);
            if (employeeEntity is null)
                return null;

            return Employee(employeeEntity);
        }

        public async Task<Employee?> GetEmployee(string login)
        {
            var employeeEntity = await GetEmployeeEntity(login);
            if (employeeEntity is null)
                return null;

            return Employee(employeeEntity);
        }

        public async Task<Result> GrantRole(uint id, string role, uint granterId)
        {
            var granterUserEntity = await GetEmployeeEntity(granterId, asNoTracking: false);
            if (granterUserEntity is null)
                return new Result(ResultMessager.GRANTER_NOT_FOUND, HttpStatusCode.Unauthorized);

            var query = _dbContext.Employees.Where(e => e.Id == id);
            var employeeEntity = await query.SingleOrDefaultAsync();
            if (employeeEntity is null)
                return new Result(ResultMessager.USER_NOT_FOUND, HttpStatusCode.NotFound);

            employeeEntity.UpdateRole(role);
            if (!Equals(employeeEntity.GranterId, granterId))
                employeeEntity.UpdateGranterId(granterId);

            employeeEntity.UpdateLastUpdateDt(DateTime.Now.ToUniversalTime());

            await _dbContext.SaveChangesAsync();

            return new Result(ResultMessager.USER_UPDATED, HttpStatusCode.OK);
        }

        public async Task<Result> UpdateEmployee(UpdateAccountData upd)
        {
            ArgumentNullException.ThrowIfNull(upd);

            var employeeEntity = await _dbContext.Employees
                .SingleOrDefaultAsync(e => e.Id == upd.Id);

            if (employeeEntity is null)
                return new Result(ResultMessager.USER_NOT_FOUND, HttpStatusCode.NotFound);

            if (!string.Equals(upd.PasswordHash, employeeEntity.PasswordHash))
                return new Result(ResultMessager.PASSWORD_HASH_MISMATCH, HttpStatusCode.Forbidden);

            if (!string.Equals(upd.Login, employeeEntity.Login)) employeeEntity.UpdateLogin(upd.Login);
            if (!string.Equals(upd.Name, employeeEntity.Name)) employeeEntity.UpdateName(upd.Name);
            if (!string.Equals(upd.Surname, employeeEntity.Surname)) employeeEntity.UpdateSurname(upd.Surname);
            if (!string.Equals(upd.Email, employeeEntity.Email)) employeeEntity.UpdateEmail(upd.Email);
            if (!string.Equals(upd.NewPasswordHash, employeeEntity.PasswordHash)) employeeEntity.UpdatePasswordHash(upd.PasswordHash);
            if (!string.Equals(upd.Nick, employeeEntity.Nick)) employeeEntity.UpdateNick(upd.Nick);
            if (!string.Equals(upd.Phone, employeeEntity.Phone)) employeeEntity.UpdatePhone(upd.Phone);
            if (!string.Equals(upd.Address, employeeEntity.Address)) employeeEntity.UpdateAddress(upd.Address);
            if (!string.Equals(upd.RequestedRole, employeeEntity.Role)) employeeEntity.UpdateRole(newRole: $"?{upd.RequestedRole}"); //? - запрошенная пользователем роль утверждается администратором
            
            if (upd.ShopId != employeeEntity.ShopId) employeeEntity.UpdateShopId(upd.ShopId);
            if (upd.WarehouseId != employeeEntity.WarehouseId) employeeEntity.UpdateWarehouseId(upd.WarehouseId);

            if (_dbContext.ChangeTracker.HasChanges())
            {
                employeeEntity.UpdateLastUpdateDt(DateTime.Now.ToUniversalTime());
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.USER_UPDATED, HttpStatusCode.OK);
            }

            return new Result(ResultMessager.USER_IS_ACTUAL, HttpStatusCode.OK);
        }

        private async Task<Entities.Employee?> GetEmployeeEntity(uint id, bool asNoTracking)
        {
            var query = asNoTracking ?
                _dbContext.Employees.AsNoTracking().Where(e => e.Id == id) :
                _dbContext.Employees.Where(e => e.Id == id);

            var employeeEntity = await query.SingleOrDefaultAsync();

            return employeeEntity;
        }

        private async Task<Entities.Employee?> GetEmployeeEntity(string login)
        {
            var query = _dbContext.Employees.AsNoTracking().Where(e => e.Login.Equals(login));
            var employeeEntity = await query.SingleOrDefaultAsync();

            return employeeEntity;
        }

        private static Employee Employee(Entities.Employee userEntity) =>
            new Employee(
                id: userEntity.Id,
                login: userEntity.Login,
                name: userEntity.Name,
                surname: userEntity.Surname,
                address: userEntity.Address,
                email: userEntity.Email,
                passwordHash: userEntity.PasswordHash,
                nick: userEntity.Nick,
                phone: userEntity.Phone,
                telegramChatId: userEntity.TelegramChatId,
                notificationMethods: userEntity.NotificationMethods?.Select(nm => (NotificationMethod)nm).ToList(),
                role: userEntity.Role,
                granterId: userEntity.GranterId,
                createdDt: userEntity.CreatedDt.ToLocalTime(),
                lastUpdateDt: userEntity.LastUpdateDt?.ToLocalTime(),
                shopId: userEntity.ShopId,
                warehouseId: userEntity.WarehouseId);

    }
}
