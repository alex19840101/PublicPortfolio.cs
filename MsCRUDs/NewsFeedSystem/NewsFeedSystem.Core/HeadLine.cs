using System;
using System.Collections.Generic;

namespace NewsFeedSystem.Core
{
    public class HeadLine
    {
        /// <summary>
        /// Id новостного поста, заголовка
        /// </summary>
        public int Id { get { return _id; } }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Headline { get { return _headLine; } }

        /// <summary>
        /// Id тэгов
        /// </summary>
        public List<int> Tags { get { return _tags; } }

        /// <summary>
        /// Id тем
        /// </summary>
        public List<int> Topics { get { return _topics; } }
        public DateTime Created { get { return _created; } }

        private readonly int _id;
        private string _headLine = default!;
        private List<int> _tags = default!;
        private List<int> _topics = default!;
        private readonly DateTime _created;

        public HeadLine(
            int id,
            string headLine,
            string? url,
            List<int> tags,
            List<int> topics,
            DateTime created)
        {
            _id = id;
            _headLine = headLine;
            _tags = tags;
            _topics = topics;
            _created = created;
        }
    }
}
