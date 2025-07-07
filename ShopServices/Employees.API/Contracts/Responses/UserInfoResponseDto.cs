using System.Collections.Generic;
using ShopServices.Core.Enums;

namespace Employees.API.Contracts.Responses
{
    /// <summary>
    /// Класс ответа с информацией о пользователе
    /// </summary>
    public class UserInfoResponseDto
    {
        /// <summary>
        /// Уникальный id аккаунта*
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Логин (уникальное имя аккаунта)*
        /// </summary>
        public string Login { get; set; } = default!;

        /// <summary>
        /// Имя [, отчество] пользователя*
        /// </summary>
        public required string Name { get; set; } = default!;

        /// <summary>
        /// Фамилия пользователя*
        /// </summary>
        public required string Surname { get; set; } = default!;

        /// <summary>
        /// Адрес электронной почты (уникальный)*
        /// </summary>
        public string Email { get; set; } = default!;

        /// <summary>
        /// Ник
        /// </summary>
        public string? Nick { get; set; }

        /// <summary>
        /// Телефон(ы)
        /// </summary>
        public string Phone { get; set; } = default!;

        /// <summary> Способы уведомлений по <see cref="ShopServices.Core.Enums.NotificationMethod"/></summary>
        public List<NotificationMethod>? NotificationMethods { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public string? Role { get; set; }

        /// <summary> Id магазина (если есть привязка работника к магазину) </summary>
        public uint? ShopId { get; set; }
        /// <summary> Id склада (если есть привязка работника к складу) </summary>
        public uint? WarehouseId { get; set; }
    }
}
