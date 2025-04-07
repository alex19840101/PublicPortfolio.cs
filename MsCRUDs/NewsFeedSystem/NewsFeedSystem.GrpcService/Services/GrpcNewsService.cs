using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using NewsFeedSystem.Core;
using NewsFeedSystem.Core.Results;
using NewsFeedSystem.Core.Services;
using NewsFeedSystem.GrpcService.News;

namespace NewsFeedSystem.GrpcService.Services
{
    public class GrpcNewsService : NewsFeedSystem.GrpcService.News.GrpcNews.GrpcNewsBase
    {
        private readonly INewsService _newsService;
        private readonly ILogger<GrpcNewsService> _logger;
        public GrpcNewsService(INewsService newsService, ILogger<GrpcNewsService> logger)
        {
            _newsService = newsService;
            _logger = logger;
        }

        [Authorize(Roles = "admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public override async Task<CreateReply> CreateNewsPost(CreateNewsPostRequest createNewsPostRequest, ServerCallContext context)
        {
            var createResult = await _newsService.Create(new Core.NewsPost(
                id: 0,
                headLine: createNewsPostRequest.Headline,
                text: createNewsPostRequest.Text,
                url: createNewsPostRequest.Url,
                author: createNewsPostRequest.Author,
                tags: createNewsPostRequest.Tags.Select(t => t.Id).ToList(),
                topics: createNewsPostRequest.Topics.Select(t => t.Id).ToList(),
                created: DateTime.Now,
                updated: null));

            return new CreateReply
            {
                Id = createResult.Id,
                Message = createResult.Message,
                StatusCode = (int)createResult.StatusCode
            };
        }

        public override async Task<NewsPostReply?> GetNewsPost(NewsPostId newsPostId, ServerCallContext context)
        {
            var newsPost = await _newsService.Get(newsPostId.Id);

            if (newsPost == null)
                return null;

            var newsPostReply = new NewsPostReply
            { Id = newsPostId.Id,
                Headline = newsPost.Headline,
                Text = newsPost.Text,
                Author = newsPost.Author,
                Created = Timestamp.FromDateTime(newsPost.Created),
                Updated = Timestamp.FromDateTime(newsPost.Updated ?? newsPost.Created),
                Url = newsPost.URL,
            };

            newsPostReply.Tags.AddRange(newsPost.Tags.Select(t => new TagId { Id = t }));
            newsPostReply.Topics.AddRange(newsPost.Topics.Select(t => new TopicId { Id = t }));

            return newsPostReply;
        }

        public override async Task<HeadLinesReply> GetHeadlines(HeadlinesRequest headlinesRequest, ServerCallContext context)
        {
            var headlinesList = await _newsService.GetHeadlines(headlinesRequest.MinNewsId, headlinesRequest.MaxNewsId);
            if (!headlinesList.Any())
                return new HeadLinesReply();

            return GetHeadlinesReply(headlinesList);
        }

        public override async Task<HeadLinesReply> GetHeadlinesByTag(HeadlinesByTagOrTopicRequest headlinesRequest, ServerCallContext context)
        {
            var headlinesList = await _newsService.GetHeadlinesByTag(headlinesRequest.Id, headlinesRequest.MinNewsId);
            if (!headlinesList.Any())
                return new HeadLinesReply();

            return GetHeadlinesReply(headlinesList);
        }

        public override async Task<HeadLinesReply> GetHeadlinesByTopic(HeadlinesByTagOrTopicRequest headlinesRequest, ServerCallContext context)
        {
            var headlinesList = await _newsService.GetHeadlinesByTopic(headlinesRequest.Id, headlinesRequest.MinNewsId);
            if (!headlinesList.Any())
                return new HeadLinesReply();

            return GetHeadlinesReply(headlinesList);
        }

        [Authorize(Roles = "admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public override async Task<ResultReply> UpdateNewsPost(UpdateNewsRequest updateNewsRequest, ServerCallContext context)
        {
            var topics = updateNewsRequest.Topics.Select(t => t.Id).ToList();

            var updateResult = await _newsService.Update(new Core.NewsPost(
                id: updateNewsRequest.Id,
                headLine: updateNewsRequest.Headline,
                text: updateNewsRequest.Text,
                url: updateNewsRequest.Url,
                author: updateNewsRequest.Url,
                topics: topics,
                tags: updateNewsRequest.Tags.Select(t => t.Id).ToList(),
                created: DateTime.Now,
                updated: DateTime.Now));

            return GetResultReply(updateResult);
        }

        [Authorize(Roles = "admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public override async Task<ResultReply> DeleteNewsPost(NewsPostId newsIdRequest, ServerCallContext context)
        {
            var deleteResult = await _newsService.DeleteNewsPost(newsIdRequest.Id);

            return GetResultReply(deleteResult);
        }

        private static ResultReply GetResultReply(UpdateResult result) => new ResultReply
        {
            StatusCode = (int)result.StatusCode,
            Message = result.Message
        };

        private static ResultReply GetResultReply(DeleteResult result) => new ResultReply
        {
            StatusCode = (int)result.StatusCode,
            Message = result.Message
        };


        /// <summary>
        /// Формирование из IEnumerable(HeadLine) ответа HeadLinesReply
        /// </summary>
        /// <param name="headlinesLst"></param>
        /// <returns></returns>

        public static HeadLinesReply GetHeadlinesReply(IEnumerable<HeadLine> headlinesLst)
        {
            var headLinesReply = new HeadLinesReply();
            foreach (var headline in headlinesLst)
            {
                var headLineReply = new HeadLineReply();
                headLineReply.Tags.AddRange(headline.Tags.Select(t => new TagId { Id = t }));
                headLineReply.Topics.AddRange(headline.Topics.Select(t => new TopicId { Id = t }));
            }
            return headLinesReply;
        }
    }
}
