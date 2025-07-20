namespace Managers.API.Contracts.Requests
{
    /// <summary> Запрос на обновление данных менеджера </summary>
    public class UpdateManagerRequestDto
    {
        /// <summary>
        /// Id курьера*
        /// </summary>
        public uint Id { get; set; }

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
