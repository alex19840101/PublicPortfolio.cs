using Grpc.Core;
using NewsFeedSystem.GrpcService.Tags;

namespace NewsFeedSystem.GrpcClient
{
    internal sealed class GrpcTagsTester
    {
        private readonly GrpcTags.GrpcTagsClient _gprcTagsClient;
        private const byte DEADLINE_SECONDS = 3;
        private string _adminJwt;

        internal GrpcTagsTester(GrpcTags.GrpcTagsClient gprcTagsClient, string adminJwt)
        {
            _gprcTagsClient = gprcTagsClient;
            _adminJwt = adminJwt;
        }

        internal async Task MakeTests()
        {
            // CreateTag
            var createTagRequest = new CreateTagRequest { Name = $"Tag {Guid.NewGuid}" };
            var TagId = await TestCreateTagAsync(createTagRequest, _adminJwt);

            //UpdateTag
            var updateTagRequest = new UpdateTagRequest
            {
                Id = (uint)TagId,
                Name = $"{createTagRequest.Name} upd."
            };
            await TestUpdateTagAsync(updateTagRequest, _adminJwt);

            // GetTag
            var tagIdRequest = new TagId { Id = (uint)TagId };
            await TestGetTagAsync(tagIdRequest);

            // GetTags
            var getTagsRequest = new GetTagsRequest { MinTagId = 10, MaxTagId = 10 };
            await TestGetTagsAsync(getTagsRequest);

            //DeleteTag
            await TestDeleteTagAsync(tagIdRequest);
        }
        private async Task<uint?> TestCreateTagAsync(CreateTagRequest createTagRequest, string? jwt = null)
        {
            try
            {
                var headers = RequestHeadersPreparator.GetMetadataWithAuthorizationHeader(jwt);

                var reply = await _gprcTagsClient.CreateTagAsync(
                    createTagRequest,
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

        private async Task TestGetTagAsync(TagId tagIdRequest)
        {
            try
            {
                var reply = await _gprcTagsClient.GetTagAsync(
                    tagIdRequest,
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
        private async Task TestGetTagsAsync(GetTagsRequest getTagsRequest)
        {
            try
            {
                var reply = await _gprcTagsClient.GetTagsAsync(
                    getTagsRequest,
                    deadline: DateTime.UtcNow.AddSeconds(DEADLINE_SECONDS));
                foreach (var tag in reply.Tags)
                {
                    ConsoleLogger.InfoOkMessage($"Tag: {tag.Id} {tag.Name}");
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
        private async Task TestUpdateTagAsync(UpdateTagRequest updateTagRequest, string? jwt = null)
        {
            try
            {
                var headers = RequestHeadersPreparator.GetMetadataWithAuthorizationHeader(jwt);

                var reply = await _gprcTagsClient.UpdateTagAsync(
                    updateTagRequest,
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

        private async Task TestDeleteTagAsync(TagId tagIdRequest, string? jwt = null)
        {
            try
            {
                var headers = RequestHeadersPreparator.GetMetadataWithAuthorizationHeader(jwt);

                var reply = await _gprcTagsClient.DeleteTagAsync(
                    tagIdRequest,
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
