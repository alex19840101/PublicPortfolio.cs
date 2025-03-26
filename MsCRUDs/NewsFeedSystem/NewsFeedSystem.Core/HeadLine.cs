using System;
using System.Collections.Generic;

namespace NewsFeedSystem.Core
{
    public class HeadLine
    {
        /// <summary>
        /// Id новостного поста, заголовка
        /// </summary>
        public uint Id { get { return _id; } }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Headline { get { return _headLine; } }

        /// <summary>
        /// Id тэгов
        /// </summary>
        public List<uint> Tags { get { return _tags; } }

        /// <summary>
        /// Id тем
        /// </summary>
        public List<uint> Topics { get { return _topics; } }
        public DateTime Created { get { return _created; } }

        private readonly uint _id;
        private string _headLine = default!;
        private List<uint> _tags = default!;
        private List<uint> _topics = default!;
        private readonly DateTime _created;

        public HeadLine(
            uint id,
            string headLine,
            List<uint> tags,
            List<uint> topics,
            DateTime created)
        {
            _id = id;
            _headLine = headLine;
            _tags = tags;
            _topics = topics;
            _created = created;
        }

        public override string ToString()
        {
            return $"{_id} {_headLine}";
        }
    }
}
