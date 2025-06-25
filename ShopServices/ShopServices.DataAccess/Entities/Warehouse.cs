using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopServices.DataAccess.Entities
{
    public class Warehouse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "integer")]
        public uint Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string Address { get; private set; } = default!;
        public string Phone { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string Url { get; private set; } = default!;

        /// <summary> В архиве (удален) ли склад </summary>
        public bool Archived { get; private set; }

        /// <summary> Режим работы склада </summary>
        public string WorkSchedule { get; private set; }

        /// <summary> Дата и время создания записи в БД </summary>
        public DateTime? CreatedDt { get; private set; }

        /// <summary> Дата и время обновления записи в БД (опционально) </summary>
        public DateTime? Updated { get; private set; }

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
           string workSchedule = "")
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

        internal void Archive() => Archived = true;
        internal void UpdateAddress(string address) => Address = address;
        internal void UpdateEmail(string email) => Email = email;
        internal void UpdateName(string name) => Name = name;
        internal void UpdatePhone(string phone) => Phone = phone;
        internal void UpdateUpdatedDt(DateTime updatedDt) => Updated = updatedDt;
        internal void UpdateUrl(string url) => Url = url;
        internal void UpdateWorkSchedule(string workSchedule) => WorkSchedule = workSchedule;
    }
}
