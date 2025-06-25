using System;

namespace ShopServices.Core.Models
{
    public class Warehouse
    {
        public uint Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }
        public string Url { get; private set; }

        /// <summary> Дата и время создания записи в БД </summary>
        public DateTime? CreatedDt { get; private set; }

        /// <summary> Дата и время обновления записи в БД (опционально) </summary>
        public DateTime? Updated { get; private set; }

        /// <summary> В архиве (удален) ли склад </summary>
        public bool Archived { get; private set; }

        /// <summary> Режим работы склада </summary>
        public string WorkSchedule { get; private set; }

        public Warehouse(
           uint id,
           string name,
           string address,
           string phone,
           string email,
           string url,
           DateTime? createdDt = null,
           DateTime? updated = null,
           bool archived = false,
           string workSchedule = null)
        {
            Id = id;
            Name = name;
            Address = address;
            Phone = phone;
            Email = email;
            Url = url;
            CreatedDt = createdDt;
            Updated = updated;
            Archived = archived;
            WorkSchedule = workSchedule;
        }

        /// <summary>
        /// Проверка на равенство (существующего Warehouse) с игнорированием:
        /// <para> - Id (Id до момента регистрации не определен, как бы равен нулю),</para>
        /// <para> - Created, Updated, Archived - не важны для сравнения</para>
        /// </summary>
        /// <param name="comparedWarehouse"> Warehouse для сравнения</param>
        /// <returns></returns>
        public bool IsEqualIgnoreIdAndDt(Warehouse comparedWarehouse)
        {
            if (!string.Equals(comparedWarehouse.Name, Name) ||
                !string.Equals(comparedWarehouse.Address, Address) ||
                !string.Equals(comparedWarehouse.Phone, Phone) ||
                !string.Equals(comparedWarehouse.Email, Email) ||
                !string.Equals(comparedWarehouse.Url, Url) ||
                !string.Equals(comparedWarehouse.WorkSchedule, WorkSchedule))
                return false;

            return true;
        }
    }
}
