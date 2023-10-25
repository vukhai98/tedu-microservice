using Shared.DTOs.ScheduledJob;

namespace Hangfire.API.Services.Interfaces
{
    public interface IBackgroundJobService
    {
        public IScheduleJobService scheduleJobServiceProperty { get; }

        string? AutoSendMail(ReminderCheckoutOrderDto requestDto);
    }
}
