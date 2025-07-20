using System.Collections.Generic;
using System.Threading.Tasks;
using NewsFeedSystem.Core.Results;

namespace NewsFeedSystem.Core.Repositories
{
    public interface ITopicsRepository
    {
        Task<CreateResult> Create(Topic topic);
        Task<Topic?> Get(uint topicId);
        Task<IEnumerable<Topic>> GetTopics(uint? minTopicId, uint? maxTopicId);
        Task<UpdateResult> Update(Topic topic);
        Task<DeleteResult> Delete(uint topicId);
    }
}
