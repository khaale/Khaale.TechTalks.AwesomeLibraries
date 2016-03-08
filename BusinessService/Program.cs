using System.Configuration;
using System.Web.Http;
using Khaale.TechTalks.AwesomeLibs.BusinessService.Registration;
using Topshelf;
using Topshelf.Ninject;
using Topshelf.WebApi;
using Topshelf.WebApi.Ninject;

namespace Khaale.TechTalks.AwesomeLibs.BusinessService
{
    public class Program
    {
        public static string ServiceName { get { return "Service"; } }

        public static string Environment
        {
            get { return ConfigurationManager.AppSettings["Environment"] ?? "dev"; }
        }

        public static int ServicePort
        {
            get { return int.Parse(ConfigurationManager.AppSettings["ServicePort"] ?? "12346"); }
        }

        static void Main()
        {
            HostFactory.Run(c =>
            {
                c.UseNinject(new SampleModule()); //Initiates Ninject and consumes Modules

                c.Service<Service>(s =>
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
    }
}
