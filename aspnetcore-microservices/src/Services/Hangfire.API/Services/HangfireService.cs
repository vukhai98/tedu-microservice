using Hangfire.API.Services.Interfaces;
using System.Linq.Expressions;

namespace Hangfire.API.Services
{
    public class HangfireService : IScheduleJobService
    {
        public HangfireService()
        {
        }

        public string ContinueQueueWith(string parentJobId, Expression<Action> functionCall)
        {
            return BackgroundJob.ContinueJobWith(parentJobId, functionCall);
        }

        public bool Delete(string jobId)
        {
            return BackgroundJob.Delete(jobId);
        }

        public string Enqueue(Expression<Action> functionCall)
        {
            return BackgroundJob.Enqueue(functionCall);
        }

        public string Enqueue<T>(Expression<Action<T>> functionCall)
        {
            return BackgroundJob.Enqueue<T>(functionCall);
        }

        public bool Requeue(string jobId)
        {
            return BackgroundJob.Requeue(jobId);
        }

        public string Schedule(Expression<Action> functionCall, TimeSpan delay)
        {
            return BackgroundJob.Schedule(functionCall, delay);
        }

        public string Schedule<T>(Expression<Action<T>> functionCall, TimeSpan delay)
        {
            return BackgroundJob.Schedule<T>(functionCall, delay);
        }

        public string Schedule(Expression<Action> functionCall, DateTimeOffset enqueueAt)
        {
            return BackgroundJob.Schedule(functionCall, enqueueAt);
        }
    }
}
