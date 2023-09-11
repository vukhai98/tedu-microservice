using Common.Logging;
using Contracts.Common.Interfaces;
using Customer.API.Repositories;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services;
using Customer.API.Services.Interfaces;
using Infrastructure.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Information(messageTemplate: "Start Product API up");
try
{
    builder.Host.UseSerilog(Serilogger.Configure);

    // Add services to the container.
    builder.Services.AddDbContext<CustomerContext>(options =>
           options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString")));

    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>))
                    .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
                    .AddScoped(typeof(ICustomerServices), typeof(CustomerServices))
                    .AddScoped(typeof(ICustomerRepository), typeof(CustomerRepository));

    var app = builder.Build();

    app.MapGet("/", () => "Welcome to Customer API !");
    app.MapGet("/api/customers/{userName}", async (ICustomerServices service, string userName) =>
    {
        var reulst = await service.GetCustomerByUserName(userName);

        return reulst;
    });
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
    Log.Information(messageTemplate: "Shut down Product API complete");
    Log.CloseAndFlush();
}