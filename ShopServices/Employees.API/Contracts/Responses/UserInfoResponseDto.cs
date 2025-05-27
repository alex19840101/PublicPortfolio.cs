namespace Auth.API.Contracts.Responses
{
    /// <summary>
    /// Класс ответа с информацией о пользователе
    /// </summary>
    public class UserInfoResponseDto
    {
        /// <summary>
        /// Уникальный id аккаунта*
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Логин (уникальное имя аккаунта)*
        /// </summary>
        public string Login { get; set; } = default!;

        /// <summary>
        /// Имя (ФИО) пользователя*
        /// </summary>
        public string UserName { get; set; } = default!;

        /// <summary>
        /// Адрес электронной почты (уникальный)*
        /// </summary>
        public string Email { get; set; } = default!;

        /// <summary>
        /// Ник
        /// </summary>
        public string? Nick { get; set; }

        /// <summary>
        /// Телефон(ы)
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// Роль
        /// </summary>
        public string? Role { get; set; }
    }
}
