namespace Employees.API.Contracts.Requests
{
    /// <summary>
    /// Запрос на регистрацию аккаунта работника
    /// </summary>
    public class RegisterRequestDto
    {
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
        /// Адрес электронной почты (уникальный) работника*
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Пароль для входа работника*
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Пароль для входа работника (повторно)*
        /// </summary>
        public required string RepeatPassword { get; set; }

        /// <summary>
        /// Ник работника
        /// </summary>
        public string? Nick { get; set; }

        /// <summary>
        /// Телефон(ы) работника
        /// </summary>
        public string Phone { get; set; } = default!;
        /// <summary>
        /// Роль работника, например: "manager" / "courier")
        /// </summary>
        public string? RequestedRole { get; set; }
        /// <summary> Id магазина (если есть привязка работника к магазину) </summary>
        public uint? ShopId { get; set; }
        /// <summary> Id склада (если есть привязка работника к складу) </summary>
        public uint? WarehouseId { get; set; }
    }
}
