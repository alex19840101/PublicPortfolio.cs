using System.Collections.Generic;
using System.Threading.Tasks;
using NewsFeedSystem.Core.Results;

namespace NewsFeedSystem.Core.Repositories
{
    public interface INewsRepository
    {
        Task<CreateResult> Create(NewsPost newsPost);
        Task<NewsPost?> Get(uint newsId);
        Task<IEnumerable<HeadLine>> GetHeadlines(uint? minNewsId, uint? maxNewsId);
        Task<IEnumerable<HeadLine>> GetHeadlinesByTag(uint tagId, uint minNewsId);
        Task<IEnumerable<HeadLine>> ReadHeadlinesByTopic(uint topicId, uint minNewsId);

        Task<UpdateResult> Update(NewsPost newsPost);
        Task<DeleteResult> DeleteNewsPost(uint newsId);
    }
}
