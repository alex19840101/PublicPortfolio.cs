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

        /// <summary> Ник </summary>
        public string Nick { get; private set; }

        /// <summary> Дата и время создания данных пользователя </summary>
        public DateTime Created { get; private set; }

        /// <summary> Дата и время обновления данных пользователя (опционально) </summary>
        public DateTime? Updated { get; private set; }

        /// <summary> Список id групп скидок </summary>
        public List<uint> DiscountGroups { get; private set; }
        public uint? GranterId { get; private set; }
        public bool Blocked { get; private set; }

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
            string nick = null,
            List<uint> discountGroups = null,
            uint? granterId = null,
            bool blocked = false)
        {
            Id = id;
            Login = login;
            Name = name;
            Surname = surname;
            Address = address;
            Email = email;
            Phones = phones;
            PasswordHash = passwordHash;
            Nick = nick;
            Created = created;
            Updated = updated;
            DiscountGroups = discountGroups;
            GranterId = granterId;
            Blocked = blocked;
        }

        /// <summary>
        /// Проверка на равенство (существующему Buyer) с игнорированием:
        /// <para> - Id (Id до момента регистрации не определен, как бы равен нулю),</para>
        /// <para> - Created, Updated - не важны для сравнения</para>
        /// </summary>
        /// <param name="comparedBuyer"> Buyer для сравнения </param>
        /// <returns></returns>
        public bool IsEqualIgnoreIdAndDt(Buyer comparedBuyer)
        {
            if (!string.Equals(comparedBuyer.Login, Login) ||
                !string.Equals(comparedBuyer.Name, Name) ||
                !string.Equals(comparedBuyer.Surname, Surname) ||
                !string.Equals(comparedBuyer.Address, Address) ||
                !string.Equals(comparedBuyer.Email, Email) ||
                !string.Equals(comparedBuyer.PasswordHash, PasswordHash) ||
                !string.Equals(comparedBuyer.Nick, Nick) ||
                !string.Equals(comparedBuyer.Phones, Phones))
                return false;

            return true;
        }
    }
}