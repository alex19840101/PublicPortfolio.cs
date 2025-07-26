using System.Threading.Tasks;
using ShopServices.Core.Models;

namespace ShopServices.Core.Services
{
    public interface IContactsGetterService
    {
        public Task<ContactData> GetBuyerContactData(uint buyerId);
        public Task<ContactData> GetEmployeeContactData(uint employeeId);
    }
}
