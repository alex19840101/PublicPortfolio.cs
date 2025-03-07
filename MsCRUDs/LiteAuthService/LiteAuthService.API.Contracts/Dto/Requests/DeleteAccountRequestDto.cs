namespace LiteAuthService.API.Contracts.Dto.Requests.Auth
{
    /// <summary>
    /// Запрос на удаление аккаунта
    /// </summary>
    public class DeleteAccountRequestDto
    {
        /// <summary>
        /// Id пользователя*
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Логин (уникальное имя аккаунта)*
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль*
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Пароль (повторно)*
        /// </summary>
        public string RepeatPassword { get; set; }

        /// <summary>
        /// Id администратора (если удаление администратором)
        /// </summary>
        public int? GranterId { get; set; }

        /// <summary>
        /// Логин (уникальное имя аккаунта) администратора (если удаление администратором)*
        /// </summary>
        public string GranterLogin { get; set; }
    }
}
