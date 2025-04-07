using Grpc.Core;
using NewsFeedSystem.GrpcService.Topics;

namespace NewsFeedSystem.GrpcClient
{
    internal class GrpcTopicsTester
    {
        private readonly GrpcTopics.GrpcTopicsClient _gprcTopicsClient;
        private const byte DEADLINE_SECONDS = 3;
        private string _adminJwt;
        private string _adminPassword;

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
            // CreateTopic
            var createTopicRequest = new CreateTopicRequest { Name = $"Topic {Guid.NewGuid}" };
            var topicId = await TestCreateTopicAsync(createTopicRequest, _adminJwt);

            //rpc GetTopic(TopicId) returns(TopicReply);
            //rpc GetTopics(GetTopicsRequest) returns(TopicsReply);
            //rpc UpdateTopic(UpdateTopicRequest) returns(ResultReply);
            //rpc DeleteTopic(TopicId) returns(ResultReply);
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
        //rpc CreateTopic(CreateTopicRequest) returns(CreateReply);
        //rpc GetTopic(TopicId) returns(TopicReply);
        //rpc GetTopics(GetTopicsRequest) returns(TopicsReply);
        //rpc UpdateTopic(UpdateTopicRequest) returns(ResultReply);
        //rpc DeleteTopic(TopicId) returns(ResultReply);
    }
}
