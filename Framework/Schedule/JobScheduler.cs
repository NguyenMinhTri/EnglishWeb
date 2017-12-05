using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.Schedule
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<StudyDictJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger3", "group1")
                 // if a start time is not given (if this line were omitted), "now" is implied
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(600)
                    .RepeatForever()) // note that 10 repeats will give a total of 11 firings
                .ForJob(job) // identify job with handle to its JobDetail itself                   
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}