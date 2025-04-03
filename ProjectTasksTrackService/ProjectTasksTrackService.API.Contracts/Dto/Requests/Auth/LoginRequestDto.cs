namespace ProjectTasksTrackService.API.Contracts.Dto.Requests.Auth
{
    /// <summary>
    /// Запрос на вход (аутентификацию)
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// Логин (уникальное имя аккаунта)*
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль для входа*
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Таймаут, минут
        /// </summary>
        public int? TimeoutMinutes { get; set; }
    }
}
