using System;

namespace ShopServices.Abstractions
{
    /// <summary> Абстрактное событие в ShopServices </summary>
    public abstract class EventBase
    {
        /// <summary> Id события </summary>
        public Guid Id { get; }
        /// <summary> Дата и время создания события </summary>
        public DateTime Created { get; }

        /// <summary> Внутреннее сообщение о событии (например, на английском языке) </summary>
        public string Message { get; }

        /// <summary> Уведомление (например, на русском языке) </summary>
        public string Notification { get; }

        /// <summary> Абстрактное событие в ShopServices </summary>
        /// <param name="id"> Id события в ShopServices </param>
        /// <param name="created"> Дата и время создания события </param>
        /// <param name="message"> Внутреннее сообщение о событии (например, на английском языке) </param>
        /// <param name="notification"> Уведомление (например, на русском языке) </param>
        public EventBase(
            Guid id,
            DateTime created,
            string message,
            string notification)
        {
            Id = id;
            Created = created;
            Message = message;
            Notification = notification;
        }
    }
}
