using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServices.Core.Models
{
    public class Price
    {
        public uint Id { get; private set; }
        public uint ProductId { get; private set; }

        /// <summary> Цена за единицу измерения </summary>
        public decimal PricePerUnit { get; private set; }

        /// <summary> Валюта </summary>
        public string Currency { get; private set; }

        /// <summary> Единица измерения товара </summary>
        public string Unit { get; private set; }
    }
}
