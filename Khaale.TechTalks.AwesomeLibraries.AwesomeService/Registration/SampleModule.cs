using Khaale.TechTalks.AwesomeLibraries.Shared.ServiceDiscovery;
using Ninject.Modules;

namespace Khaale.TechTalks.AwesomeLibraries.AwesomeService.Registration
{
    public class SampleModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRegistrationManager>()
                .ToConstructor(_ => new RegistrationManagerSafe(
                    new RegistrationManager(Program.ServiceName, "http://localhost", Program.ServicePort, Program.Environment)))
                .InSingletonScope();
        }
    }
}