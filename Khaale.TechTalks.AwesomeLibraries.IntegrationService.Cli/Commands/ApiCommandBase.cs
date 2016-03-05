using System;
using System.Net.Http;
using System.Threading.Tasks;
using Khaale.TechTalks.AwesomeLibraries.Shared.ServiceDiscovery;
using ManyConsole;

namespace Khaale.TechTalks.AwesomeLibraries.IntegrationService.Cli.Commands
{
    public abstract class ApiCommandBase : ConsoleCommand
    {
        protected abstract Task<HttpResponseMessage> ExecuteCommand(HttpClient client);

        public override int Run(string[] remainingArguments)
        {
            var serviceLookup = new ServiceLookup();

            var address = serviceLookup.GetServiceUri("IntegrationService", "dev");

            var httpClient = new HttpClient();
            httpClient.BaseAddress = address;

            var result = ExecuteCommand(httpClient).Result;

            result.EnsureSuccessStatusCode();

            Console.WriteLine(result.Content.ReadAsStringAsync().Result);

            return 0;
        }
    }
}