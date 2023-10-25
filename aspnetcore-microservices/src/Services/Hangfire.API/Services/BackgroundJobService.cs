using Contracts.Services;
using Hangfire.API.Services.Interfaces;
using Shared.DTOs.ScheduledJob;
using Shared.Services.Email;

namespace Hangfire.API.Services
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly ISmtpEmailService _emailService;
        private readonly IScheduleJobService _scheduleJobService;
        private readonly ILogger<BackgroundJobService> _logger;

        public IScheduleJobService scheduleJobServiceProperty { get; }

        public BackgroundJobService(ISmtpEmailService emailService, ILogger<BackgroundJobService> logger, IScheduleJobService scheduleJobService)
        {
            _emailService = emailService;
            _logger = logger;
            _scheduleJobService = scheduleJobService;
        }

        public string? AutoSendMail(ReminderCheckoutOrderDto requestDto)
        {
            try
            {
                var mailRequest = new MailRequest()
                {
                    ToAddress = requestDto.Email,
                    Subject = requestDto.Subject,
                    Body = requestDto.EmailContent
                };

                var jobId = _scheduleJobService.Schedule(() => _emailService.SendEmail(mailRequest), requestDto.enqueueAt);
                _logger.LogInformation($"Sent email to {requestDto.Email} with subject:{requestDto.Subject} - JodId: {jobId}");
                return jobId;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed due to an error with the email service:{ex.Message}");
                return null;
            }
        }
    }
}
