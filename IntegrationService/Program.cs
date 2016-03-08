using System.Collections.Specialized;
using System.Configuration;
using Khaale.TechTalks.AwesomeLibs.AwesomeService.Api;
using Khaale.TechTalks.AwesomeLibs.AwesomeService.Jobs;
using Khaale.TechTalks.AwesomeLibs.AwesomeService.Registration;
using Quartz;
using Quartz.Impl;
using Topshelf;
using Topshelf.Nancy;
using Topshelf.Ninject;
using Topshelf.Quartz;
using Topshelf.Quartz.Ninject;

namespace Khaale.TechTalks.AwesomeLibs.AwesomeService
{
    static class Program
    {
        public static string ServiceName { get { return "Service"; } }

        public static string Environment
        {
            get { return ConfigurationManager.AppSettings["Environment"] ?? "dev"; }
        }

        public static int ServicePort
        {
            get { return int.Parse(ConfigurationManager.AppSettings["ServicePort"] ?? "12345"); }
        }

        static void Main(string[] args)
        {
            HostFactory.Run(c =>
            {
                //c.UseNLog();
                

                // Topshelf.Ninject (Optional) - Initiates Ninject and consumes Modules
                c.UseNinject(new SampleModule());

                c.Service<Service>(s =>
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

                        n.AddHost(port: ServicePort);
                    });
                });
            });
        }
    }
}
