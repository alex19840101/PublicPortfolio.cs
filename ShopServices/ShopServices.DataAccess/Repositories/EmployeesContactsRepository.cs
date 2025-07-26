using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;
using ShopServices.DataAccess.DapperInterfaces;

namespace ShopServices.DataAccess.Repositories
{
    public class EmployeesContactsRepository : IEmployeesContactsRepository
    {
        private readonly IDapperAsyncExecutor _dapperSqlExecutor;

        public EmployeesContactsRepository(IDapperAsyncExecutor dapperSqlExecutor)
        {
            _dapperSqlExecutor = dapperSqlExecutor;
        }


        public async Task<ContactData> GetContactData(uint employeeId)
        {
            var sql = @"SELECT e.Email, e.Phone, e.NotificationMethods, e.TelegramChatId FROM Employees e WHERE e.Id = @id";
            var dp = new DynamicParameters(new { employeeId });

            var contactData = (await _dapperSqlExecutor.QueryAsync<ContactData>(sql, dp)).SingleOrDefault();

            return contactData!;
        }
    }
}
