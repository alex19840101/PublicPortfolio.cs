namespace Couriers.API.Contracts.Responses
{
    /// <summary> Класс данных курьера для Couriers.API </summary>
    public class CourierInfoResponseDto
    {
        /// <summary>
        /// Id курьера*
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Информация о работнике (Employee) ((курьере)))
        /// </summary>
        public EmployeeAccountDto EmployeeAccount { get; set; } = default!;

        /// <summary>
        /// Категория водительских прав (при наличии)
        /// </summary>
        public string? DriverLicenseCategory { get; set; }

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
