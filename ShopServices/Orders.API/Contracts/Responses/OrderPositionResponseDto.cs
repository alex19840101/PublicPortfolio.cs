namespace Orders.API.Contracts.Responses
{
    /// <summary> Товарная позиция в заказе </summary>
    public class OrderPositionResponseDto
    {
        /// <summary> *Уникальный идентификатор товарной позиции системе </summary>
        public uint Id { get; set; }

        /// <summary> *Уникальный идентификатор товара в системе </summary>
        public uint ProductId { get; set; }

        /// <summary> Артикул производителя (при наличии) </summary>
        public string ArticleNumber { get; set; } = default!;

        /// <summary> Производитель (бренд) </summary>
        public string Brand { get; set; } = default!;

        /// <summary> *Название товара </summary>
        public string Name { get; set; } = default!;

        /// <summary> Параметры товара </summary>
        public string Params { get; set; } = default!;

        /// <summary> Цена за единицу измерения </summary>
        public decimal Price { get; set; }

        /// <summary> Количество, единиц измерения </summary>
        public float Quantity { get; set; }

        /// <summary> Стоимость общая по товарной позиции </summary>
        public decimal Cost { get; set; }

        /// <summary> Валюта </summary>
        public string Currency { get; set; } = default!;
    }
}
