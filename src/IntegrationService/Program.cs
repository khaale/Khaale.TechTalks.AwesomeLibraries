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
using System;

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
                // Topshelf.Ninject (Optional) - Initiates Ninject and consumes Modules
                c.UseNinject(new SampleModule());

                c.Service<Service>(s =>
                {
                    //Topshelf.Ninject (Optional) - Construct service using Ninject
                    s.ConstructUsingNinject();


                    // Topshelf.Quartz.Ninject (Optional) - Construct IJob instance with Ninject
                    s.UseQuartzNinject();

                    ISchedulerFactory sf = new StdSchedulerFactory();
                    IScheduler sched = sf.GetScheduler();

                    var jobDetail = JobBuilder.Create<SampleJob>().Build();

                    var key = new TriggerKey("trigger-name", "trigger-group");
                    if (sched.GetTrigger(key) == null)
                    {
                        ITrigger trigger = TriggerBuilder.Create()
                            .WithIdentity(key)
                            .StartNow()
                            .WithSimpleSchedule(x => x
                                .RepeatForever()
                                .WithInterval(TimeSpan.FromMinutes(1)))
                            .Build();

                        sched.ScheduleJob(jobDetail, trigger);
                    }
                    sched.Start();

                    // Set up REST API based on Nancy
                    s.WithNancyEndpoint(c, n =>
                    {
                        n.UseBootstrapper(
                            new NancyBootstrapper(NinjectBuilderConfigurator.Kernel));

                        n.AddHost(port: ServicePort);
                    });


                    s.WhenStarted((service, control) => service.Start());
                    s.WhenStopped((service, control) => { sched.Shutdown(true); return service.Stop(); });
                });
            });
        }
    }
}
