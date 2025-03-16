namespace LiteAuthService.Core.Auth
{
    public class DeleteAccountData
    {
        public int Id { get { return _id; } }
        public string Login { get { return _login; } }
        public string PasswordHash { get { return _passwordHash; } }
        public int? GranterId { get { return _granterId; } }
        public string GranterLogin { get { return _granterLogin; } }

        private readonly int _id;
        private readonly string _login;
        private readonly string _passwordHash;
        private readonly int? _granterId;
        private readonly string _granterLogin;

        public DeleteAccountData(
            int id,
            string login,
            string passwordHash,
            int? granterId,
            string granterLogin)
        {
            _id = id;
            _login = login;
            _passwordHash = passwordHash;
            _granterId = granterId;
            _granterLogin = granterLogin;
        }
    }
}
