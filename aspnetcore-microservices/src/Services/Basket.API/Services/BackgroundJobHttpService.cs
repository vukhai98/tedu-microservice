using Shared.Configurations;

namespace Basket.API.Services
{
    public class BackgroundJobHttpService
    {
        public HttpClient Client { get; }

        public string ScheduledJobUrl { get; set; }
        public BackgroundJobHttpService(HttpClient client, BackgroundJobSettings settings)
        {
            Client = client;
            client.BaseAddress = new Uri(settings.HangfireUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            ScheduledJobUrl = settings.ScheduleJobUrl;
        }
    }
}
