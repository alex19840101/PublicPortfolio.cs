using ShopServices.Core.Enums;

namespace Notifications.API.Contracts.Requests
{
    /// <summary> Запрос на добавление однократного уведомления </summary>
    public class AddNotificationRequestDto
    {
        /// <summary> *Тип измененной модели (сущности) (для классификации уведомлений) по <see cref="ShopServices.Core.Enums.ModelEntityType"/> </summary>
        public ModelEntityType ModelEntityType { get; set; }

        /// <summary> *Метод уведомления по <see cref="ShopServices.Core.Enums.NotificationMethod"/> </summary>
        public NotificationMethod NotificationMethod { get; set; }

        /// <summary> *Id измененной сущности в БД </summary>
        public ulong ChangedEntityId { get; set; }
        
        /// <summary> Уникальный идентификатор покупателя </summary>
        public uint? BuyerId { get; set; }

        /// <summary> *Отправитель (E-mail/телефон/...) </summary>
        public string From { get; set; } = default!;

        /// <summary> *Получатель (E-mail/телефон/...) </summary>
        public string To { get; set; } = default!;

        /// <summary> *Тема сообщения (в E-mail будет отдельно, в SMS/Telegram-сообщении будет в начале сообщения) </summary>
        public string Topic { get; set; } = default!;

        /// <summary> *Сообщение </summary>
        public string Message { get; set; } = default!;
    }
}
