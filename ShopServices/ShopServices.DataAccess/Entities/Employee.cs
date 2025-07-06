using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopServices.DataAccess.Entities
{
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "integer")]
        public uint Id { get { return _id; } }
        public string Login { get { return _login; } }
        public string Name { get { return _name; } }
        public string Surname { get { return _surname; } }
		public string Address { get { return _address; } }
		public string Email { get { return _email; } }
        public string PasswordHash { get { return _passwordHash; } }
        public string? Nick { get { return _nick; } }
        public string? Phone { get { return _phone; } }
        public long? TelegramChatId { get { return _telegramChatId; } }
        /// <summary> Способы уведомлений по <see cref="ShopServices.Core.Enums.NotificationMethod"/></summary>
        [Column(TypeName = "smallint[]")]
        public List<byte> NotificationMethods { get { return _notificationMethods; } }
        public string? Role { get { return _role; } }
        public uint? GranterId { get { return _granterId; } }
        public DateTime CreatedDt { get { return _createdDt; } }
        public DateTime? LastUpdateDt { get { return _lastUpdateDt; } }

        [Column(TypeName = "integer")]
        public uint? ShopId { get { return _shopId; } set { _shopId = value; } }

        [Column(TypeName = "integer")]
        public uint? WarehouseId { get { return _warehouseId; } set { _warehouseId = value; } }

        public Shop? Shop { get; set; }
        public Warehouse? Warehouse { get; set; }

        private readonly uint _id;
        private string _login;
        private string _name;
        private string _surname;
		private string _address;
		private string _email;
        private string _passwordHash;
        private string? _nick;
        private string? _phone;
        /// <summary> Способы уведомлений по <see cref="ShopServices.Core.Enums.NotificationMethod"/></summary>
        private List<byte> _notificationMethods;
        private long? _telegramChatId;
        private string? _role;
        private uint? _granterId;
        private readonly DateTime _createdDt;
        private DateTime? _lastUpdateDt;
        private uint? _shopId;
        private uint? _warehouseId;

        public Employee(
            uint id,
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
            uint? shopId,
            uint? warehouseId)
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
            _role = role;
            _granterId = granterId;
            _createdDt = createdDt;
            _lastUpdateDt = lastUpdateDt;
            _shopId = shopId;
            _warehouseId = warehouseId;
        }

        internal void UpdateLogin(string newLogin) => _login = newLogin;
        internal void UpdateName(string newName) => _name = newName;
        internal void UpdateSurname(string newSurname) => _surname = newSurname;
		internal void UpdateAddress(string newAddress) => _address = newAddress;
		internal void UpdateEmail(string newEmail) => _email = newEmail;
        internal void UpdatePasswordHash(string newPasswordHash) => _passwordHash = newPasswordHash;
        internal void UpdateNick(string? newNick) => _nick = newNick;
        internal void UpdatePhone(string? newPhone) => _phone = newPhone;
        internal void UpdateTelegramChatId(long? telegramChatId) => _telegramChatId = telegramChatId;
        /// <summary> Обновить настройки способов уведомлений по <see cref="ShopServices.Core.Enums.NotificationMethod"/></summary>
        internal void UpdateNotificationMethods(List<byte> notificationMethods) => _notificationMethods = notificationMethods;
        internal void UpdateRole(string? newRole) => _role = newRole;
        internal void UpdateGranterId(uint granterId) => _granterId = granterId;
        internal void UpdateLastUpdateDt(DateTime? lastUpdateDt) => _lastUpdateDt = lastUpdateDt;
        internal void UpdateShopId(uint? shopId) => _shopId = shopId;
        internal void UpdateWarehouseId(uint? warehouseId) => _warehouseId = warehouseId;

        public override bool Equals(object obj)
        {
            var comparedEmployee = (Employee)obj;
            if (comparedEmployee.Id != _id ||
                !string.Equals(comparedEmployee.Login, _login) ||
                !string.Equals(comparedEmployee.Name, _name) ||
                !string.Equals(comparedEmployee.Surname, _surname) ||
				!string.Equals(comparedEmployee.Address, _address) ||
				!string.Equals(comparedEmployee.Email, _email) ||
                !string.Equals(comparedEmployee.PasswordHash, _passwordHash) ||
                !string.Equals(comparedEmployee.Nick, _nick) ||
                !string.Equals(comparedEmployee.Phone, _phone) ||
                !string.Equals(comparedEmployee.Role, _role) ||
                comparedEmployee.CreatedDt != _createdDt ||
                comparedEmployee.LastUpdateDt != _lastUpdateDt)
                return false;

            return true;
        }

        public override string ToString()
        {
            return $"{_role} {_login} {_name} {_surname}";
        }
    }
}
