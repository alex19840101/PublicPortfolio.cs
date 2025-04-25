using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServices.Core.Models
{
    /// <summary> Класс данных покупателя </summary>
    public class Buyer
    {
        /// <summary> *Уникальный идентификатор данных покупателя в системе </summary>
        public uint Id { get; private set; }

        /// <summary> Логин </summary>
        public string Login { get; private set; }
        
        /// <summary> Имя </summary>
        public string Name { get; private set; }

        /// <summary> Фамилия </summary>
        public string Surname { get; private set; }

        /// <summary> Адрес доставки по умолчанию (опционально) </summary>
        public string Address { get; private set; }

        /// <summary> E-mail </summary>
        public string Email { get; private set; }
        
        /// <summary> Телефон(ы) </summary>
        public string Phones { get; private set; }
        
        /// <summary> Хэш пароля </summary>
        public string PasswordHash { get; private set; }
        
        /// <summary> Дата и время создания данных пользователя </summary>
        public DateTime Created { get; private set; }

        /// <summary> Дата и время обновления данных пользователя (опционально) </summary>
        public DateTime? Updated { get; private set; }

        /// <summary> Список id групп скидок </summary>
        public List<uint> DiscountGroups { get; private set; }

        public Buyer(uint id,
            string login,
            string name,
            string surname,
            string address,
            string email,
            string phones,
            string passwordHash,
            DateTime created,
            DateTime? updated,
            List<uint> discountGroups)
        {
            Id = id;
            Login = login;
            Name = name;
            Surname = surname;
            Address = address;
            Email = email;
            Phones = phones;
            PasswordHash = passwordHash;
            Created = created;
            Updated = updated;
            DiscountGroups = discountGroups;
        }
    }
}