namespace Employees.API.Contracts.Requests
{
    /// <summary>
    /// Запрос на вход (аутентификацию) работника
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// Логин (уникальное имя аккаунта)*
        /// </summary>
        public required string Login { get; set; }

        /// <summary>
        /// Пароль для входа*
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Таймаут, минут
        /// </summary>
        public int? TimeoutMinutes { get; set; }
    }
}
