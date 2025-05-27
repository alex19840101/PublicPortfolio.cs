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

namespace ShopServices.DataAccess.Repositories
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly ShopServicesDbContext _dbContext;

        public EmployeesRepository(ShopServicesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AuthResult> AddUser(AuthUser authUser)
        {
            ArgumentNullException.ThrowIfNull(authUser);

            var newEmployeeEntity = new Entities.Employee(
                id: authUser.Id,
                login: authUser.Login,
                name: authUser.Name,
                surname: authUser.Surname,
                email: authUser.Email,
                passwordHash: authUser.PasswordHash,
                nick: authUser.Nick,
                phone: authUser.Phone,
                role: authUser.Role,
                granterId: authUser.GranterId,
                createdDt: authUser.CreatedDt.ToUniversalTime(),
                lastUpdateDt: authUser.LastUpdateDt?.ToUniversalTime());

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

        public async Task<Result> DeleteUser(uint id)
        {
            var employeeEntity = await GetAuthUserEntity(id);

            if (employeeEntity is null)
                return new Result(ResultMessager.USER_NOT_FOUND, HttpStatusCode.NotFound);

            _dbContext.Employees.Remove(employeeEntity);
            await _dbContext.SaveChangesAsync();

            return new Result(ResultMessager.OK, HttpStatusCode.OK);
        }

        public async Task<AuthUser> GetUser(uint id)
        {
            var employeeEntity = await GetAuthUserEntity(id);
            if (employeeEntity is null)
                return null;

            return AuthUser(employeeEntity);
        }

        public async Task<AuthUser> GetUser(string login)
        {
            var employeeEntity = await GetAuthUserEntity(login);
            if (employeeEntity is null)
                return null;

            return AuthUser(employeeEntity);
        }

        public async Task<Result> GrantRole(uint id, string role, uint granterId)
        {
            var granterUserEntity = await GetAuthUserEntity(granterId);
            if (granterUserEntity is null)
                return new Result(ResultMessager.GRANTER_NOT_FOUND, HttpStatusCode.Unauthorized);

            var query = _dbContext.Employees.Where(u => u.Id == id);
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

        public async Task<Result> UpdateUser(UpdateAccountData upd)
        {
            ArgumentNullException.ThrowIfNull(upd);

            var employeeEntity = await _dbContext.Employees
                .SingleOrDefaultAsync(u => u.Id == upd.Id);

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
            if (!string.Equals(upd.RequestedRole, employeeEntity.Role)) employeeEntity.UpdateRole(newRole: $"?{upd.RequestedRole}"); //? - запрошенная пользователем роль утверждается администратором

            if (_dbContext.ChangeTracker.HasChanges())
            {
                employeeEntity.UpdateLastUpdateDt(DateTime.Now.ToUniversalTime());
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.USER_UPDATED, HttpStatusCode.OK);
            }

            return new Result(ResultMessager.USER_IS_ACTUAL, HttpStatusCode.OK);
        }

        private async Task<Entities.Employee> GetAuthUserEntity(uint id)
        {
            var query = _dbContext.Employees.AsNoTracking().Where(u => u.Id == id);
            var employeeEntity = await query.SingleOrDefaultAsync();

            return employeeEntity;
        }

        private async Task<Entities.Employee> GetAuthUserEntity(string login)
        {
            var query = _dbContext.Employees.AsNoTracking().Where(u => u.Login.Equals(login));
            var employeeEntity = await query.SingleOrDefaultAsync();

            return employeeEntity;
        }

        private static AuthUser AuthUser(Entities.Employee userEntity) =>
            new AuthUser(
                id: userEntity.Id,
                login: userEntity.Login,
                name: userEntity.Name,
                surname: userEntity.Surname,
                email: userEntity.Email,
                passwordHash: userEntity.PasswordHash,
                nick: userEntity.Nick,
                phone: userEntity.Phone,
                role: userEntity.Role,
                granterId: userEntity.GranterId,
                createdDt: userEntity.CreatedDt.ToLocalTime(),
                lastUpdateDt: userEntity.LastUpdateDt?.ToLocalTime());

    }
}
