using System;
using System.Collections.Generic;

namespace NewsFeedSystem.API.Contracts.Responses
{
    /// <summary>
    /// Класс новостного поста
    /// </summary>
    public class NewsPostDto
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
        /// Текст новости
        /// </summary>
        public required string Text { get; set; }

        /// <summary>
        /// Гиперссылка
        /// </summary>
        public string? URL { get; set; }

        /// <summary>
        /// Автор
        /// </summary>
        public string? Author { get; set; }

        /// <summary>
        /// Id тэгов
        /// </summary>
        public List<int> Tags { get; set; } = default!;

        /// <summary>
        /// Id тем
        /// </summary>
        public List<int> Topics { get; set; } = default!;

        /// <summary>
        /// Дата и время публикации новостного поста
        /// </summary>
        public DateTime Created { get; set; } = default!;

        /// <summary>
        /// Дата и время обновления новостного поста
        /// </summary>
        public DateTime? Updated { get; set; } = default!;
    }
}
