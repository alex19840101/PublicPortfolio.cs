using System;
using System.Collections.Generic;
using System.Text;
using ShopServices.Core.Enums;

namespace Buyers.API.Contracts
{
    /// <summary> Класс данных покупателя </summary>
    public class Buyer
    {
        /// <summary> Уникальный идентификатор данных покупателя в системе </summary>
        public uint? Id { get; set; }

        /// <summary> Логин </summary>
        public string Login { get; set; }
        
        /// <summary> Имя </summary>
        public string Name { get; set; }

        /// <summary> Фамилия </summary>
        public string Surname { get; set; }
        /// <summary> Ник (опционально) </summary>
        public string Nick { get; set; }

        /// <summary> Адрес доставки по умолчанию (опционально) </summary>
        public string Address { get; set; }

        /// <summary> E-mail </summary>
        public string Email { get; set; }
        
        /// <summary> Телефон(ы) </summary>
        public string Phones { get; set; }
        /// <summary> Способы уведомлений по <see cref="ShopServices.Core.Enums.NotificationMethod"/></summary>
        public List<NotificationMethod>? NotificationMethods { get; set; }

        /// <summary> Пароль для регистрации/входа* </summary>
        public string Password { get; set; }

        /// <summary> Пароль для регистрации/входа (повторно)* </summary>
        public string RepeatPassword { get; set; }

        /// <summary> Дата и время создания данных пользователя </summary>
        public DateTime Created { get; set; }

        /// <summary> Дата и время обновления данных пользователя (опционально) </summary>
        public DateTime? Updated { get; set; }

        /// <summary> Список id групп скидок </summary>
        public List<uint> DiscountGroups { get; set; }
    }
}