namespace LiteAuthService.Core.Auth
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

        /// <summary>
        /// Таймаут, минут
        /// </summary>
        public int? TimeoutMinutes { get { return _timeoutMinutes; } }


        private readonly string _login;
        private readonly string _passwordHash;

        private readonly int? _timeoutMinutes;

        public LoginData(string login, string passwordHash, int? timeoutMinutes)
        {
            _login = login;
            _passwordHash = passwordHash;
            _timeoutMinutes = timeoutMinutes;
        }
    }
}
