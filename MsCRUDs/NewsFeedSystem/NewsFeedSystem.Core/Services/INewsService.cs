using System.Collections.Generic;
using System.Threading.Tasks;
using NewsFeedSystem.Core.Results;

namespace NewsFeedSystem.Core.Services
{
    public interface INewsService
    {
        Task<CreateResult> Create(NewsPost newsPost);
        Task<NewsPost> Read(int newsId);
        Task<IEnumerable<HeadLine>> ReadHeadlines(int? maxNewsId, int? minNewsId);
        Task<IEnumerable<HeadLine>> ReadHeadlinesByTag(int tagId, int minNewsId);
        Task<IEnumerable<HeadLine>> ReadHeadlinesByTopic(int topicId, int minNewsId);
        
        Task<UpdateResult> Update(NewsPost newsPost);
        Task<DeleteResult> DeleteNewsPost(int newsId);
    }
}
