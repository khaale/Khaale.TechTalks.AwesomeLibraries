using System.Web.Http;
using Khaale.TechTalks.AwesomeLibraries.Shared.ServiceDiscovery;
using Khaale.TechTalks.AwesomeLibrary.BusinessService.Api.Controllers;
using Ninject.Modules;

namespace Khaale.TechTalks.AwesomeLibrary.BusinessService.Registration
{
    public class SampleModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRegistrationManager>()
                .ToConstructor(_ => new RegistrationManagerSafe(
                    new RegistrationManager(Program.ServiceName, "http://localhost", Program.ServicePort, Program.Environment)))
                .InSingletonScope();

            Bind<ApiController>().To<ItemsController>().Named("Items");
        }
    }
}