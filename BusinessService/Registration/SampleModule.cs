using System.Web.Http;
using Khaale.TechTalks.AwesomeLibs.BusinessService.Api.Controllers;
using Khaale.TechTalks.AwesomeLibs.Shared.ServiceDiscovery;
using Ninject.Modules;

namespace Khaale.TechTalks.AwesomeLibs.BusinessService.Registration
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