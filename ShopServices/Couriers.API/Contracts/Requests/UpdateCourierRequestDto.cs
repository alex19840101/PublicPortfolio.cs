namespace Couriers.API.Contracts.Requests
{
    public class UpdateCourierRequestDto
    {
        /// <summary>
        /// Id курьера*
        /// </summary>
        public uint Id { get; set; }
        public string DriverLicenseCategory { get; set; }
        public string Transport { get; set; }
        public string Areas { get; set; }
        public string DeliveryTimeSchedule { get; set; }
    }
}
