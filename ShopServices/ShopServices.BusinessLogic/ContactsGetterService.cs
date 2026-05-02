using System.Threading.Tasks;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class ContactsGetterService : IContactsGetterService
    {
        private readonly IBuyersContactsRepository _buyersContactsRepository;
        private readonly IEmployeesContactsRepository _employeesContactsRepository;

        public ContactsGetterService(
            IBuyersContactsRepository buyersContactsRepository,
            IEmployeesContactsRepository employeesContactsRepository)
        {
            _buyersContactsRepository = buyersContactsRepository;
            _employeesContactsRepository = employeesContactsRepository;
        }

        public async Task<ContactData> GetBuyerContactData(uint buyerId)
        {
            return await _buyersContactsRepository.GetContactData(buyerId);
        }

        public async Task<ContactData> GetEmployeeContactData(uint employeeId)
        {
            return await _employeesContactsRepository.GetContactData(employeeId);
        }
    }
}
