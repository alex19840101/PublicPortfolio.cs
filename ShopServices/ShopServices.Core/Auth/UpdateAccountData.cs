﻿namespace ShopServices.Core.Auth
{
    public class UpdateAccountData
    {
        public uint Id { get { return _id; } }
        public string Login { get { return _login; } }
        public string Name { get { return _name; } }
        public string Surname { get { return _surname; } }
        public string Address { get { return _address; } }
        public string Email { get { return _email; } }
        public string PasswordHash { get { return _passwordHash; } }
        public string NewPasswordHash { get { return _newPasswordHash; } }
        public string Nick { get { return _nick; } }
        public string Phone { get { return _phone; } }
        public long? TelegramChatId { get { return _telegramChatId; } }
        public string RequestedRole { get { return _requestedRole; } }
        public uint? ShopId { get { return _shopId; } }
        public uint? WarehouseId { get { return _warehouseId; } }

        private readonly uint _id;
        private readonly string _login;
        private readonly string _name;
        private readonly string _surname;
        private readonly string _address;
        private readonly string _email;
        private readonly string _passwordHash;
        private readonly string _newPasswordHash;
        private readonly string _nick;
        private readonly string _phone;
        private readonly long? _telegramChatId;
        private readonly string _requestedRole;
        private readonly uint? _shopId;
        private readonly uint? _warehouseId;

        public UpdateAccountData(
            uint id,
            string login,
            string name,
            string surname,
            string address,
            string email,
            string passwordHash,
            string newPasswordHash,
            string nick,
            string phone,
            long? telegramChatId,
            uint? shopId,
            uint? warehouseId,
            string requestedRole = null)
        {
            _id = id;
            _login = login;
            _name = name;
            _surname = surname;
            _address = address;
            _email = email;
            _passwordHash = passwordHash;
            _newPasswordHash = newPasswordHash;
            _nick = nick;
            _phone = phone;
            _telegramChatId = telegramChatId;
            _requestedRole = requestedRole;
            _shopId = shopId;
            _warehouseId = warehouseId;
        }
    }
}
