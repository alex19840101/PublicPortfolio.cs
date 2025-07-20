using System;
using System.ComponentModel.DataAnnotations.Schema;

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

        /// <summary> Актуально от какого времени </summary>
        public DateTime ActualFromDt { get; private set; }

        /// <summary> Актуально до какого времени </summary>
        public DateTime? ActualToDt { get; private set; }

        /// <summary> Дата и время обновления ActualToDt (опционально) </summary>
        public DateTime? Updated { get; private set; }

        public Price(
            uint id,
            uint productId,
            decimal pricePerUnit,
            string currency,
            string unit,
            DateTime actualFromDt,
            DateTime? actualToDt)
        {
            Id = id;
            ProductId = productId;
            PricePerUnit = pricePerUnit;
            Currency = currency;
            Unit = unit;
            ActualFromDt = actualFromDt;
            ActualToDt = actualToDt;
        }

        public void UpdateActualToDt(DateTime? newActualToDt) => ActualToDt = newActualToDt;
        public void UpdateUpdatedDt(DateTime? updatedDt) => Updated = updatedDt;
    }
}
