using System.Collections.Generic;

namespace NewsFeedSystem.API.Contracts.Requests
{
    /// <summary>
    /// Запрос на обновление новости
    /// </summary>
    public class UpdateNewsRequestDto
    {
        /// <summary>
        /// Id новости
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
        /// Тэги
        /// </summary>
        public required List<uint> Tags { get; set; } = [];

        /// <summary>
        /// Темы
        /// </summary>
        public required List<uint> Topics { get; set; } = [];
    }
}
