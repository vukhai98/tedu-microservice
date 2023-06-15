using Common.Logging;
using FluentValidation;
using MediatR;
using Ordering.Application;
using Ordering.Application.Common.Behaviours;
using Ordering.Infrastructure;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog(Serilogger.Configure);

Log.Information(messageTemplate: "Start Ordering API up");
try
{
    builder.Services.AddApplicationServices();
    // Add services to the container.
    builder.Services.AddInfrastructureServices(builder.Configuration);
 
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
    Log.Fatal(ex, messageTemplate: "Unhanled exception");
}
finally
{
    Log.Information(messageTemplate: "Shut down Ordering API complete");
    Log.CloseAndFlush();
}