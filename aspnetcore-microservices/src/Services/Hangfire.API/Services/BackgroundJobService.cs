using Contracts.Services;
using Hangfire.API.Services.Interfaces;

namespace Hangfire.API.Services
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly ISmtpEmailService _emailService;
        private readonly ILogger<BackgroundJobService> _logger;

        public BackgroundJobService(ISmtpEmailService emailService, ILogger<BackgroundJobService> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }
        
        public string AutoSendMail(string email, string subject, string emailContent, DateTimeOffset enqueueAt)
        {
            throw new NotImplementedException();
        }
    }
}
