using Basket.API.AutoMapper;
using Basket.API.Extensions;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Common.Logging;
using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.
builder.Host.UseSerilog(Serilogger.Configure);
Log.Information(messageTemplate: "Start Basket API up");

try
{
    builder.Host.AddAppConfigurations();
    builder.Services.AddGrpc();

    // Register DI for services 
    builder.Services.ConfigureServices();

    builder.Services.AddConfigurationSettings(builder.Configuration);

    builder.Services.ConfigureGrpcServices(builder.Configuration);

    // Register Redis 
    builder.Services.ConfigureRedis(builder.Configuration);

    // Register AutoMapper
    builder.Services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));


    // router url is lowercase
    builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

    // Configure MassTransit
    builder.Services.ConfigueMassTransit();



    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.MapGrpcService<StockItemGrpcService>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName}v1"));
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, messageTemplate: "Unhanled exception");
}
finally
{
    Log.Information(messageTemplate: "Shut down Basket API complete");
    Log.CloseAndFlush();
}
