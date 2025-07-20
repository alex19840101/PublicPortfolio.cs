using System.Threading.Tasks;
using ShopServices.Abstractions;

namespace NotifierBySms.API.Interfaces
{
    /// <summary> Нотификатор, отправляющий SMS-уведомления </summary>
    public interface ISmsNotificationsService
    {
        /// <summary> Отправка SMS-уведомления </summary>
        /// <param name="phoneSender"> *Телефон отправителя </param>
        /// <param name="phoneReceiver"> *Телефон получателя </param>
        /// <param name="message"> *Сообщение </param>
        /// <returns> Результат <see cref="ShopServices.Abstractions.Result"/> </returns>
        public Task<Result> SendSmsNotification(
            string phoneSender,
            string phoneReceiver,
            string message);
    }
}
