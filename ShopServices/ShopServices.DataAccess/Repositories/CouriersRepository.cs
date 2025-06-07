using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopServices.Abstractions;
using ShopServices.Abstractions.Auth;
using ShopServices.Core;
using ShopServices.Core.Auth;
using ShopServices.Core.Models;
using ShopServices.Core.Models.Requests;
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

        public async Task<AuthResult> AddUser(Employee employee)
        {
            ArgumentNullException.ThrowIfNull(employee);

            var newCourierEntity = new Entities.Courier(
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
                lastUpdateDt: employee.LastUpdateDt?.ToUniversalTime())
            {
                Transport = "?",
                Areas = "?",
                DeliveryTimeSchedule = "?"
            };

            await _dbContext.Couriers.AddAsync(newCourierEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newCourierEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id
            return new AuthResult
            {
                Id = newCourierEntity.Id,
                StatusCode = HttpStatusCode.Created,
                Message = ResultMessager.OK
            };
        }


        public async Task<Courier?> GetUser(uint id)
        {
            var courierEntity = await GetCourierEntity(id, asNoTracking: true);
            if (courierEntity is null)
                return null;

            return GetCoreCourierModel(courierEntity);
        }

        public async Task<Courier?> GetUserForUpdate(uint id)
        {
            var courierEntity = await GetCourierEntity(id, asNoTracking: false);
            if (courierEntity is null)
                return null;

            return GetCoreCourierModel(courierEntity);
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

        public async Task<Result> UpdateCourier(UpdateCourierRequest upd)
        {
            ArgumentNullException.ThrowIfNull(upd);

            var courierEntity = await _dbContext.Couriers
                .SingleOrDefaultAsync(c => c.Id == upd.Id);

            if (courierEntity == null)
            {
                var employeeEntity = await _dbContext.Employees
                    .SingleOrDefaultAsync(e => e.Id == upd.Id);

                if (employeeEntity is null)
                    return new Result(ResultMessager.USER_NOT_FOUND, HttpStatusCode.NotFound);

                return new Result(ResultMessager.EMPLOYEE_IS_NOT_COURIER, System.Net.HttpStatusCode.Conflict);
            }

            if (!string.Equals(upd.DriverLicenseCategory, courierEntity.DriverLicenseCategory)) courierEntity.DriverLicenseCategory = upd.DriverLicenseCategory;
            if (!string.Equals(upd.Transport, courierEntity.Transport)) courierEntity.Transport = upd.Transport;
            if (!string.Equals(upd.Areas, courierEntity.Areas)) courierEntity.Areas = upd.Areas;
            if (!string.Equals(upd.DeliveryTimeSchedule, courierEntity.DeliveryTimeSchedule)) courierEntity.DeliveryTimeSchedule = upd.DeliveryTimeSchedule;
            if (_dbContext.ChangeTracker.HasChanges())
            {
                courierEntity.UpdateLastUpdateDt(DateTime.Now.ToUniversalTime());
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.USER_UPDATED, HttpStatusCode.OK);
            }

            return new Result(ResultMessager.USER_IS_ACTUAL, HttpStatusCode.OK);
        }

        /// <summary>
        /// Маппинг Entities.Courier - Core.Models.Courier
        /// </summary>
        /// <param name="courierEntity"> Entities.Courier-данные курьера из БД </param>
        /// <returns> Core.Models.Courier-объект </returns>
        private static Courier GetCoreCourierModel(Entities.Courier courierEntity) =>
            new Courier
            {
                DriverLicenseCategory = courierEntity.DriverLicenseCategory,
                Transport = courierEntity.Transport,
                Areas = courierEntity.Areas,
                DeliveryTimeSchedule = courierEntity.DeliveryTimeSchedule,

                Employee = new Core.Auth.Employee(
                    id: courierEntity.Id,
                    login: courierEntity.Login,
                    name: courierEntity.Name,
                    surname: courierEntity.Surname,
                    address: courierEntity.Address,
                    email: courierEntity.Email,
                    passwordHash: courierEntity.PasswordHash,
                    nick: courierEntity.Nick,
                    phone: courierEntity.Phone,
                    role: courierEntity.Role,
                    granterId: courierEntity.GranterId,
                    createdDt: courierEntity.CreatedDt.ToLocalTime(),
                    lastUpdateDt: courierEntity.LastUpdateDt?.ToLocalTime())

            };
    }
}
