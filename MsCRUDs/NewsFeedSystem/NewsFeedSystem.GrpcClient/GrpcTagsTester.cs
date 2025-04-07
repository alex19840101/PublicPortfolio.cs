using Grpc.Core;
using NewsFeedSystem.GrpcService.Tags;

namespace NewsFeedSystem.GrpcClient
{
    internal class GrpcTagsTester
    {
        private readonly GrpcTags.GrpcTagsClient _authClient;
        private const byte DEADLINE_SECONDS = 3;
        private string _adminLogin;
        private string _adminPassword;

        internal GrpcTagsTester(GrpcTags.GrpcTagsClient authClient, string adminLogin, string adminPassword)
        {
            _authClient = authClient;
            _adminLogin = adminLogin;
            _adminPassword = adminPassword;
        }

        internal async Task MakeTests()
        {
            //rpc CreateTag(CreateTagRequest) returns(CreateReply);
            //rpc GetTag(TagId) returns(TagReply);
            //rpc GetTags(GetTagsRequest) returns(TagsReply);
            //rpc UpdateTag(UpdateTagRequest) returns(ResultReply);
            //rpc DeleteTag(TagId) returns(ResultReply);
        }

        //rpc CreateTag(CreateTagRequest) returns(CreateReply);
        //rpc GetTag(TagId) returns(TagReply);
        //rpc GetTags(GetTagsRequest) returns(TagsReply);
        //rpc UpdateTag(UpdateTagRequest) returns(ResultReply);
        //rpc DeleteTag(TagId) returns(ResultReply);
    }
}
