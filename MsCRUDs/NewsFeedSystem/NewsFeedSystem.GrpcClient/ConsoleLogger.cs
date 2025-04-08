using System.Runtime.CompilerServices;
using Grpc.Core;

namespace NewsFeedSystem.GrpcClient
{
    internal static class ConsoleLogger
    {
        internal static void InfoOkMessage(string message,
            [CallerMemberName] string? caller = null,
            ConsoleColor consoleColor = ConsoleColor.Green)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine($"OK: {caller} | {message}");
        }

        internal static void Error(Exception ex, [CallerMemberName] string? caller = null,
            ConsoleColor consoleColor = ConsoleColor.Yellow)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine($"{caller}\n{ex}");
        }


        internal static void Error(RpcException rpcException,
            [CallerMemberName] string? caller = null,
            ConsoleColor consoleColor = ConsoleColor.Yellow)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine($"{caller}\n{rpcException}\nStatus code: {rpcException.Status.StatusCode}\nDetail: {rpcException.Status.Detail}");
        }

        internal static void Warn(string message, [CallerMemberName] string? caller = null,
            ConsoleColor consoleColor = ConsoleColor.Yellow)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine($"{caller}\n{message}");
        }
    }
}
