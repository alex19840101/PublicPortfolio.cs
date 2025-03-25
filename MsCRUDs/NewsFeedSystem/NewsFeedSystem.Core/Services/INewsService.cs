using System.Collections.Generic;
using System.Threading.Tasks;
using NewsFeedSystem.Core.Results;

namespace NewsFeedSystem.Core.Services
{
    public interface INewsService
    {
        Task<CreateResult> Create(NewsPost newsPost);
        Task<NewsPost> Get(uint newsId);
        Task<IEnumerable<HeadLine>> GetHeadlines(uint? minNewsId, uint? maxNewsId);
        Task<IEnumerable<HeadLine>> GetHeadlinesByTag(uint tagId, uint minNewsId);
        Task<IEnumerable<HeadLine>> GetHeadlinesByTopic(uint topicId, uint minNewsId);
        
        Task<UpdateResult> Update(NewsPost newsPost);
        Task<DeleteResult> DeleteNewsPost(uint newsId);
    }
}
