namespace NewsFeedSystem.Core.Auth
{
    public class GrantRoleData
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public uint Id { get { return _id; } }
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get { return _login; } }
        public string PasswordHash { get { return _passwordHash; } }
        public uint GranterId { get { return _granterId; } }
        public string GranterLogin { get { return _granterLogin; } }
        public string NewRole { get { return _newRole; } }

        private readonly uint _id;
        private readonly string _login;
        private readonly string _passwordHash;
        private readonly uint _granterId;
        private readonly string _granterLogin;
        private readonly string _newRole;

        public GrantRoleData(
            uint id,
            string login,
            string passwordHash,
            string newRole,
            uint granterId,
            string granterLogin)
        {
            _id = id;
            _login = login;
            _passwordHash = passwordHash;
            _newRole = newRole;
            _granterId = granterId;
            _granterLogin = granterLogin;
        }
    }
}
