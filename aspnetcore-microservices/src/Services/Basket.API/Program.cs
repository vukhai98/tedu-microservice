using Basket.API.Extensions;
using Common.Logging;
using Product.API.Repositories.Interfaces;
using Product.API.Repositories;
using Serilog;
using Basket.API.Repositories.Interfaces;
using Basket.API.Repositories;
using Contracts.Common.Interfaces;
using Infrastructure.Common;

var builder = WebApplication.CreateBuilder(args);
    // Add services to the container.
    builder.Host.UseSerilog(Serilogger.Configure);
Log.Information(messageTemplate: "Start Basket API up");

try
{

    builder.Host.AddAppConfigurations();

    //                .AddScoped<ISerializeService, SerializeService>();
    
    //builder.Services.AddStackExchangeRedisCache(options =>
    //{
    //    options.Configuration = builder.Configuration.GetConnectionString("DefaultConnectionString");
    //});

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
    Log.Information(messageTemplate: "Shut down Basket API complete");
    Log.CloseAndFlush();
}
