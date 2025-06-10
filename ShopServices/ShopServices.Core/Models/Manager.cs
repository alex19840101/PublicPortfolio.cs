using System;
using System.Collections.Generic;
using System.Text;
using ShopServices.Core.Auth;

namespace ShopServices.Core.Models
{
    public class Manager
    {
        public Employee Employee { get; set; } = default!;

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
