using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NewsFeedSystem.Core.Results;

namespace NewsFeedSystem.Core.Services
{
    public interface ITopicsService
    {
        Task<CreateResult> Create(Topic topic);
        Task<Topic> Get(uint topicId);
        Task<IEnumerable<Topic>> GetTopics(uint? maxTopicId, uint? minTopicId);
        Task<UpdateResult> Update(Topic topic);
        Task<DeleteResult> Delete(uint topicId);
    }
}
