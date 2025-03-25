using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsFeedSystem.Core;
using NewsFeedSystem.Core.Repositories;
using NewsFeedSystem.Core.Results;
using NewsFeedSystem.Core.Services;

namespace NewsFeedSystem.BusinessLogic
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;

        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<CreateResult> Create(NewsPost newsPost)
        {
            if (newsPost == null)
                throw new ArgumentNullException(ErrorStrings.NEWSPOST_RARAM_NAME);

            if (string.IsNullOrWhiteSpace(newsPost.Headline))
                return new CreateResult(ErrorStrings.HEADLINE_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(newsPost.Text))
                return new CreateResult(ErrorStrings.TEXT_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            var createResult = await _newsRepository.Create(newsPost);

            return createResult;
        }

        public async Task<DeleteResult> DeleteNewsPost(uint newsId)
        {
            return await _newsRepository.DeleteNewsPost(newsId);
        }

        public async Task<NewsPost> Read(uint newsId)
        {
            return await _newsRepository.Read(newsId);
        }

        public async Task<IEnumerable<HeadLine>> ReadHeadlines(uint? minNewsId, uint? maxNewsId)
        {
            return await _newsRepository.ReadHeadlines(maxNewsId, minNewsId);
        }

        public async Task<IEnumerable<HeadLine>> ReadHeadlinesByTag(uint tagId, uint minNewsId)
        {
            return await _newsRepository.ReadHeadlinesByTag(tagId, minNewsId);
        }

        public async Task<IEnumerable<HeadLine>> ReadHeadlinesByTopic(uint topicId, uint minNewsId)
        {
            return await _newsRepository.ReadHeadlinesByTopic(topicId, minNewsId);
        }

        public async Task<UpdateResult> Update(NewsPost newsPost)
        {
            if (newsPost == null)
                throw new ArgumentNullException(ErrorStrings.NEWSPOST_RARAM_NAME);

            if (string.IsNullOrWhiteSpace(newsPost.Headline))
                return new UpdateResult(ErrorStrings.HEADLINE_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(newsPost.Text))
                return new UpdateResult(ErrorStrings.TEXT_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            return await _newsRepository.Update(newsPost);
        }
    }
}
