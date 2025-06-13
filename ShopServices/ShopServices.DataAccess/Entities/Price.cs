using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopServices.DataAccess.Entities
{
    public class Price
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "integer")]
        public uint Id { get; private set; }
        public uint ProductId { get; private set; }

        /// <summary> Цена за единицу измерения </summary>
        public decimal PricePerUnit { get; private set; }

        /// <summary> Валюта </summary>
        public string Currency { get; private set; } = default!;

        /// <summary> Единица измерения товара </summary>
        public string Unit { get; private set; } = default!;
    }
}
