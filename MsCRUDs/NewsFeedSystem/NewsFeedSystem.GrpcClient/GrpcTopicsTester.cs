using Grpc.Core;
using NewsFeedSystem.GrpcService.Topics;

namespace NewsFeedSystem.GrpcClient
{
    internal sealed class GrpcTopicsTester
    {
        private readonly GrpcTopics.GrpcTopicsClient _gprcTopicsClient;
        private const byte DEADLINE_SECONDS = 3;
        private string _adminJwt;

        /// <summary>
        /// Конструктор класса GrpcTopicsTester
        /// </summary>
        /// <param name="gprcTopicsClient"> GPRC-клиент для работы с темами по gRPC </param>
        /// <param name="adminJwt"> JWT-токен админа с правами добавления/удаления/изменения </param>
        internal GrpcTopicsTester(GrpcTopics.GrpcTopicsClient gprcTopicsClient, string adminJwt)
        {
            _gprcTopicsClient = gprcTopicsClient;
            _adminJwt = adminJwt;
        }

        internal async Task MakeTests()
        {
            Console.WriteLine($"{nameof(GrpcTopicsTester)} tests:");
            // CreateTopic
            var createTopicRequest = new CreateTopicRequest { Name = $"Topic {Guid.NewGuid}" };
            var topicId = await TestCreateTopicAsync(createTopicRequest, _adminJwt);

            //UpdateTopic
            var updateTopicRequest = new UpdateTopicRequest
            {
                Id = (uint)topicId,
                Name = $"{createTopicRequest.Name} upd."
            };
            await TestUpdateTopicAsync(updateTopicRequest, _adminJwt);

            // GetTopic
            var topicIdRequest = new TopicId { Id = (uint)topicId };
            await TestGetTopicAsync(topicIdRequest);

            // GetTopics
            var getTopicsRequest = new GetTopicsRequest { MinTopicId = 1, MaxTopicId = 10 };
            await TestGetTopicsAsync(getTopicsRequest);
            
            //DeleteTopic
            await TestDeleteTopicAsync(topicIdRequest, _adminJwt);
        }
        private async Task<uint?> TestCreateTopicAsync(CreateTopicRequest createTopicRequest, string? jwt = null)
        {
            try
            {
                var headers = RequestHeadersPreparator.GetMetadataWithAuthorizationHeader(jwt);

                var reply = await _gprcTopicsClient.CreateTopicAsync(
                    createTopicRequest,
                    headers,
                    deadline: DateTime.UtcNow.AddSeconds(DEADLINE_SECONDS));

                ConsoleLogger.InfoOkMessage($"{reply.Id} {reply.StatusCode} {reply.Message}");
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

        private async Task TestGetTopicAsync(TopicId topicIdRequest)
        {
            try
            {
                var reply = await _gprcTopicsClient.GetTopicAsync(
                    topicIdRequest,
                    deadline: DateTime.UtcNow.AddSeconds(DEADLINE_SECONDS));

                ConsoleLogger.InfoOkMessage($"{reply.Id} {reply.Name}");
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
        private async Task TestGetTopicsAsync(GetTopicsRequest getTopicsRequest)
        {
            try
            {
                var reply = await _gprcTopicsClient.GetTopicsAsync(
                    getTopicsRequest,
                    deadline: DateTime.UtcNow.AddSeconds(DEADLINE_SECONDS));
                foreach (var topic in reply.Topics)
                {
                    ConsoleLogger.InfoOkMessage($"Topic: {topic.Id} {topic.Name}");
                }
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
        private async Task TestUpdateTopicAsync(UpdateTopicRequest updateTopicRequest, string? jwt = null)
        {
            try
            {
                var headers = RequestHeadersPreparator.GetMetadataWithAuthorizationHeader(jwt);

                var reply = await _gprcTopicsClient.UpdateTopicAsync(
                    updateTopicRequest,
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

        private async Task TestDeleteTopicAsync(TopicId topicIdRequest, string? jwt = null)
        {
            try
            {
                var headers = RequestHeadersPreparator.GetMetadataWithAuthorizationHeader(jwt);

                var reply = await _gprcTopicsClient.DeleteTopicAsync(
                    topicIdRequest,
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
    }
}
