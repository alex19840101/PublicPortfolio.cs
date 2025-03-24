using System.Collections.Generic;

namespace NewsFeedSystem.API.Contracts.Requests
{
    /// <summary>
    /// Запрос на создание (публикацию) новости
    /// </summary>
    public class CreateNewsRequestDto
    {
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
        public List<uint> Tags { get; set; } = [];

        /// <summary>
        /// Темы
        /// </summary>
        public List<uint> Topics { get; set; } = [];
    }
}
