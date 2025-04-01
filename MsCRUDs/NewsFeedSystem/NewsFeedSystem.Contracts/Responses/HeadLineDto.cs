using System;
using System.Collections.Generic;

namespace NewsFeedSystem.API.Contracts.Responses
{
    /// <summary>
    /// Класс заголовка новостного поста
    /// </summary>
    public class HeadLineDto
    {
        /// <summary>
        /// Id новостного поста, заголовка
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public required string Headline { get; set; }

        /// <summary>
        /// Id тэгов
        /// </summary>
        public List<uint> Tags { get; set; } = default!;

        /// <summary>
        /// Id тем
        /// </summary>
        public List<uint> Topics { get; set; } = default!;

        /// <summary>
        /// Дата и время публикации новостного поста
        /// </summary>
        public DateTime Created { get; set; } = default!;
    }
}
