namespace LiteAuthService.Core.Auth
{
    public class UpdateAccountData
    {
        public int Id { get { return _id; } }
        public string Login { get { return _login; } }
        public string UserName { get { return _userName; } }
        public string Email { get { return _email; } }
        public string PasswordHash { get { return _passwordHash; } }
        public string NewPasswordHash { get { return _newPasswordHash; } }
        public string Nick { get { return _nick; } }
        public string Phone { get { return _phone; } }
        public string RequestedRole { get { return _requestedRole; } }

        private readonly int _id;
        private readonly string _login;
        private readonly string _userName;
        private readonly string _email;
        private readonly string _passwordHash;
        private readonly string _newPasswordHash;
        private readonly string _nick;
        private readonly string _phone;
        private readonly string _requestedRole;

        public UpdateAccountData(
            int id,
            string login,
            string userName,
            string email,
            string passwordHash,
            string newPasswordHash,
            string nick,
            string phone,
            string requestedRole)
        {
            _id = id;
            _login = login;
            _userName = userName;
            _email = email;
            _passwordHash = passwordHash;
            _newPasswordHash = newPasswordHash;
            _nick = nick;
            _phone = phone;
            _requestedRole = requestedRole;
        }
    }
}
