using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Core.Models;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class ContactsGetterService : IContactsGetterService
    {
        public async Task<ContactData> GetBuyerContactData(uint buyerId)
        {
            throw new NotImplementedException();
        }

        public async Task<ContactData> GetEmployeeContactData(uint employeeId)
        {
            throw new NotImplementedException();
        }
    }
}
