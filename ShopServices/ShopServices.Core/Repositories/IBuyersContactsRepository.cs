using System.Threading.Tasks;
using ShopServices.Core.Models;

namespace ShopServices.Core.Repositories
{
    /// <summary> Интерфейс для получения контактных данных для уведомлений покупателей </summary>
    public interface IBuyersContactsRepository
    {
        public Task<ContactData> GetContactData(uint buyerId);
    }
}
