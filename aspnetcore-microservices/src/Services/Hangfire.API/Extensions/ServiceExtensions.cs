using Contracts.Services;
using Hangfire.API.Services;
using Hangfire.API.Services.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Shared.Configurations;

namespace Hangfire.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var hangFireSettings = configuration.GetSection(nameof(HangFireSettings)).Get<HangFireSettings>();

            services.AddSingleton(hangFireSettings);

            var emailSettings = configuration.GetSection(nameof(SmtpEmailSettings)).Get<SmtpEmailSettings>();

            services.AddSingleton(emailSettings);
            return services;
        }
        public static IServiceCollection AddConfigurationServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IScheduleJobService), typeof(HangfireService));
            services.AddTransient(typeof(IBackgroundJobService), typeof(BackgroundJobService));
            services.AddScoped(typeof(ISmtpEmailService), typeof(SMTPEmailServices));

            return services;
        }
    }
}
