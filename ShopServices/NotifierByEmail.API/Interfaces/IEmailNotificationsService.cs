using System.Threading.Tasks;
using ShopServices.Abstractions;

namespace NotifierByEmail.API.Interfaces
{
    /// <summary> Нотификатор, отправляющий Email-уведомления </summary>
    public interface IEmailNotificationsService
    {
        /// <summary> Отправка Email </summary>
        /// <param name="emailSender"> *E-mail-адрес отправителя </param>
        /// <param name="emailReceiver"> *E-mail-адрес получателя </param>
        /// <param name="topic"> *Тема сообщения (письма) </param>
        /// <param name="emailBody"> *Тело (Body) письма </param>
        /// <returns> Результат <see cref="ShopServices.Abstractions.Result"/></returns>
        public Task<Result> SendEmail(
            string emailSender,
            string emailReceiver,
            string topic,
            string emailBody);
    }
}
