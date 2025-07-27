using System.Threading.Tasks;
using ShopServices.Core.Models;

namespace ShopServices.Core.Services
{
    /// <summary> Интерфейс для получения контактных данных для уведомлений покупателей/работников </summary>
    public interface IContactsGetterService
    {
        public Task<ContactData> GetBuyerContactData(uint buyerId);
        public Task<ContactData> GetEmployeeContactData(uint employeeId);
    }
}
