namespace LiteAuthService.API.Contracts.Dto.Requests.Auth
{
    /// <summary>
    /// Запрос на выход
    /// </summary>
    public class LogoutRequestDto
    {
        /// <summary>
        /// Логин (уникальное имя аккаунта)*
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Id пользователя ((опционально, пригодилось бы для логов))
        /// </summary>
        public int? Id { get; set; }
    }
}
