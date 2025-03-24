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
    public class NewsRepository : INewsRepository
    {
        public async Task<CreateResult> Create(NewsPost newsPost)
        {
            throw new NotImplementedException();
        }

        public async Task<DeleteResult> DeleteNewsPost(uint newsId)
        {
            throw new NotImplementedException();
        }

        public async Task<NewsPost> Read(uint newsId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<HeadLine>> ReadHeadlines(uint? maxNewsId, uint? minNewsId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<HeadLine>> ReadHeadlinesByTag(uint tagId, uint minNewsId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<HeadLine>> ReadHeadlinesByTopic(uint topicId, uint minNewsId)
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateResult> Update(NewsPost newsPost)
        {
            throw new NotImplementedException();
        }
    }
}
