// See https://aka.ms/new-console-template for more information
using Grpc.Net.Client;
using NewsFeedSystem.GrpcClient;

Console.WriteLine("NewsFeedSystem.GrpcClient started");

using var channel = GrpcChannel.ForAddress("https://localhost:7092");
var authClient = new NewsFeedSystem.GrpcService.Auth.GrpcAuth.GrpcAuthClient(channel);

Console.WriteLine("Enter GranterLogin:");
var granterLogin = Console.ReadLine();

Console.WriteLine("Enter GranterPassword:");
Console.ForegroundColor = ConsoleColor.Black;
var granterPassword = Console.ReadLine();

var grpcAuthTester = new GrpcAuthTester(authClient, granterLogin!, granterPassword!);
await grpcAuthTester.MakeTests();
var adminJwt = await grpcAuthTester.CreateTempAdmin();

var newsClient = new NewsFeedSystem.GrpcService.News.GrpcNews.GrpcNewsClient(channel);
var grpcNewsTester = new GrpcNewsTester(newsClient, adminJwt);

var tagsClient = new NewsFeedSystem.GrpcService.Tags.GrpcTags.GrpcTagsClient(channel);
var grpcTagsTester = new GrpcTagsTester(tagsClient, adminJwt);

var topicsClient = new NewsFeedSystem.GrpcService.Topics.GrpcTopics.GrpcTopicsClient(channel);
var grpcTopicsTester = new GrpcTopicsTester(topicsClient, adminJwt);

await grpcAuthTester.DeleteTempAdmin();


Console.WriteLine($"\nPress any key to exit");

Console.ReadLine();

