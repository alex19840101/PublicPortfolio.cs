using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsFeedSystem.Core;
using NewsFeedSystem.Core.Repositories;
using NewsFeedSystem.Core.Results;

namespace NewsFeedSystem.DataAccess.Repositories
{
    public class TopicsRepository : ITopicsRepository
    {
        private readonly NewsFeedSystemDbContext _dbContext;
        const int LIMIT_COUNT = 100;

        public TopicsRepository(NewsFeedSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateResult> Create(Core.Topic topic)
        {
            ArgumentNullException.ThrowIfNull(topic);

            var newTopicEntity = Entities.Topic.TopicEntity(topic);

            await _dbContext.AddAsync(newTopicEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newTopicEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id
            return new CreateResult
            {
                Id = newTopicEntity.Id,
                StatusCode = HttpStatusCode.Created
            };
        }

        public async Task<DeleteResult> Delete(uint topicId)
        {
            var entityTopic = await _dbContext.Topics
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.Id == topicId);

            if (entityTopic is null)
                return new DeleteResult(ErrorStrings.TOPIC_NOT_FOUND, HttpStatusCode.NotFound);

            _dbContext.Topics.Remove(entityTopic);
            await _dbContext.SaveChangesAsync();

            return new DeleteResult(ErrorStrings.OK, HttpStatusCode.OK);
        }

        public async Task<Topic?> Get(uint topicId)
        {
            var entityTopic = await _dbContext.Topics
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.Id == topicId);

            if (entityTopic is null)
                return null;

            return entityTopic.GetCoreTopic();
        }

        public async Task<IEnumerable<Topic>> GetTopics(uint? minTopicId, uint? maxTopicId)
        {
            if (maxTopicId == null)
            {
                maxTopicId = await _dbContext.Topics.AsNoTracking().MaxAsync(t=> t.Id);
                minTopicId ??= maxTopicId > LIMIT_COUNT ? maxTopicId - LIMIT_COUNT : 1;
            }

            minTopicId ??= maxTopicId - LIMIT_COUNT;

            List<Entities.Topic> entityTopicsLst = await _dbContext.Topics.AsNoTracking()
                .Where(t => t.Id >= minTopicId && t.Id <= maxTopicId).ToListAsync();

            if (entityTopicsLst.Count == 0)
                return [];

            return GetTopics(entityTopicsLst);
        }

        public async Task<UpdateResult> Update(Topic topic)
        {
            ArgumentNullException.ThrowIfNull(topic);

            var entityTopic = await _dbContext.Topics
                .SingleOrDefaultAsync(t => t.Id == topic.Id);

            if (entityTopic is null)
                return new UpdateResult(ErrorStrings.NEWS_NOT_FOUND, HttpStatusCode.NotFound);

            if (!string.Equals(topic.Name, entityTopic.Name)) entityTopic.UpdateTopicName(topic.Name);

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
                return new UpdateResult(ErrorStrings.TOPIC_UPDATED, HttpStatusCode.OK);
            }
            return new UpdateResult(ErrorStrings.TOPIC_IS_ACTUAL, HttpStatusCode.OK);
        }

        private static IEnumerable<Topic> GetTopics(IEnumerable<Entities.Topic> entityTopicsLst)
        {
            return entityTopicsLst.Select(t => new Topic(
                        id: t.Id,
                        name: t.Name));
        }
    }
}
