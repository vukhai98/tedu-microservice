﻿using System.Linq.Expressions;

namespace Hangfire.API.Services.Interfaces
{
    public interface IScheduleJobService
    {
        #region Fire And Forget
        string Enqueue(Expression<Action> functionCall);
        string Enqueue<T>(Expression<Action<T>> functionCall);
        #endregion

        #region DeldayJob
        string Schedule(Expression<Action> functionCall, TimeSpan delay);
        string Schedule<T>(Expression<Action<T>> functionCall, TimeSpan delay);
        string Schedule(Expression<Action> functionCall, DateTimeOffset enqueueAt);
        //string Schedule<T>(Expression<Action<T>> functionCall, DateTimeOffset enqueueAt);
        #endregion

        #region Continuos Jobs
        string ContinueQueueWith(string parentJobId, Expression<Action> functionCall);
        #endregion

        bool Delete(string jobId);

        bool Requeue(string jobId);
    }
}
