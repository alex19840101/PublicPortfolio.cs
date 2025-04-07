using Google.Protobuf.Collections;
using Grpc.Core;
using NewsFeedSystem.GrpcService.News;

namespace NewsFeedSystem.GrpcClient
{
    internal sealed class GrpcNewsTester
    {
        private readonly GrpcNews.GrpcNewsClient _newsClient;
        private const byte DEADLINE_SECONDS = 3;
        private string _adminJwt;

        internal GrpcNewsTester(GrpcNews.GrpcNewsClient newsClient, string adminJwt)
        {
            _newsClient = newsClient;
            _adminJwt = adminJwt;
        }

        internal async Task MakeTests()
        {
            Console.WriteLine($"{nameof(GrpcNewsTester)} tests:");
            // CreateTopic
            var tags = new RepeatedField<TagId>();
            tags.Add(new TagId() { Id = 1});
            tags.Add(new TagId() { Id = 2 });

            var topics = new RepeatedField<TopicId>();
            topics.Add(new TopicId() { Id = 1 });
            topics.Add(new TopicId() { Id = 3 });

            var createNewsPostRequest = new CreateNewsPostRequest
            {
                Headline = $"Headline {Guid.NewGuid}",
                Text = $"Text {Guid.NewGuid}",
                Url = "404",
                Author = "Anonymous",
            };
            createNewsPostRequest.Tags.AddRange(tags);
            createNewsPostRequest.Topics.AddRange(topics);

            var newsPostId = await TestCreateNewsPostAsync(createNewsPostRequest, _adminJwt);

            //UpdateNewsPost
            var updateTopicRequest = new UpdateNewsRequest
            {
                Id = (uint)newsPostId,
                Headline = $"{createNewsPostRequest.Headline} upd.",
                Text = $"{createNewsPostRequest.Text} upd.",
                Author = "Incognito",
                Url = "404",
            };
            tags.Add(new TagId() { Id = 102 });
            updateTopicRequest.Tags.AddRange(tags);
            topics.Add(new TopicId() { Id = 103 });
            updateTopicRequest.Topics.AddRange(topics);

            await TestUpdateNewsPostAsync(updateTopicRequest, _adminJwt);

            // GetNewsPost
            var newsPostIdRequest = new NewsPostId { Id = (uint)newsPostId };
            await TestGetNewsPostAsync(newsPostIdRequest);

            // GetHeadlines
            var getHeadlinesRequest = new HeadlinesRequest { MinNewsId = 1, MaxNewsId = 10 };
            await TestGetHeadlinesAsync(getHeadlinesRequest);

            //GetHeadlinesByTopic
            var getHeadlinesByTopicRequest = new HeadlinesByTagOrTopicRequest { MinNewsId = 1, Id = 1 };
            await TestGetHeadlinesByTopicAsync(getHeadlinesByTopicRequest);

            //GetHeadlinesByTag
            var getHeadlinesByTagRequest = new HeadlinesByTagOrTopicRequest { MinNewsId = 1, Id = 2 };
            await TestGetHeadlinesByTagAsync(getHeadlinesByTagRequest);

            //DeleteNewsPost
            await TestDeleteNewsPostAsync(newsPostIdRequest, _adminJwt);
        }
        private async Task<uint?> TestCreateNewsPostAsync(CreateNewsPostRequest createNewsPostRequest, string? jwt = null)
        {
            try
            {
                var headers = RequestHeadersPreparator.GetMetadataWithAuthorizationHeader(jwt);

                var reply = await _newsClient.CreateNewsPostAsync(
                    createNewsPostRequest,
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

        private async Task TestGetNewsPostAsync(NewsPostId newsPostIdRequest)
        {
            try
            {
                var reply = await _newsClient.GetNewsPostAsync(
                    newsPostIdRequest,
                    deadline: DateTime.UtcNow.AddSeconds(DEADLINE_SECONDS));

                ConsoleLogger.InfoOkMessage($"NewsPost #{reply.Id} Headline: {reply.Headline} Text: {reply.Text}");
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
        private async Task TestGetHeadlinesAsync(HeadlinesRequest headlinesRequest)
        {
            try
            {
                var reply = await _newsClient.GetHeadlinesAsync(
                    headlinesRequest,
                    deadline: DateTime.UtcNow.AddSeconds(DEADLINE_SECONDS));
                foreach (var headline in reply.Headlines)
                {
                    ConsoleLogger.InfoOkMessage($"Headline #{headline.Id} {headline.Headline}");
                    foreach (var tag in headline.Tags)
                    {
                        ConsoleLogger.InfoOkMessage($"Tag: {tag.Id}");
                    }
                    foreach (var topic in headline.Topics)
                    {
                        ConsoleLogger.InfoOkMessage($"Topic: {topic.Id}");
                    }
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

        private async Task TestGetHeadlinesByTagAsync(HeadlinesByTagOrTopicRequest headlinesRequest)
        {
            try
            {
                var reply = await _newsClient.GetHeadlinesByTagAsync(
                    headlinesRequest,
                    deadline: DateTime.UtcNow.AddSeconds(DEADLINE_SECONDS));
                foreach (var headline in reply.Headlines)
                {
                    ConsoleLogger.InfoOkMessage($"Headline #{headline.Id} {headline.Headline}");
                    foreach (var tag in headline.Tags)
                    {
                        ConsoleLogger.InfoOkMessage($"Tag: {tag.Id}");
                    }
                    foreach (var topic in headline.Topics)
                    {
                        ConsoleLogger.InfoOkMessage($"Topic: {topic.Id}");
                    }
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

        private async Task TestGetHeadlinesByTopicAsync(HeadlinesByTagOrTopicRequest headlinesRequest)
        {
            try
            {
                var reply = await _newsClient.GetHeadlinesByTopicAsync(
                    headlinesRequest,
                    deadline: DateTime.UtcNow.AddSeconds(DEADLINE_SECONDS));
                foreach (var headline in reply.Headlines)
                {
                    ConsoleLogger.InfoOkMessage($"Headline #{headline.Id} {headline.Headline}");
                    foreach (var tag in headline.Tags)
                    {
                        ConsoleLogger.InfoOkMessage($"Tag: {tag.Id}");
                    }
                    foreach (var topic in headline.Topics)
                    {
                        ConsoleLogger.InfoOkMessage($"Topic: {topic.Id}");
                    }
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

        private async Task TestUpdateNewsPostAsync(UpdateNewsRequest updateNewsRequest, string? jwt = null)
        {
            try
            {
                var headers = RequestHeadersPreparator.GetMetadataWithAuthorizationHeader(jwt);

                var reply = await _newsClient.UpdateNewsPostAsync(
                    updateNewsRequest,
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

        private async Task TestDeleteNewsPostAsync(NewsPostId newsPostIdRequest, string? jwt = null)
        {
            try
            {
                var headers = RequestHeadersPreparator.GetMetadataWithAuthorizationHeader(jwt);

                var reply = await _newsClient.DeleteNewsPostAsync(
                    newsPostIdRequest,
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
