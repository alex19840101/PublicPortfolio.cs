using System;
using System.Collections.Generic;
using System.Linq;
using NewsFeedSystem.API.Contracts;
using NewsFeedSystem.API.Contracts.Requests;
using NewsFeedSystem.API.Contracts.Responses;
using NewsFeedSystem.Core;

namespace NewsFeedSystem.API.Extensions
{
    /// <summary>
    /// Класс расширений моделей. Методы маппинга моделей
    /// </summary>
    public static class ModelsExtensions
    {
        /// <summary>
        /// Получение HeadLineDto-модели из Core.HeadLine-модели заголовка новостного поста
        /// </summary>
        /// <param name="headLine"></param>
        /// <returns></returns>
        public static HeadLineDto GetHeadLineDto(this HeadLine headLine) => new HeadLineDto
        {
            Id = headLine.Id,
            Headline = headLine.Headline,
            Created = headLine.Created,
            Tags = headLine.Tags,
            Topics = headLine.Topics
        };

        /// <summary>
        /// Получение Core.HeadLine-модели из HeadLineDto-модели заголовка новостного поста
        /// </summary>
        /// <param name="headLine"></param>
        /// <returns></returns>
        public static HeadLine GetHeadLine(this HeadLineDto headLine) => new HeadLine
        (
            id: headLine.Id,
            headLine: headLine.Headline,
            created: headLine.Created,
            tags: headLine.Tags,
            topics: headLine.Topics
        );

        /// <summary>
        /// Маппинг NewsPostDto - Core.NewsPost
        /// </summary>
        /// <param name="newsPostDto"></param>
        /// <returns></returns>

        public static NewsPost GetNewsPost(this NewsPostDto newsPostDto) => new NewsPost(
            id: newsPostDto.Id,
            headLine: newsPostDto.Headline,
            text: newsPostDto.Text,
            url: newsPostDto.URL,
            author: newsPostDto.Author,
            tags: newsPostDto.Tags,
            topics: newsPostDto.Topics,
            created: newsPostDto.Created,
            updated: newsPostDto.Updated);

        /// <summary>
        /// Маппинг Core.NewsPost - NewsPostDto
        /// </summary>
        /// <param name="newsPost"></param>
        /// <returns></returns>

        public static NewsPostDto GetNewsPostDto(this NewsPost newsPost) => new NewsPostDto
        {
            Id = newsPost.Id,
            Headline = newsPost.Headline,
            Text = newsPost.Text,
            URL = newsPost.URL,
            Author = newsPost.Author,
            Tags = newsPost.Tags,
            Topics = newsPost.Topics,
            Created = newsPost.Created,
            Updated = newsPost.Updated
        };


        /// <summary>
        /// Маппинг TagDto - Core.Tag
        /// </summary>
        /// <param name="tagDto"></param>
        /// <returns></returns>
        public static Tag GetTag(this TagDto tagDto) => new Tag
        (
            id: (uint)tagDto.Id!,
            name: tagDto.Tag
        );

        /// <summary>
        /// Маппинг Core.Tag - TagDto
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static TagDto GetTagDto(this Tag tag) => new TagDto
        {
            Id = tag.Id,
            Tag = tag.Name
        };

        /// <summary>
        /// Маппинг TopicDto - Core.Topic
        /// </summary>
        /// <param name="topicDto"></param>
        /// <returns></returns>
        public static Topic GetTopic(this TopicDto topicDto) => new Topic
        (
            id: (uint)topicDto.Id!,
            name: topicDto.Topic
        );


        /// <summary>
        /// Маппинг Core.Topic - TopicDto
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public static TopicDto GetTopicDto(this Topic topic) => new TopicDto
        {
            Id = topic.Id,
            Topic = topic.Name
        };

        /// <summary>
        /// Маппинг CreateNewsRequestDto - Core.NewsPost
        /// </summary>
        /// <param name="createNewsRequestDto"></param>
        /// <returns></returns>

        public static NewsPost GetNewsPostForCreate(this CreateNewsRequestDto createNewsRequestDto) => new NewsPost(
            id: 0,
            headLine: createNewsRequestDto.Headline,
            text: createNewsRequestDto.Text,
            url: createNewsRequestDto.URL,
            author: createNewsRequestDto.Author,
            tags: createNewsRequestDto.Tags,
            topics: createNewsRequestDto.Topics,
            created: DateTime.Now,
            updated: DateTime.Now);

        /// <summary>
        /// Маппинг IEnumerable(HeadLine) - IEnumerable(HeadLineDto)
        /// </summary>
        /// <param name="headlinesLst"></param>
        /// <returns></returns>

        public static IEnumerable<HeadLineDto> GetHeadlineDtos(this IEnumerable<HeadLine> headlinesLst)
        {
            return headlinesLst.Select(h => new HeadLineDto
            {
                        Id = h.Id,
                        Headline = h.Headline,
                        Topics = h.Topics,
                        Tags = h.Tags,
                        Created = h.Created
            });
        }

        /// <summary>
        /// Маппинг UpdateNewsRequestDto - NewsPost
        /// </summary>
        /// <param name="updateNewsRequestDto"></param>
        /// <returns></returns>
        public static NewsPost GetNewsPostForUpdate(this UpdateNewsRequestDto updateNewsRequestDto) => new NewsPost(
            id: updateNewsRequestDto.Id,
            headLine: updateNewsRequestDto.Headline,
            text: updateNewsRequestDto.Text,
            url: updateNewsRequestDto.URL,
            author: updateNewsRequestDto.Author,
            tags: updateNewsRequestDto.Tags,
            topics: updateNewsRequestDto.Topics,
            created: DateTime.Now,
            updated: DateTime.Now);

        /// <summary>
        /// Маппинг IEnumerable(Core.Tag) - IEnumerable(TagDto)
        /// </summary>
        /// <param name="tagsLst"></param>
        /// <returns></returns>
        public static IEnumerable<TagDto> GetTagDtos(this IEnumerable<Tag> tagsLst)
        {
            return tagsLst.Select(t => new TagDto
            {
                Id = t.Id,
                Tag = t.Name
            });
        }

        /// <summary>
        /// Маппинг IEnumerable(Core.Topic) - IEnumerable(TopicDto)
        /// </summary>
        /// <param name="topicsLst"></param>
        /// <returns></returns>

        public static IEnumerable<TopicDto> GetTopicDtos(this IEnumerable<Topic> topicsLst)
        {
            return topicsLst.Select(t => new TopicDto
            {
                Id = t.Id,
                Topic = t.Name
            });
        }
    }
}
