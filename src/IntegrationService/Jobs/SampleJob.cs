using System;
using Quartz;

namespace Khaale.TechTalks.AwesomeLibs.AwesomeService.Jobs
{
    public class SampleJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("SCHEDULER: Job has been done!");
        }
    }
}