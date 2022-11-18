using Quartz;
using Quartz.Impl;

namespace DeliveryCorporationBot
{
    #region SchedularHelper
    public class SchedularHelper
    {
        #region static
        static SchedularHelper _instance;
        public static SchedularHelper Instance => _instance ?? (_instance = new SchedularHelper());
        #endregion
        #region private
        IScheduler _scheduler;

        SchedularHelper()
        {
            StartScheduleAsync().Wait();
        }

        async Task StartScheduleAsync()
        {
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await _scheduler.Start();
        }
        #endregion
        #region public
        public IScheduler GetScheduler() => _scheduler;
        #endregion
    }
    #endregion
}
