using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace NotifierByEmail.API.Services.gRPC
{
    //[Obsolete("Test class for gRPC smoke tests. Don't use!")]
    //public class GreeterService : Greeter.GreeterBase
    //{
    //    private readonly ILogger<GreeterService> _logger;
    //    public GreeterService(ILogger<GreeterService> logger)
    //    {
    //        _logger = logger;
    //    }

    //    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    //    {
    //        return Task.FromResult(new HelloReply
    //        {
    //            Message = "Hello " + request.Name
    //        });
    //    }
    //}
}
