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
            var id = (int)employeeId;
            var sql = @"SELECT b.""Email"", b.""Phone"", b.""NotificationMethods"", b.""TelegramChatId"" FROM public.""Employees"" b WHERE b.""Id"" = @id";
            var dp = new DynamicParameters(new { employeeId });

            var contactData = (await _dapperSqlExecutor.QueryAsync<ContactData>(sql, dp)).SingleOrDefault();

            return contactData!;
        }
    }
}
