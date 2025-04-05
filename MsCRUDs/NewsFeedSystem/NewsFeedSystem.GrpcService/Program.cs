using NewsFeedSystem.GrpcService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
//app.MapGrpcService<GreeterService>();
//app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.UseRouting();
app.UseEndpoints(static endpoints =>
{
    endpoints.MapGrpcService<GreeterService>();
    endpoints.MapGrpcService<GrpcAuthService>();
    endpoints.MapGrpcService<GrpcNewsService>();
    endpoints.MapGrpcService<GrpcTagsService>();
    endpoints.MapGrpcService<GrpcTopicsService>();
    //endpoints.MapGrpcReflectionService();
});

app.Run();