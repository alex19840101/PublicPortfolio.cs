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

        internal GrpcAuthTester(GrpcAuth.GrpcAuthClient authClient)
        {
            _authClient = authClient;
        }

        internal async Task MakeTests()
        {
            var getUserInfoByIdRequest = new GetUserInfoByIdRequest { Id = 1 };
            await TestGetUserInfoByIdAsync(getUserInfoByIdRequest);

            var getUserInfoByLoginRequest = new GetUserInfoByLoginRequest { Login = "login" };
            await TestGetUserInfoByLoginAsync(getUserInfoByLoginRequest);
        }

        private async Task TestGetUserInfoByIdAsync(GetUserInfoByIdRequest getUserInfoByIdRequest)
        {
            try
            {
                var reply = await _authClient.GetUserInfoByIdAsync(getUserInfoByIdRequest);
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

        private async Task TestGetUserInfoByLoginAsync(GetUserInfoByLoginRequest getUserInfoByLoginRequest)
        {
            try
            {
                var reply = await _authClient.GetUserInfoByLoginAsync(getUserInfoByLoginRequest);
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
    }
}
