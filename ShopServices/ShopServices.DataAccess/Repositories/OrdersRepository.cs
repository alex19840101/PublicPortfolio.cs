using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;

namespace ShopServices.DataAccess.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        public async Task<Result> Create(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
