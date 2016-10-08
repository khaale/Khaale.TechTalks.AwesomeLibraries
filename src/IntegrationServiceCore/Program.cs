using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ServiceDiscoveryCore;

namespace IntegrationServiceCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AssemblyLoadContext.Default.Unloading += context =>
            {
                Console.WriteLine("Context unload");
            };
            var cts = new CancellationTokenSource();
            try
            {
                Console.CancelKeyPress += (ConsoleCancelEventHandler)((sender, eventArgs) =>
                {
                    cts.Cancel();
                    eventArgs.Cancel = true;
                });

                Startup();

                Run(cts.Token);
            }
            finally
            {
                cts.Dispose();
            }
        }

        private static void Run(CancellationToken cancellationToken)
        {
            Console.WriteLine("Running..");
            cancellationToken.WaitHandle.WaitOne();
            Console.WriteLine("Stopped!");
        }

        private static void Startup()
        {
            Console.WriteLine("Starting..");
            var hostName = Dns.GetHostName();
            Console.WriteLine($"Host name: {hostName}");

            var addrs = Dns.GetHostAddressesAsync(hostName).Result;
            foreach (var addr in addrs)
            {
                Console.WriteLine($"IP address {addr.AddressFamily}: {addr}");
            }
            var pickedAddr = addrs.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork && Regex.IsMatch(a.ToString(), @"\d+.\d+.\d+.\d+"));

            var registrationManager = new RegistrationManager("IntegrationService", $"http://{pickedAddr}", 5001, "dev");
            registrationManager.Register();

            Console.WriteLine("Started!");
        }
    }
}
