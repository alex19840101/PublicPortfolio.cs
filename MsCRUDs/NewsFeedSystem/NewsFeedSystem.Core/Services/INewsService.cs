using System.Collections.Generic;
using System.Threading.Tasks;
using NewsFeedSystem.Core.Results;

namespace NewsFeedSystem.Core.Services
{
    public interface INewsService
    {
        Task<CreateResult> Create(NewsPost newsPost);
        Task<NewsPost> Read(uint newsId);
        Task<IEnumerable<HeadLine>> ReadHeadlines(uint? maxNewsId, uint? minNewsId);
        Task<IEnumerable<HeadLine>> ReadHeadlinesByTag(uint tagId, uint minNewsId);
        Task<IEnumerable<HeadLine>> ReadHeadlinesByTopic(uint topicId, uint minNewsId);
        
        Task<UpdateResult> Update(NewsPost newsPost);
        Task<DeleteResult> DeleteNewsPost(uint newsId);
    }
}
