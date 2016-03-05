using System;
using Nancy;
using Topshelf.Quartz;

namespace Khaale.TechTalks.AwesomeLibraries.AwesomeService.Api.Modules
{
    public class SchedulerModule: NancyModule
    {
        public SchedulerModule()
        {
            Post["/scheduler/pause"] = _ =>
            {
                Console.WriteLine("API: /scheduler/pause");
                var scheduler = ScheduleJobServiceConfiguratorExtensions.SchedulerFactory();
                scheduler.PauseAll();
                return Response.AsJson(new { status = "Scheduler was paused." });
            };

            Post["/scheduler/resume"] = _ =>
            {
                Console.WriteLine("API: /scheduler/resume");
                var scheduler = ScheduleJobServiceConfiguratorExtensions.SchedulerFactory();
                scheduler.ResumeAll();
                return Response.AsJson(new { status = "Scheduler was resumed." });
            };


        }
    }
}
