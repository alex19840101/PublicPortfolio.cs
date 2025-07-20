using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServices.Core.Models.Requests
{
    /// <summary> Запрос на обновление данных курьера </summary>
    public class UpdateCourierRequest
    {
        /// <summary>
        /// Id курьера*
        /// </summary>
        public uint Id { get; set; }
        
        /// <summary>
        /// Категория водительских прав (при наличии)
        /// </summary>
        public string DriverLicenseCategory { get; set; } = default!;
        
        /// <summary>
        /// Информация о транспорте*
        /// </summary>
        public string Transport { get; set; } = default!;
        
        /// <summary>
        /// Районы доставки*
        /// </summary>
        public string Areas { get; set; } = default!;
        
        /// <summary>
        /// Расписание доставки*
        /// </summary>
        public string DeliveryTimeSchedule { get; set; } = default!;
    }
}
