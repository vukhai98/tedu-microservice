using Common.Logging;
using Ocelot.Middleware;
using OcelotApiGw.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);

Log.Information(messageTemplate: "Start OcelotApiGateWays startup");

try
{

    // Add services to the container.

    builder.Host.AddAppConfigurations();
    builder.Services.AddControllers();
    builder.Services.AddConfigurationSettings(builder.Configuration);
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.ConfigureOcelot(builder.Configuration);
    builder.Services.ConfigureCors(builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("CorsPolicy");

    //app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.UseOcelot();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, messageTemplate: "Unhanled exception");
}
finally
{
    Log.Information(messageTemplate: "Shut down OcelotApiGateWays complete");
    Log.CloseAndFlush();
}

