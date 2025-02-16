namespace ProjectTasksTrackService.API.Contracts.Dto.Requests.Auth
{
    /// <summary>
    /// Запрос на обновление аккаунта
    /// </summary>
    public class UpdateAccountRequestDto
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
        /// Имя (ФИО) пользователя*
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Адрес электронной почты (уникальный)*
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль для входа*
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// Пароль для входа (повторно)*
        /// </summary>
        public string RepeatPassword { get; set; }

        /// <summary>
        /// Ник
        /// </summary>
        public string Nick { get; set; }

        /// <summary>
        /// Телефон(ы)
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public string RequestedRole { get; set; }
    }
}
