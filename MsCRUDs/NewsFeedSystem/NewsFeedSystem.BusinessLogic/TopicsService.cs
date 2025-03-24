using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task<CreateResult> Create(Topic request)
        {
            throw new NotImplementedException();
        }

        public async Task<DeleteResult> Delete(uint topicId)
        {
            throw new NotImplementedException();
        }

        public async Task<Topic> Get(uint topicId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Topic>> GetTopics(uint? maxTopicId, uint? minTopicId)
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateResult> Update(Topic request)
        {
            throw new NotImplementedException();
        }
    }
}
