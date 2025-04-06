// See https://aka.ms/new-console-template for more information
using Grpc.Net.Client;
using NewsFeedSystem.GrpcClient;
using NewsFeedSystem.GrpcService.Auth;

Console.WriteLine("NewsFeedSystem.GrpcClient started");

using var channel = GrpcChannel.ForAddress("https://localhost:7092");
var authClient = new NewsFeedSystem.GrpcService.Auth.GrpcAuth.GrpcAuthClient(channel);

var grpcAuthTester = new GrpcAuthTester(authClient);
grpcAuthTester.MakeTests();


Console.WriteLine($"\nPress any key to exit");

Console.ReadLine();

