using System.Collections.Specialized;
using Khaale.TechTalks.AwesomeLibraries.AwesomeService.Api;
using Khaale.TechTalks.AwesomeLibraries.AwesomeService.Jobs;
using Khaale.TechTalks.AwesomeLibraries.AwesomeService.Registration;
using Quartz;
using Topshelf;
using Quartz.Impl;
using Topshelf.Ninject;
using Topshelf.Quartz;
using Topshelf.Nancy;
using Topshelf.Quartz.Ninject;

namespace Khaale.TechTalks.AwesomeLibraries.AwesomeService
{
    static class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(c =>
            {
                //c.UseNLog();
                

                // Topshelf.Ninject (Optional) - Initiates Ninject and consumes Modules
                c.UseNinject(new SampleModule());

                c.Service<SampleService>(s =>
                {
                    //Topshelf.Ninject (Optional) - Construct service using Ninject
                    s.ConstructUsingNinject();

                    s.WhenStarted((service, control) => service.Start());
                    s.WhenStopped((service, control) => service.Stop());

                    // Topshelf.Quartz.Ninject (Optional) - Construct IJob instance with Ninject
                    s.UseQuartzNinject();
                    
                    //Set up misfire threshold property for testing purposes
                    ScheduleJobServiceConfiguratorExtensions.SchedulerFactory = () =>
                    {
                        var properties = new NameValueCollection();
                        properties["org.quartz.jobStore.misfireThreshold"] = "1000";

                        return new StdSchedulerFactory(properties).GetScheduler();
                    };

                    // Schedule a job to run in the background every 5 seconds.
                    // The full Quartz Builder framework is available here.
                    s.ScheduleQuartzJob(q =>
                        q.WithJob(() =>
                            JobBuilder.Create<SampleJob>().Build())
                        .AddTrigger(() =>
                            TriggerBuilder.Create()
                                .WithSimpleSchedule(builder => builder
                                    .WithIntervalInSeconds(5)
                                    .RepeatForever()
                                    .WithMisfireHandlingInstructionNextWithRemainingCount())
                                .Build())
                        );

                    // Set up REST API based on Nancy
                    s.WithNancyEndpoint(c, n =>
                    {
                        n.UseBootstrapper(
                            new NancyBootstrapper(NinjectBuilderConfigurator.Kernel));

                        n.AddHost(port: 12345);
                    });
                });
            });
        }
    }
}
