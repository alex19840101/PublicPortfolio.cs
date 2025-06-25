using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopServices.DataAccess.Entities
{
    [Table("Managers")]
    public class Manager : Employee
    {
        public Manager(uint id,
            string login,
            string name,
            string surname,
            string address,
            string email,
            string passwordHash,
            string? nick,
            string? phone,
            string? role,
            uint? granterId,
            DateTime createdDt,
            DateTime? lastUpdateDt,
            string workStatus,
            uint? shopId,
            uint? warehouseId) : base(
                id: id,
                login: login,
                name: name,
                surname: surname,
                address: address,
                email: email,
                passwordHash: passwordHash,
                nick: nick,
                phone: phone,
                role: role,
                granterId: granterId,
                createdDt: createdDt,
                lastUpdateDt: lastUpdateDt,
                shopId: shopId,
                warehouseId: warehouseId)
        {
            WorkStatus = workStatus;
        }

        /// <summary> Режим работы/отпуск </summary>
        public string WorkStatus { get; set; }

        /// <summary> Группы клиентов (физ. лица и/или корпоративные клиенты / VIP...) </summary>
        public string? ClientGroups { get; set; }

        /// <summary> Категории товаров </summary>
        public string? GoodsCategories { get; set; }

        /// <summary> Услуги, предоставляемые клиентам (продажа / сервисные услуги) </summary>
        public string? Services { get; set; }

        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<Delivery> Deliveries { get; set; } = [];
    }
}
