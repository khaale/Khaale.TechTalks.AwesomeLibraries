using System;
using System.Net.Http;
using System.Threading.Tasks;
using Khaale.TechTalks.AwesomeLibs.Shared.ServiceDiscovery;
using ManyConsole;
using Polly;

namespace Khaale.TechTalks.AwesomeLibs.IntegrationService.Cli.Commands
{
    public abstract class ApiCommandBase : ConsoleCommand
    {
        /// <summary>
        /// An example of Polly-backed error handling policy
        /// </summary>
        private static readonly Policy ErrorHandlingPolicy = Policy
            .Handle<AggregateException>()
            .WaitAndRetry(
                3,
                i => TimeSpan.FromSeconds(i),
                (ex, ts, ctx) => Console.WriteLine(((AggregateException)ex).Flatten().GetBaseException())
            );

        /// <summary>
        /// An example of Consul-backed service discovery (with fallback to app config)
        /// </summary>
        private static Uri ServiceAddress
        {
            get
            {
                var serviceLookup = new ServiceLookupWithFallback(
                    new ServiceLookup("dev"),
                    new ServiceLookupFromAppConfig());

                var address = serviceLookup.GetServiceUri("IntegrationService");
                return address;
            }
        }

        protected abstract Task<HttpResponseMessage> ExecuteCommand(HttpClient client);

        public override int Run(string[] remainingArguments)
        {
            var address = ServiceAddress;

            var httpClient = new HttpClient();
            httpClient.BaseAddress = address;

            var policyResult = ErrorHandlingPolicy
                .ExecuteAndCapture(() => ExecuteCommand(httpClient).Result);
            if (policyResult.Outcome == OutcomeType.Failure)
            {
                Console.WriteLine("Unable to execute remote command. Exiting..");
                return 0;
            }

            var result = policyResult.Result;
            result.EnsureSuccessStatusCode();

            Console.WriteLine(result.Content.ReadAsStringAsync().Result);

            return 0;
        }
    }
}