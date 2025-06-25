using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopServices.DataAccess.Entities
{
    public class Shop
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "integer")]
        public uint Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string Address { get; private set; } = default!;
        public string Phone { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string Url { get; private set; } = default!;

        /// <summary> Дата и время создания записи в БД </summary>
        public DateTime? CreatedDt { get; private set; }

        /// <summary> Дата и время обновления записи в БД (опционально) </summary>
        public DateTime? Updated { get; private set; }

        public Shop(
            uint id,
            string name,
            string address,
            string phone,
            string email,
            string url,
            DateTime? createdDt = null,
            DateTime? updated = null)
        {
            Id = id;
            Name = name;
            Address = address;
            Phone = phone;
            Email = email;
            Url = url;
            CreatedDt = createdDt;
            Updated = updated;
        }
    }
}
