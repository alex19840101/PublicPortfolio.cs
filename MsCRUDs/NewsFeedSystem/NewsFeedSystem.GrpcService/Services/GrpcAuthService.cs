using Grpc.Core;
using NewsFeedSystem.Core.Auth;
using NewsFeedSystem.Core.Services;
using NewsFeedSystem.GrpcService;
using NewsFeedSystem.GrpcService.Auth;

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

        public async Task<AuthReply> RegisterUser(RegisterUserRequest registerUserRequest)
        {
            
            var authUserToRegister = new AuthUser(
                id: 0,
                login: registerUserRequest.Login,
                userName: registerUserRequest.UserName,
                email: registerUserRequest.Email,
                passwordHash: SHA256Hasher.GeneratePasswordHash(registerUserRequest.Password, registerUserRequest.RepeatPassword),
                nick: registerUserRequest.Nick,
                phone: registerUserRequest.Phone,
                role: registerUserRequest.RequestedRole,
                granterId: null,
                createdDt: DateTime.Now,
                lastUpdateDt: null);

            var registerResult = await _authService.Register(authUserToRegister);

            return new AuthReply
            {
                Id = registerResult.Id,
                Message = registerResult.Message,
                StatusCode = (int)registerResult.StatusCode
            };
        }



    }
}
