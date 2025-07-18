﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopServices.DataAccess.Entities
{
    public class Buyer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "integer")]
        public uint Id { get { return _id; } }
        public string Login { get { return _login; } }
        public string Name { get { return _name; } }
        public string Surname { get { return _surname; } }
		public string? Address { get { return _address; } }
		public string Email { get { return _email; } }
        public string PasswordHash { get { return _passwordHash; } }
        public string? Nick { get { return _nick; } }
        public string? Phone { get { return _phone; } }
        public long? TelegramChatId { get { return _telegramChatId; } }

        /// <summary> Способы уведомлений по <see cref="ShopServices.Core.Enums.NotificationMethod"/></summary>
        [Column(TypeName = "smallint[]")]
        public List<byte>? NotificationMethods { get { return _notificationMethods; } }
        [Column(TypeName = "integer[]")]
        public List<uint>? DiscountGroups { get { return _discountGroups; } }
        public uint? GranterId{ get { return _granterId; } }
        public DateTime CreatedDt { get { return _createdDt; } }
        public DateTime? LastUpdateDt { get { return _lastUpdateDt; } }
        public bool Blocked { get { return _blocked; } }

        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<Delivery> Deliveries { get; set; } = [];


        private readonly uint _id;
        private string _login;
        private string _name;
        private string _surname;
		private string? _address;
		private string _email;
        private string _passwordHash;
        private string? _nick;
        private string? _phone;
        private long? _telegramChatId;
        /// <summary> Способы уведомлений по <see cref="ShopServices.Core.Enums.NotificationMethod"/></summary>
        private List<byte>? _notificationMethods;
        private List<uint>? _discountGroups;
        private uint? _granterId;
        private readonly DateTime _createdDt;
        private DateTime? _lastUpdateDt;
        private bool _blocked;

        public Buyer(
            uint id,
            string login,
            string name,
            string surname,
			string? address,
			string email,
            string passwordHash,
            string? nick,
            string? phone,
            long? telegramChatId,
            List<byte>? notificationMethods,
            List<uint>? discountGroups,
            uint? granterId,
            DateTime createdDt,
            DateTime? lastUpdateDt,
            bool blocked)
        {
            _id = id;
            _login = login;
            _name = name;
            _surname = surname;
			_address = address;
			_email = email;
            _passwordHash = passwordHash;
            _nick = nick;
            _phone = phone;
            _telegramChatId = telegramChatId;
            _notificationMethods = notificationMethods;
            _discountGroups = discountGroups;
            _granterId = granterId;
            _createdDt = createdDt;
            _lastUpdateDt = lastUpdateDt;
            _blocked = blocked;
        }

        public void UpdateLogin(string newLogin) => _login = newLogin;
        public void UpdateName(string newName) => _name = newName;
        public void UpdateSurname(string newSurname) => _surname = newSurname;
		public void UpdateAddress(string? newAddress) => _address = newAddress;
		public void UpdateEmail(string newEmail) => _email = newEmail;
        public void UpdatePasswordHash(string newPasswordHash) => _passwordHash = newPasswordHash;
        public void UpdateNick(string? newNick) => _nick = newNick;
        public void UpdatePhone(string? newPhone) => _phone = newPhone;
        public void UpdateTelegramChatId(long? telegramChatId) => _telegramChatId = telegramChatId;
        /// <summary> Обновить настройки способов уведомлений по <see cref="ShopServices.Core.Enums.NotificationMethod"/></summary>
        public void UpdateNotificationMethods(List<byte> notificationMethods) => _notificationMethods = notificationMethods;
        public void UpdateDiscountGroups(List<uint>? discountGroups) => _discountGroups = discountGroups;
        public void UpdateGranterId(uint granterId) => _granterId = granterId;
        public void UpdateLastUpdateDt(DateTime? lastUpdateDt) => _lastUpdateDt = lastUpdateDt;
        public void UpdateBlocked(bool blocked) => _blocked = blocked;

        public override bool Equals(object obj)
        {
            var comparedBuyer = (Buyer)obj;
            if (comparedBuyer.Id != _id ||
                !string.Equals(comparedBuyer.Login, _login) ||
                !string.Equals(comparedBuyer.Name, _name) ||
                !string.Equals(comparedBuyer.Surname, _surname) ||
				!string.Equals(comparedBuyer.Address, _address) ||
				!string.Equals(comparedBuyer.Email, _email) ||
                !string.Equals(comparedBuyer.PasswordHash, _passwordHash) ||
                !string.Equals(comparedBuyer.Nick, _nick) ||
                !string.Equals(comparedBuyer.Phone, _phone) ||
                comparedBuyer.CreatedDt != _createdDt ||
                comparedBuyer.LastUpdateDt != _lastUpdateDt)
                return false;

            return true;
        }

        public override string ToString()
        {
            return $"{_login} {_name} {_surname}";
        }
    }
}
