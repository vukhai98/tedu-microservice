
using Common.Logging;
using Inventory.Grpc;
using Inventory.Grpc.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);

Log.Information(messageTemplate: "Start Inventory.Grpc startup");

try
{

    builder.Host.AddAppConfigurations();
    builder.Services.AddInfratructureServices();
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.ConfigurationMongoDBClient();

    builder.Services.AddGrpc();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.MapGrpcService<InventoryService>();
    app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, messageTemplate: "Unhanled exception");
}
finally
{
    Log.Information(messageTemplate: "Shut down Inventory.Grpc complete");
    Log.CloseAndFlush();
}
