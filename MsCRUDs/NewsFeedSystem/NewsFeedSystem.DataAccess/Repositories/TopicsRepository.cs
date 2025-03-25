using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsFeedSystem.Core;
using NewsFeedSystem.Core.Repositories;
using NewsFeedSystem.Core.Results;

namespace NewsFeedSystem.DataAccess.Repositories
{
    public class TopicsRepository : ITopicsRepository
    {
        public async Task<CreateResult> Create(Topic topic)
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

        public async Task<UpdateResult> Update(Topic topic)
        {
            throw new NotImplementedException();
        }
    }
}
