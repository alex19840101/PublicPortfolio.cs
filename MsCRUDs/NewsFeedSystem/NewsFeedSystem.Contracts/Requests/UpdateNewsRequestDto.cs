using System.Collections.Generic;

namespace NewsFeedSystem.Contracts.Requests
{
    /// <summary>
    /// Запрос на обновление новости
    /// </su
    public class UpdateNewsRequestDto
    {
        /// <summary>
        /// Id новости
        /// </summary>
        public int Id { get; set; }

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
        public List<string> Tags { get; set; } = [];

        /// <summary>
        /// Темы
        /// </summary>
        public List<string> Topics { get; set; } = [];
    }
}
