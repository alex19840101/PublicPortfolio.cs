using Microsoft.AspNetCore.Authorization;
using NewsFeedSystem.Core.Results;
using NewsFeedSystem.Core.Services;
using NewsFeedSystem.GrpcService.Tags;

namespace NewsFeedSystem.GrpcService.Services
{
    public class GrpcTagsService : NewsFeedSystem.GrpcService.Tags.GrpcTags.GrpcTagsBase
    {
        private readonly ITagsService _tagsService;
        private readonly ILogger<GrpcTagsService> _logger;
        public GrpcTagsService(ITagsService tagsService, ILogger<GrpcTagsService> logger)
        {
            _tagsService = tagsService;
            _logger = logger;
        }

        [Authorize(Roles = "admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<CreateReply> CreateTopic(CreateTagRequest createTagRequest)
        {
            var createResult = await _tagsService.Create(new Core.Tag(id: 0, name: createTagRequest.Name));

            return new CreateReply
            {
                Id = createResult.Id,
                Message = createResult.Message,
                StatusCode = (int)createResult.StatusCode
            };
        }

        public async Task<TagReply?> GetTag(TagId tagIdRequest)
        {
            var tag = await _tagsService.Get(tagIdRequest.Id);

            if (tag == null)
                return null;

            return new TagReply
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }

        public async Task<TagsReply> GetTags(GetTagsRequest getTagsRequest)
        {
            var tagsList = await _tagsService.GetTags(getTagsRequest.MinTagId, getTagsRequest.MaxTagId);
            if (!tagsList.Any())
                return new TagsReply();

            var tagsReply = new TagsReply();
            var tags = tagsList.Select(t => new TagReply
            {
                Id = t.Id,
                Name = t.Name
            });
            tagsReply.Tags.AddRange(tags);

            return tagsReply;
        }

        [Authorize(Roles = "admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ResultReply> UpdateTag(UpdateTagRequest updateTagRequest)
        {
            var updateResult = await _tagsService.Update(new Core.Tag(
                id: updateTagRequest.Id,
                name: updateTagRequest.Name));

            return GetResultReply(updateResult);
        }

        [Authorize(Roles = "admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ResultReply> DeleteTag(TagId tagIdRequest)
        {
            var deleteResult = await _tagsService.Delete(tagIdRequest.Id);

            return GetResultReply(deleteResult);
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
    }
}
