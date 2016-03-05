using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Khaale.TechTalks.AwesomeLibraries.Shared.ServiceDiscovery;
using Khaale.TechTalks.AwesomeLibrary.BusinessService.Api.Controllers;
using Ninject.Modules;
using Topshelf;
using Topshelf.Ninject;
using Topshelf.WebApi;
using Topshelf.WebApi.Ninject;

namespace Khaale.TechTalks.AwesomeLibrary.BusinessService
{
    public class Program
    {
        static void Main()
        {
            HostFactory.Run(c =>
            {
                c.UseNinject(new SampleModule()); //Initiates Ninject and consumes Modules

                c.Service<SampleService>(s =>
                {
                    //Specifies that Topshelf should delegate to Ninject for construction
                    s.ConstructUsingNinject();

                    s.WhenStarted((service, control) => service.Start());
                    s.WhenStopped((service, control) => service.Stop());

                    //Topshelf.WebApi - Begins configuration of an endpoint
                    s.WebApiEndpoint(api =>
                        //Topshelf.WebApi - Uses localhost as the domain, defaults to port 8080.
                        //You may also use .OnHost() and specify an alternate port.
                        api.OnLocalhost(port: 12346)
                            //Topshelf.WebApi - Pass a delegate to configure your routes
                            //.ConfigureRoutes(ConfigureRoutes)
                            .ConfigureServer(ConfigureServer)
                            //Topshelf.WebApi.Ninject (Optional) - You may delegate controller 
                            //instantiation to Ninject.
                            //Alternatively you can set the WebAPI Dependency Resolver with
                            //.UseDependencyResolver()
                            .UseNinjectDependencyResolver()
                            //Instantaties and starts the WebAPI Thread.
                            .Build());
                });
            });
        }
        
        private static void ConfigureServer(System.Web.Http.HttpConfiguration configuration)
        {
            var routes = configuration.Routes;

            configuration.MapHttpAttributeRoutes();
            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            SwaggerConfig.Register(configuration);
        }

        /*
        private static void ConfigureRoutes(System.Web.Http.HttpRouteCollection routes)
        {
            routes.MapHttpRoute(
                    "DefaultApiWithId",
                    "Api/{controller}/{id}",
                    new { id = RouteParameter.Optional },
                    new { id = @"\d+" });
        }
         */
    }

    public class SampleService
    {
        private readonly IRegistrationManager _registrationManager;

        public SampleService(IRegistrationManager registrationManager)
        {
            _registrationManager = registrationManager;
        }

        public bool Start()
        {
            _registrationManager.Register();
            Console.WriteLine("Started!");
            return true;
        }

        public bool Stop()
        {
            _registrationManager.Deregister();
            Console.WriteLine("Stopped!");
            return true;
        }
    }

    public class SampleModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRegistrationManager>().ToConstructor(_ => new RegistrationManager("BusinessService", "http://localhost", 12346, "dev")).InSingletonScope();

            Bind<ApiController>().To<ItemsController>().Named("Items");
        }
    }
}
