using Hangfire.API.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Information(messageTemplate: "Start Hangfire API up");

try
{
    // Add services to the container.
    builder.Host.AddAppConfigurations();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(ex, messageTemplate: $"Unhanled exception: {ex.Message}");
}
finally
{
    Log.Information(messageTemplate: "Shut down Hangfire API complete");
    Log.CloseAndFlush();
}
