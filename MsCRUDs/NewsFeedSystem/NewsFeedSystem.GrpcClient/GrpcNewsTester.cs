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
            //rpc CreateNewsPost(CreateNewsPostRequest) returns(CreateReply);
            //rpc GetNewsPost(NewsPostId) returns(NewsPostReply);
            //rpc GetHeadlines(HeadlinesRequest) returns(HeadLinesReply);
            //rpc GetHeadlinesByTag(HeadlinesByTagOrTopicRequest) returns(HeadLinesReply);
            //rpc GetHeadlinesByTopic(HeadlinesByTagOrTopicRequest) returns(HeadLinesReply);
            //rpc UpdateNewsPost(UpdateNewsRequest) returns(ResultReply);
            //rpc DeleteNewsPost(NewsPostId) returns(ResultReply);
        }

        //rpc CreateNewsPost(CreateNewsPostRequest) returns(CreateReply);
        //rpc GetNewsPost(NewsPostId) returns(NewsPostReply);
        //rpc GetHeadlines(HeadlinesRequest) returns(HeadLinesReply);
        //rpc GetHeadlinesByTag(HeadlinesByTagOrTopicRequest) returns(HeadLinesReply);
        //rpc GetHeadlinesByTopic(HeadlinesByTagOrTopicRequest) returns(HeadLinesReply);
        //rpc UpdateNewsPost(UpdateNewsRequest) returns(ResultReply);
        //rpc DeleteNewsPost(NewsPostId) returns(ResultReply);
    }
}
