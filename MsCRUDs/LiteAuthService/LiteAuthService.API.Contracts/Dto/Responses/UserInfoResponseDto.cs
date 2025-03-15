using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteAuthService.API.Contracts.Dto.Responses
{
    /// <summary>
    /// Класс ответа с информацией о пользователе
    /// </summary>
    public class UserInfoResponseDto
    {
        /// <summary>
        /// Логин (уникальное имя аккаунта)*
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Имя (ФИО) пользователя*
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Адрес электронной почты (уникальный)*
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Ник
        /// </summary>
        public string Nick { get; set; }

        /// <summary>
        /// Телефон(ы)
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Роль
        /// </summary>
        public string Role { get; set; }
    }
}
