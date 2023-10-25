namespace Hangfire.API.Services.Interfaces
{
    public interface IBackgroundJobService
    {
        string AutoSendMail(string email, string subject, string emailContent, DateTimeOffset enqueueAt);
    }
}
