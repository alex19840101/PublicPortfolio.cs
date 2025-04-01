namespace NewsFeedSystem.Core.Auth
{
    public class DeleteAccountData
    {
        public uint Id { get { return _id; } }
        public string Login { get { return _login; } }
        public string PasswordHash { get { return _passwordHash; } }
        public uint? GranterId { get { return _granterId; } }
        public string GranterLogin { get { return _granterLogin; } }

        private readonly uint _id;
        private readonly string _login;
        private readonly string _passwordHash;
        private readonly uint? _granterId;
        private readonly string _granterLogin;

        public DeleteAccountData(
            uint id,
            string login,
            string passwordHash,
            uint? granterId,
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
