namespace Employees.API.Contracts.Requests
{
    /// <summary>
    /// Запрос на предоставление роли аккаунту работника
    /// </summary>
    public class GrantRoleRequestDto
    {
        /// <summary>
        /// Id пользователя*
        /// </summary>
        public required uint Id { get; set; }

        /// <summary>
        /// Логин (уникальное имя аккаунта) пользователя*
        /// </summary>
        public required string Login { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public string? NewRole { get; set; }

        /// <summary>
        /// Id администратора*
        /// </summary>
        public required uint GranterId { get; set; }

        /// <summary>
        /// Логин (уникальное имя аккаунта) администратора*
        /// </summary>
        public required string GranterLogin { get; set; }

        /// <summary>
        /// Пароль администратора
        /// </summary>
        public required string Password { get; set; }
    }
}
