using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using NewsFeedSystem.GrpcService.Auth;

namespace NewsFeedSystem.GrpcClient
{
    internal class GrpcAuthTester
    {
        private readonly GrpcAuth.GrpcAuthClient _authClient;
        private const byte DEADLINE_SECONDS = 3;

        internal GrpcAuthTester(GrpcAuth.GrpcAuthClient authClient)
        {
            _authClient = authClient;
        }

        internal async Task MakeTests()
        {
            
            Console.WriteLine("Enter GranterPassword:");
            Console.ForegroundColor = ConsoleColor.Black;
            var granterPassword = Console.ReadLine();

            var getUserInfoByIdRequest = new GetUserInfoByIdRequest { Id = 1 };
            await TestGetUserInfoByIdAsync(getUserInfoByIdRequest);
            var granterLogin = "login";
            var getUserInfoByLoginRequest = new GetUserInfoByLoginRequest { Login = "login" };
            await TestGetUserInfoByLoginAsync(getUserInfoByLoginRequest);
            
            //Register
            var testLoginName = $"User{DateTime.Now.Date}";
            var pass = Guid.NewGuid().ToString();
            var registerUserRequest = new RegisterUserRequest
            {
                Login = testLoginName,
                UserName = testLoginName,
                Email = "{testLoginName}@404.fake".Replace(" ", ""),
                Password = pass,
                RepeatPassword = pass,
                Nick = null,
                Phone = null,
                RequestedRole = null
            };
            var userId = await TestRegisterUserAsync(registerUserRequest);

            //GrantRoleToUser
            var grantRoleRequest = new GrantRoleRequest
            {
                Id = (uint)userId,
                Login = registerUserRequest.Login,
                NewRole = "fakeRole",
                GranterId = 1,
                GranterLogin = granterLogin,
                GranterPassword = granterPassword
            };
            await TestGrantRoleToUserAsync(grantRoleRequest);

        }

        private async Task TestGetUserInfoByIdAsync(GetUserInfoByIdRequest getUserInfoByIdRequest, string? jwt = null)
        {
            try
            {
                var headers = RequestHeadersPreparator.GetMetadataWithAuthorizationHeader(jwt);
                
                var reply = await _authClient.GetUserInfoByIdAsync(
                    getUserInfoByIdRequest,
                    headers,
                    deadline: DateTime.UtcNow.AddSeconds(DEADLINE_SECONDS));

                ConsoleLogger.InfoOkMessage($"{reply.Id} | {reply.UserName} | {reply.Login}");
            }
            catch (RpcException rpcException)
            {
                ConsoleLogger.Error(rpcException);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex);
            }
        }

        private async Task TestGetUserInfoByLoginAsync(GetUserInfoByLoginRequest getUserInfoByLoginRequest, string? jwt = null)
        {
            try
            {
                var headers = RequestHeadersPreparator.GetMetadataWithAuthorizationHeader(jwt);

                var reply = await _authClient.GetUserInfoByLoginAsync(
                    getUserInfoByLoginRequest,
                    headers,
                    deadline: DateTime.UtcNow.AddSeconds(DEADLINE_SECONDS));

                ConsoleLogger.InfoOkMessage($"{reply.Id} | {reply.UserName} | {reply.Login}");
            }
            catch (RpcException rpcException)
            {
                ConsoleLogger.Error(rpcException);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex);
            }
        }

        private async Task TestGrantRoleToUserAsync(GrantRoleRequest grantRoleRequest, string? jwt = null)
        {
            try
            {
                var headers = RequestHeadersPreparator.GetMetadataWithAuthorizationHeader(jwt);

                var reply = await _authClient.GrantRoleToUserAsync(
                    grantRoleRequest,
                    headers,
                    deadline: DateTime.UtcNow.AddSeconds(DEADLINE_SECONDS));

                ConsoleLogger.InfoOkMessage($"{reply.StatusCode} {reply.Message}");
            }
            catch (RpcException rpcException)
            {
                ConsoleLogger.Error(rpcException);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex);
            }
        }


        private async Task<uint?> TestRegisterUserAsync(RegisterUserRequest registerUserRequest)
        {
            try
            {
                var reply = await _authClient.RegisterUserAsync(
                    registerUserRequest,
                    deadline: DateTime.UtcNow.AddSeconds(DEADLINE_SECONDS));

                ConsoleLogger.InfoOkMessage($"{reply.Id} {reply.StatusCode} {reply.Message} Token: {reply.Token}");
                
                return reply.Id;
            }
            catch (RpcException rpcException)
            {
                ConsoleLogger.Error(rpcException);
                return null;
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex);
                return null;
            }
        }
    }
}
