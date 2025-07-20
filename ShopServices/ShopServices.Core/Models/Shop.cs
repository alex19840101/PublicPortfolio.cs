using System;

namespace ShopServices.Core.Models
{
    public class Shop
    {
        public uint Id { get; private set; }
        public string Name { get; private set; }
        /// <summary> Код города/населенного пункта </summary>
        public uint RegionCode { get; private set; } = default!;
        public string Address { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }
        public string Url { get; private set; }

        /// <summary> Дата и время создания записи в БД </summary>
        public DateTime? CreatedDt { get; private set; }

        /// <summary> Дата и время обновления записи в БД (опционально) </summary>
        public DateTime? Updated { get; private set; }

        /// <summary> В архиве (удален) ли магазин </summary>
        public bool Archived { get; private set; }

        /// <summary> Режим работы магазина </summary>
        public string WorkSchedule { get; private set; }

        public Shop(
            uint id,
            string name,
            uint regionCode,
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
            RegionCode = regionCode;
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
        /// Проверка на равенство (существующего Shop) с игнорированием:
        /// <para> - Id (Id до момента регистрации не определен, как бы равен нулю),</para>
        /// <para> - Created, Updated, Archived - не важны для сравнения</para>
        /// </summary>
        /// <param name="comparedShop"> Shop для сравнения</param>
        /// <returns></returns>
        public bool IsEqualIgnoreIdAndDt(Shop comparedShop)
        {
            if (!string.Equals(comparedShop.Name, Name) ||
                comparedShop.RegionCode != RegionCode ||
                !string.Equals(comparedShop.Address, Address) ||
                !string.Equals(comparedShop.Phone, Phone) ||
                !string.Equals(comparedShop.Email, Email) ||
                !string.Equals(comparedShop.Url, Url) ||
                !string.Equals(comparedShop.WorkSchedule, WorkSchedule))
                return false;

            return true;
        }
    }
}
