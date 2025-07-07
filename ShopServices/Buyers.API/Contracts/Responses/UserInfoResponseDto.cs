using System.Collections.Generic;
using ShopServices.Core.Enums;

namespace Buyers.API.Contracts.Responses
{
    /// <summary>
    /// Класс ответа с информацией о покупателе
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
        /// Имя [, отчество] покупателя*
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
        public string? Phone { get; set; }
        /// <summary> Способы уведомлений по <see cref="ShopServices.Core.Enums.NotificationMethod"/></summary>
        public List<NotificationMethod>? NotificationMethods { get; set; }
    }
}
