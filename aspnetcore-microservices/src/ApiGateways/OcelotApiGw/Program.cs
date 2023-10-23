using Common.Logging;
using Ocelot.Middleware;
using OcelotApiGw.Extensions;
using Serilog;
using Ocelot.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);

Log.Information(messageTemplate: "Start OcelotApiGateWays startup");

try
{
    // Add services to the container.
    builder.Host.AddAppConfigurations();
    builder.Services.AddControllers();
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.ConfigureOcelot(builder.Configuration);
    builder.Services.ConfigureCors(builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName}v1"));
    }

    app.UseCors("CorsPolicy");

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    await app.UseOcelot();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, messageTemplate: $"Unhandled exception: {ex}");
}
finally
{
    Log.Information(messageTemplate: "Shut down OcelotApiGateWays complete");
    Log.CloseAndFlush();
}
