﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsFeedSystem.Core;

namespace NewsFeedSystem.DataAccess.Entities
{
    public class News
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
        public List<uint> Tags { get { return _tags; } }

        /// <summary>
        /// Темы
        /// </summary>
        public List<uint> Topics { get { return _topics; } }
        public DateTime Created { get { return _created; } }
        public DateTime? Updated { get { return _updated; } }

        private readonly uint _id;
        private string _headLine = default!;
        private string _text = default!;
        private string? _url;
        private string? _author;
        private List<uint> _tags = default!;
        private List<uint> _topics = default!;
        private DateTime _created;
        private DateTime? _updated;

        public News(
            uint id,
            string headLine,
            string text,
            string? url,
            string? author,
            List<uint> tags,
            List<uint> topics,
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

        public static News NewsEntity(NewsPost newsPost)
            => new(
                id: newsPost.Id,
                headLine: newsPost.Headline,
                text: newsPost.Text,
                url: newsPost.URL,
                author: newsPost.Author,
                tags: newsPost.Tags,
                topics: newsPost.Topics,
                created: newsPost.Created,
                updated: newsPost.Updated);

        public NewsPost GetCoreNewsPost()
            => new(
                id: _id,
                headLine: _headLine,
                text: _text,
                url: _url,
                author: _author,
                tags: _tags,
                topics: _topics,
                created: _created,
                updated: _updated);

        public void UpdateHeadline(string newHeadline) => _headLine = newHeadline;
        public void UpdateText(string newText) => _text = newText;
        public void UpdateUrl(string? newUrl) => _url = newUrl;
        public void UpdateAuthor(string? newAuthor) => _author = newAuthor;
        public void UpdateTags(List<uint> tags) => _tags = tags;
        public void UpdateTopics(List<uint> topics) => _topics = topics;
        public void UpdateCreated(DateTime created) => _created = created;
        public void UpdateLastUpdateDt(DateTime? updated) => _updated = updated;
    }
}
