using Khaale.TechTalks.AwesomeLibraries.AwesomeService.Api.Modules;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Ninject;
using Ninject;

namespace Khaale.TechTalks.AwesomeLibraries.AwesomeService.Api
{
    public class NancyBootstrapper : NinjectNancyBootstrapper
    {
        private readonly IKernel _kernel;

        public NancyBootstrapper(IKernel kernel)
        {
            _kernel = kernel;
            _kernel.Load(new FactoryModule());
        }

        protected override IKernel GetApplicationContainer()
        {
            return _kernel;
        }


        protected override void ApplicationStartup(IKernel container, IPipelines pipelines)
        {
            // No registrations should be performed in here, however you may
            // resolve things that are needed during application startup.
        }

        protected override void ConfigureApplicationContainer(IKernel existingContainer)
        {
            existingContainer.Bind<NancyModule>().To<ProcessingModule>().Named(typeof(ProcessingModule).Name);
            existingContainer.Bind<NancyModule>().To<SchedulerModule>().Named(typeof(SchedulerModule).Name);
        }

        protected override void ConfigureRequestContainer(IKernel container, NancyContext context)
        {
        }

        protected override void RequestStartup(IKernel container, IPipelines pipelines, NancyContext context)
        {
            // No registrations should be performed in here, however you may
            // resolve things that are needed during request startup.
        }
    }
}
