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
            Console.WriteLine("Enter GranterLogin:");
            var granterLogin = Console.ReadLine();

            Console.WriteLine("Enter GranterPassword:");
            Console.ForegroundColor = ConsoleColor.Black;
            var granterPassword = Console.ReadLine();

            //Register
            var testLoginName = $"User{DateTime.Now}";
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

            //Login
            var loginRequest = new LoginRequest
            {
                Login = registerUserRequest.Login,
                Password = registerUserRequest.Password,
                TimeoutMinutes = null
            };
            var jwt = await TestLoginUserAsync(loginRequest);

            //Login
            var loginAdminRequest = new LoginRequest
            {
                Login = granterLogin,
                Password = granterPassword,
                TimeoutMinutes = null
            };
            var jwtAdmin = await TestLoginUserAsync(loginAdminRequest);

            //Update
            var updateUserRequest = new UpdateUserRequest
            {
                Id = (uint)userId,
                Login = loginRequest.Login,
                ExistingPassword = registerUserRequest.Password,
                UserName = registerUserRequest.UserName,
                Email = registerUserRequest.Email,
                Nick = $"Nick{loginRequest.Login}",
                Phone = "?",
                RequestedRole = "admin"

            };

            await TestUpdateUserAsync(updateUserRequest);
            updateUserRequest.NewPassword = Guid.NewGuid().ToString();
            updateUserRequest.RepeatNewPassword = updateUserRequest.NewPassword;
            await TestUpdateUserAsync(updateUserRequest);

            //GrantRoleToUser
            var grantRoleRequest = new GrantRoleRequest
            {
                Id = (uint)userId,
                Login = registerUserRequest.Login,
                NewRole = "admin",
                GranterId = 1,
                GranterLogin = granterLogin,
                GranterPassword = granterPassword
            };
            await TestGrantRoleToUserAsync(grantRoleRequest, jwtAdmin);

            jwt = await TestLoginUserAsync(loginRequest);

            //GetUserInfoById
            var getUserInfoByIdRequest = new GetUserInfoByIdRequest { Id = (uint)userId };
            await TestGetUserInfoByIdAsync(getUserInfoByIdRequest, jwt);

            //GetUserInfoByLogin
            var getUserInfoByLoginRequest = new GetUserInfoByLoginRequest { Login = registerUserRequest.Login };
            await TestGetUserInfoByLoginAsync(getUserInfoByLoginRequest, jwt);

            //DeleteUser
            var deleteUserRequest = new DeleteUserRequest
            {
                Id = (uint)userId,
                Login = registerUserRequest.Login,
                Password = registerUserRequest.Password,
                RepeatPassword = registerUserRequest.RepeatPassword
            };
            await TestDeleteUserAsync(deleteUserRequest);
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

        private async Task<string?> TestLoginUserAsync(LoginRequest loginRequest)
        {
            try
            {
                var reply = await _authClient.LoginUserAsync(
                    loginRequest,
                    deadline: DateTime.UtcNow.AddSeconds(DEADLINE_SECONDS));

                ConsoleLogger.InfoOkMessage($"{reply.Id} {reply.StatusCode} {reply.Message} Token: {reply.Token}");

                return reply.Token;
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

        private async Task TestUpdateUserAsync(UpdateUserRequest updateUserRequest)
        {
            try
            {
                var reply = await _authClient.UpdateUserAsync(
                    updateUserRequest,
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

        private async Task TestDeleteUserAsync(DeleteUserRequest deleteUserRequest)
        {
            try
            {
                var reply = await _authClient.DeleteUserAsync(
                    deleteUserRequest,
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
    }
}
