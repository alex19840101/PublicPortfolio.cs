using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;
using Dapper;
using ShopServices.DataAccess.DapperInterfaces;

namespace ShopServices.DataAccess.Repositories
{
    public class BuyersContactsRepository : IBuyersContactsRepository
    {
        private readonly IDapperAsyncExecutor _dapperSqlExecutor;

        public BuyersContactsRepository(IDapperAsyncExecutor dapperSqlExecutor)
        {
            _dapperSqlExecutor = dapperSqlExecutor;
        }


        public async Task<ContactData> GetContactData(uint buyerId)
        {
            var sql = @"SELECT b.Email, b.Phone, b.NotificationMethods, b.TelegramChatId FROM Buyers b WHERE b.Id = @id";
            var dp = new DynamicParameters(new { buyerId });

            var contactData = (await _dapperSqlExecutor.QueryAsync<ContactData>(sql, dp)).SingleOrDefault();

            return contactData!;
        }
    }
}
