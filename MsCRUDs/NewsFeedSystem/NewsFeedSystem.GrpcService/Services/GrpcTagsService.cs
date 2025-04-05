using NewsFeedSystem.Core.Services;

namespace NewsFeedSystem.GrpcService.Services
{
    public class GrpcTagsService : NewsFeedSystem.GrpcService.Tags.GrpcTags.GrpcTagsBase
    {
        private readonly ITagsService _tagsService;
        private readonly ILogger<GrpcTagsService> _logger;
        public GrpcTagsService(ITagsService tagsService, ILogger<GrpcTagsService> logger)
        {
            _tagsService = tagsService;
            _logger = logger;
        }
    }
}
