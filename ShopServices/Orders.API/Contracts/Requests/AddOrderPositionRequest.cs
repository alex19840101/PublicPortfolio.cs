namespace Orders.API.Contracts.Requests
{
    /// <summary> Подзапрос на добавление товарной позиции в заказе </summary>
    public class AddOrderPositionRequest
    {
        #region //Id генерируется автоматически
        ///// <summary> *Уникальный идентификатор товарной позиции системе </summary>
        //public uint Id { get; set; }
        #endregion //Id генерируется автоматически

        /// <summary> *Уникальный идентификатор товара в системе </summary>
        public uint ProductId { get; set; }

        #region //ArticleNumber, Brand, Name, Params - из данных товара
        ///// <summary> Артикул производителя (при наличии) </summary>
        //public string ArticleNumber { get; set; } = default!;

        ///// <summary> Производитель (бренд) </summary>
        //public string Brand { get; set; } = default!;

        ///// <summary> *Название товара </summary>
        //public string Name { get; set; } = default!;

        ///// <summary> Параметры товара </summary>
        //public string Params { get; set; } = default!;
        #endregion //ArticleNumber, Brand, Name, Params - из данных товара

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
