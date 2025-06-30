using System;
using ShopServices.Core.Enums;

namespace Notifications.API.Contracts.Responses
{
    /// <summary> Данные однократного уведомления </summary>
    public class NotificationDataResponseDto
    {
        /// <summary> Id уведомления </summary>
        public ulong Id { get; set; }

        /// <summary> Тип измененной модели (сущности) (для классификации уведомлений) по <see cref="ShopServices.Core.Enums.ModelEntityType"/> </summary>
        public ModelEntityType ModelEntityType { get; set; }

        /// <summary> Id измененной сущности в БД </summary>
        public ulong ChangedEntityId { get; set; }

        /// <summary> Уникальный идентификатор покупателя </summary>
        public uint? BuyerId { get; set; }

        /// <summary> *Метод уведомления по <see cref="ShopServices.Core.Enums.NotificationMethod"/> </summary>
        public NotificationMethod NotificationMethod { get; set; }

        /// <summary> *Отправитель (E-mail/телефон/...) </summary>
        public string From { get; set; } = default!;

        /// <summary> *Получатель (E-mail/телефон/...) </summary>
        public string To { get; set; } = default!;

        /// <summary> *Сообщение </summary>
        public string Message { get; set; } = default!;

        /// <summary> Тема сообщения (в E-mail отдельно, в SMS/Telegram-сообщении - в начале сообщения) </summary>
        public string? Topic { get; set; }

        /// <summary> *Дата и время создания уведомления </summary>
        public DateTime Created { get; set; }

        /// <summary> *Создатель (автор) уведомления </summary>
        public string Creator { get; set; } = default!;

        /// <summary> Дата и время отправки уведомления </summary>
        public DateTime? Sent { get; set; }

        /// <summary> Количество неудачных попыток отправки уведомления </summary>
        public ulong UnsuccessfulAttempts { get; set; }

        /// <summary> Дата и время последней неудачной попытки отправки уведомления </summary>
        public DateTime? LastUnsuccessfulAttempt { get; set; }
    }
}
