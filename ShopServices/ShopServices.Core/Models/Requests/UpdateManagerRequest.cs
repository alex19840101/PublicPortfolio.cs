using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServices.Core.Models.Requests
{
    /// <summary> Запрос на обновление данных менеджера </summary>
    public class UpdateManagerRequest
    {
        /// <summary>
        /// Id менеджера*
        /// </summary>
        public uint Id { get; set; }

        /// <summary> Режим работы/отпуск </summary>
        public string WorkStatus { get; set; }

        /// <summary> Группы клиентов (физ. лица и/или корпоративные клиенты / VIP...) </summary>
        public string ClientGroups { get; set; }

        /// <summary> Категории товаров </summary>
        public string GoodsCategories { get; set; }

        /// <summary> Услуги, предоставляемые клиентам (продажа / сервисные услуги) </summary>
        public string Services { get; set; }
    }
}
