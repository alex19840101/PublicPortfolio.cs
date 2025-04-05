using NewsFeedSystem.Core.Services;

namespace NewsFeedSystem.GrpcService.Services
{
    public class GrpcTopicsService : NewsFeedSystem.GrpcService.Topics.GrpcTopics.GrpcTopicsBase
    {
        private readonly ITopicsService _topicsService;
        private readonly ILogger<GrpcTopicsService> _logger;
        public GrpcTopicsService(ITopicsService topicsService, ILogger<GrpcTopicsService> logger)
        {
            _topicsService = topicsService;
            _logger = logger;
        }
    }
}
