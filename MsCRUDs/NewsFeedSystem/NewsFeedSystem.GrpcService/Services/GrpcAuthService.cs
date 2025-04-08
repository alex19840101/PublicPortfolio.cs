using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using NewsFeedSystem.Core.Auth;
using NewsFeedSystem.Core.Results;
using NewsFeedSystem.Core.Services;
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

        public override async Task<AuthReply> RegisterUser(RegisterUserRequest registerUserRequest, ServerCallContext context)
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

        public override async Task<AuthReply> LoginUser(LoginRequest request, ServerCallContext context)
        {

            var loginData = new LoginData(
                login: request.Login,
                passwordHash: SHA256Hasher.GeneratePasswordHash(request.Password, request.Password),
                timeoutMinutes: request.TimeoutMinutes);

            var loginResult = await _authService.Login(loginData);

            var result = new AuthReply
            {
                Id = loginResult!.Id!.Value,
                Message = loginResult.Message,
                StatusCode = (int)loginResult.StatusCode,
                Token = loginResult.Token
            };

            return result;
        }

        [Authorize(Roles = "admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public override async Task<UserInfoReply> GetUserInfoById(GetUserInfoByIdRequest request, ServerCallContext context)
        {
            var authUser = await _authService.GetUserInfo(request.Id);

            if (authUser is null)
                return null!;

            return GetUserInfoReply(authUser);
        }

        [Authorize(Roles = "admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public override async Task<UserInfoReply> GetUserInfoByLogin(GetUserInfoByLoginRequest request, ServerCallContext context)
        {
            var authUser = await _authService.GetUserInfo(request.Login);

            if (authUser is null)
                return null!;

            return GetUserInfoReply(authUser);
        }

        [Authorize(Roles = "admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public override async Task<ResultReply> GrantRoleToUser(GrantRoleRequest request, ServerCallContext context)
        {
            var grantResult = await _authService.GrantRole(GrantRoleData(request));

            return GetResultReply(grantResult);

        }
        public override async Task<ResultReply> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            var updateResult = await _authService.UpdateAccount(UpdateAccountData(request));

            return GetResultReply(updateResult);
        }


        public override async Task<ResultReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            var deleteResult = await _authService.DeleteAccount(DeleteAccountData(request));

            return GetResultReply(deleteResult);
        }

        private static DeleteAccountData DeleteAccountData(DeleteUserRequest request)
        {
            return new DeleteAccountData(
                id: request.Id,
                login: request.Login,
                passwordHash: SHA256Hasher.GeneratePasswordHash(request.Password, request.RepeatPassword),
                granterId: request.GranterId,
                granterLogin: request.GranterLogin);
        }

        private static UserInfoReply GetUserInfoReply(AuthUser authUser) =>
            new UserInfoReply
            {
                Id = authUser.Id,
                Login = authUser.Login,
                UserName = authUser.UserName,
                Email = authUser.Email,
                Nick = authUser.Nick,
                Phone = authUser.Phone,
                Role = authUser.Role
            };

        private static GrantRoleData GrantRoleData(GrantRoleRequest request)
        {
            return new GrantRoleData(
                id: request.Id,
                login: request.Login,
                passwordHash: SHA256Hasher.GeneratePasswordHash(request.GranterPassword, repeatPassword: request.GranterPassword),
                newRole: request.NewRole,
                granterId: request.GranterId,
                granterLogin: request.GranterLogin);
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

        private static UpdateAccountData UpdateAccountData(UpdateUserRequest request)
        {
            return new UpdateAccountData(
                    id: request.Id,
                    login: request.Login,
                    userName: request.UserName,
                    email: request.Email,
                    passwordHash: SHA256Hasher.GeneratePasswordHash(request.ExistingPassword, repeatPassword: request.ExistingPassword),
                    newPasswordHash: request.NewPassword != null ? SHA256Hasher.GeneratePasswordHash(request.NewPassword, repeatPassword: request.RepeatNewPassword) : null,
                    nick: request.Nick,
                    phone: request.Phone,
                    requestedRole: request.RequestedRole);
        }
    }
}
