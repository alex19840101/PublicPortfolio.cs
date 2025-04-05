using NewsFeedSystem.Core.Services;

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
    }
}
