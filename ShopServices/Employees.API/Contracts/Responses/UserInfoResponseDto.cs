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
        /// Имя [, отчество] пользователя*
        /// </summary>
        public required string Name { get; set; } = default!;

        /// <summary>
        /// Фамилия пользователя*
        /// </summary>
        public required string Surname { get; set; } = default!;

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
