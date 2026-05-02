using System.Threading.Tasks;
using ShopServices.Core.Models;

namespace ShopServices.Core.Repositories
{
    /// <summary> Интерфейс для получения контактных данных для уведомлений работников </summary>
    public interface IEmployeesContactsRepository
    {
        public Task<ContactData> GetContactData(uint employeeId);
    }
}
