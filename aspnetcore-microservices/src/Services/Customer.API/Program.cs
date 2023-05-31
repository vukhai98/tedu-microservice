using Common.Logging;
using Customer.API.Extensions;
using Customer.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);

Log.Information(messageTemplate: "Start Customer API up");

try
{
    builder.Host.UseSerilog(Serilogger.Configure);
    builder.Host.AddAppConfigurations();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddDbContext<CustomerContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString")));

    var app = builder.Build();
    app.UseInferastructure();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, messageTemplate: "Unhanled exception");
}
finally
{
    Log.Information(messageTemplate: "Shut down Customer API complete");
    Log.CloseAndFlush();
}
