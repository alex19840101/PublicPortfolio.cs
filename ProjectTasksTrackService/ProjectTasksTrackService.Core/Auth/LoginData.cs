namespace ProjectTasksTrackService.Core.Auth
{
    public class LoginData
    {
        /// <summary>
        /// Логин (уникальное имя аккаунта)*
        /// </summary>
        public string Login { get { return _login; } }

        /// <summary>
        /// Хэш пароля для входа*
        /// </summary>
        public string PasswordHash { get { return _passwordHash; } }

        private readonly string _login;
        private readonly string _passwordHash;

        public LoginData(string login, string passwordHash)
        {
            _login = login;
            _passwordHash = passwordHash;
        }
    }
}
