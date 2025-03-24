using System;
using System.Collections.Generic;

namespace NewsFeedSystem.Core
{
    public class NewsPost
    {
        /// <summary>
        /// Id новостного поста
        /// </summary>
        public uint Id { get { return _id; } }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Headline { get { return _headLine; } }

        /// <summary>
        /// Текст новости
        /// </summary>
        public string Text { get { return _text; } }

        /// <summary>
        /// Гиперссылка
        /// </summary>
        public string? URL { get { return _url; } }

        /// <summary>
        /// Автор
        /// </summary>
        public string? Author { get { return _author; } }

        /// <summary>
        /// Тэги
        /// </summary>
        public List<int> Tags { get { return _tags; } }

        /// <summary>
        /// Темы
        /// </summary>
        public List<int> Topics { get { return _topics; } }
        public DateTime Created { get { return _created; } }
        public DateTime? Updated { get { return _updated; } }

        private readonly uint _id;
        private string _headLine = default!;
        private string _text = default!;
        private string? _url;
        private string? _author;
        private List<int> _tags = default!;
        private List<int> _topics = default!;
        private readonly DateTime _created;
        private DateTime? _updated;

        public NewsPost(
            uint id,
            string headLine,
            string text,
            string? url,
            string? author,
            List<int> tags,
            List<int> topics,
            DateTime created,
            DateTime? updated)
        {
            _id = id;
            _headLine = headLine;
            _text = text;
            _url = url;
            _author = author;
            _tags = tags;
            _topics = topics;
            _created = created;
            _updated = updated;
        }
    }
}