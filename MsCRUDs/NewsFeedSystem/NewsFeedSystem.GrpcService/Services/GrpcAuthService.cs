using Grpc.Core;
using NewsFeedSystem.Core.Services;
using NewsFeedSystem.GrpcService;

namespace NewsFeedSystem.GrpcService.Services
{
    public class GrpcAuthService : NewsFeedSystem.GrpcService.Auth.GrpcAuth.GrpcAuthBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<GrpcAuthService> _logger;
        public GrpcAuthService(IAuthService authService, ILogger<GrpcAuthService> logger)
        {
            _authService = authService;
            _logger = logger;
        }
    }
}
