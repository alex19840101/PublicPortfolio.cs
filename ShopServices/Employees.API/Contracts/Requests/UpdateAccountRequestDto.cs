namespace Employees.API.Contracts.Requests
{
    /// <summary>
    /// Запрос на обновление аккаунта работника
    /// </summary>
    public class UpdateAccountRequestDto
    {
        /// <summary>
        /// Id работника*
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Логин (уникальное имя аккаунта) работника*
        /// </summary>
        public required string Login { get; set; }

        /// <summary>
        /// Имя [, отчество] работника*
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Фамилия работника*
        /// </summary>
        public required string Surname { get; set; }

        /// <summary>
        /// Адрес работника*
        /// </summary>
        public required string Address { get; set; }

        /// <summary>
        /// Адрес электронной почты (уникальный)*
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Существующий пароль*
        /// </summary>
        public required string ExistingPassword { get; set; }

        /// <summary>
        /// Новый пароль (если нужна смена пароля)
        /// </summary>
        public string? NewPassword { get; set; }

        /// <summary>
        /// Новый пароль (повторно)(если нужна смена пароля)
        /// </summary>
        public string? RepeatNewPassword { get; set; }

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
        public string? RequestedRole { get; set; }

        /// <summary> Id магазина (если есть привязка работника к магазину) </summary>
        public uint? ShopId { get; set; }
        /// <summary> Id склада (если есть привязка работника к складу) </summary>
        public uint? WarehouseId { get; set; }
    }
}
