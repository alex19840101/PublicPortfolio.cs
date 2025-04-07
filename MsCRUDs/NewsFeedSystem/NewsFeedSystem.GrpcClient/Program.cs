// See https://aka.ms/new-console-template for more information
using Grpc.Net.Client;
using NewsFeedSystem.GrpcClient;
using NewsFeedSystem.GrpcService.Auth;

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


Console.WriteLine($"\nPress any key to exit");

Console.ReadLine();

