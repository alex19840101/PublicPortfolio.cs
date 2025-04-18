﻿using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using NewsFeedSystem.Core.Results;
using NewsFeedSystem.Core.Services;
using NewsFeedSystem.GrpcService.Topics;

namespace NewsFeedSystem.GrpcService.Services
{
    public class GrpcTopicsService : NewsFeedSystem.GrpcService.Topics.GrpcTopics.GrpcTopicsBase
    {
        private readonly ITopicsService _topicsService;
        private readonly ILogger<GrpcTopicsService> _logger;
        public GrpcTopicsService(ITopicsService topicsService, ILogger<GrpcTopicsService> logger)
        {
            _topicsService = topicsService;
            _logger = logger;
        }

        [Authorize(Roles = "admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public override async Task<CreateReply> CreateTopic(CreateTopicRequest createTopicRequest, ServerCallContext context)
        {
            var createResult = await _topicsService.Create(new Core.Topic(id: 0, name: createTopicRequest.Name));

            return new CreateReply
            {
                Id = createResult.Id,
                Message = createResult.Message,
                StatusCode = (int)createResult.StatusCode
            };
        }

        public override async Task<TopicReply?> GetTopic(TopicId topicId, ServerCallContext context)
        {
            var topic = await _topicsService.Get(topicId.Id);

            if (topic == null)
                return null;

            return new TopicReply
            {
                Id = topic.Id,
                Name = topic.Name
            };
        }

        public override async Task<TopicsReply> GetTopics(GetTopicsRequest getTopicsRequest, ServerCallContext context)
        {
            var topicsList = await _topicsService.GetTopics(getTopicsRequest.MinTopicId, getTopicsRequest.MaxTopicId);
            if (!topicsList.Any())
                return new TopicsReply();
            
            var topicsReply = new TopicsReply();
            var topics = topicsList.Select(t => new TopicReply
            {
                Id = t.Id,
                Name = t.Name
            });
            topicsReply.Topics.AddRange(topics);

            return topicsReply;
        }

        [Authorize(Roles = "admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public override async Task<ResultReply> UpdateTopic(UpdateTopicRequest updateTopicRequest, ServerCallContext context)
        {
            var updateResult = await _topicsService.Update(new Core.Topic(
                id: updateTopicRequest.Id,
                name: updateTopicRequest.Name));

            return GetResultReply(updateResult);
        }

        [Authorize(Roles = "admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public override async Task<ResultReply> DeleteTopic(TopicId topicIdRequest, ServerCallContext context)
        {
            var deleteResult = await _topicsService.Delete(topicIdRequest.Id);

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
