using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsFeedSystem.Core;
using NewsFeedSystem.Core.Repositories;
using NewsFeedSystem.Core.Results;
using NewsFeedSystem.Core.Services;

namespace NewsFeedSystem.BusinessLogic
{
    public class TopicsService : ITopicsService
    {
        private readonly ITopicsRepository _topicsRepository;

        public TopicsService(ITopicsRepository topicsRepository)
        {
            _topicsRepository = topicsRepository;
        }

        public async Task<CreateResult> Create(Topic topic)
        {
            if (topic == null)
                throw new ArgumentNullException(ErrorStrings.TOPIC_RARAM_NAME);

            if (string.IsNullOrWhiteSpace(topic.Name))
                return new CreateResult(ErrorStrings.TOPIC_NAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);


            var createResult = await _topicsRepository.Create(topic);

            return createResult;
        }

        public async Task<DeleteResult> Delete(uint topicId)
        {
            return await _topicsRepository.Delete(topicId);
        }

        public async Task<Topic?> Get(uint topicId)
        {
            return await _topicsRepository.Get(topicId);
        }

        public async Task<IEnumerable<Topic>> GetTopics(uint? minTopicId, uint? maxTopicId)
        {
            if (minTopicId > maxTopicId)
                return new List<Topic>();

            return await _topicsRepository.GetTopics(minTopicId, maxTopicId);
        }

        public async Task<UpdateResult> Update(Topic topic)
        {
            if (topic == null)
                throw new ArgumentNullException(ErrorStrings.TOPIC_RARAM_NAME);

            if (string.IsNullOrWhiteSpace(topic.Name))
                return new UpdateResult(ErrorStrings.TOPIC_NAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            return await _topicsRepository.Update(topic);
        }
    }
}
