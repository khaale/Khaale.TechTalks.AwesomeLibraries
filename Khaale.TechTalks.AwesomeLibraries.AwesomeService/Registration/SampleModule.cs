using Khaale.TechTalks.AwesomeLibraries.Shared.ServiceDiscovery;
using Ninject.Modules;

namespace Khaale.TechTalks.AwesomeLibraries.AwesomeService.Registration
{
    public class SampleModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRegistrationManager>().ToConstructor(_ => new RegistrationManager("IntegrationService", "http://localhost", 12345, "dev")).InSingletonScope();
        }
    }
}