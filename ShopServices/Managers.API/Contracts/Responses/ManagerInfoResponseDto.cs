namespace Managers.API.Contracts.Responses
{
    /// <summary>
    /// Класс ответа с информацией о пользователе (работнике ((менеджере)))
    /// </summary>
    public class ManagerInfoResponseDto
    {
        /// <summary>
        /// Уникальный id аккаунта*
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Информация о работнике (Employee) ((менеджере)))
        /// </summary>
        public EmployeeAccountDto EmployeeAccount { get; set; } = default!;

        /// <summary> Режим работы/отпуск </summary>
        public string WorkStatus { get; set; } = default!;

        /// <summary> Группы клиентов (физ. лица и/или корпоративные клиенты / VIP...) </summary>
        public string? ClientGroups { get; set; }

        /// <summary> Категории товаров </summary>
        public string? GoodsCategories { get; set; }

        /// <summary> Услуги, предоставляемые клиентам (продажа / сервисные услуги) </summary>
        public string? Services { get; set; }
    }
}
