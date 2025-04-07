using Grpc.Core;
using NewsFeedSystem.GrpcService.News;

namespace NewsFeedSystem.GrpcClient
{
    internal class GrpcNewsTester
    {
        private readonly GrpcNews.GrpcNewsClient _authClient;
        private const byte DEADLINE_SECONDS = 3;
        private string _adminLogin;
        private string _adminPassword;

        internal GrpcNewsTester(GrpcNews.GrpcNewsClient authClient, string adminLogin, string adminPassword)
        {
            _authClient = authClient;
            _adminLogin = adminLogin;
            _adminPassword = adminPassword;
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
