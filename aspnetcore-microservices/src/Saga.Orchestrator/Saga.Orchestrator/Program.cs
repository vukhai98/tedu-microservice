using Common.Logging;
using Saga.Orchestrator.Extensions;
using Saga.Orchestrator.AutoMapper;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.
Log.Information(messageTemplate: "Start Saga.Orchestrator  up");

try
{
    builder.Host.AddAppConfigurations();

    // Register DI for services 
    builder.Services.ConfigureServices();
    builder.Services.ConfigureHttpClients();

    // Register AutoMapper
    builder.Services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));


    // router url is lowercase
    builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);



    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

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
    Log.Information(messageTemplate: "Shut down Saga.Orchestrator complete");
    Log.CloseAndFlush();
}
