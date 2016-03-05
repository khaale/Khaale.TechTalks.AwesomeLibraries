using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Consul;

namespace Khaale.TechTalks.AwesomeLibraries.Shared.ServiceDiscovery
{
    public interface IRegistrationManager
    {
        void Register();
        void Deregister();
    }

    public class RegistrationManager : IRegistrationManager, IDisposable
    {
        private readonly string _serviceName;
        private readonly string _address;
        private readonly int _port;
        private readonly string[] _tags;
        private readonly ConsulClient _client;
        private readonly CancellationTokenSource _cts;

        public RegistrationManager(string serviceName, string address, int port, params string[] tags)
        {
            _serviceName = serviceName;
            _address = address;
            _port = port;
            _tags = tags;

            _client = new ConsulClient();
            _cts = new CancellationTokenSource();
        }

        public void Register()
        {
            var check = new AgentServiceCheck
            {
                TTL = TimeSpan.FromMinutes(1),
                Status = CheckStatus.Warning
            };

            var registration = new AgentServiceRegistration
            {
                Address = _address,
                ID = GetServiceId(),
                Name = _serviceName,
                Check = check,
                Tags = _tags,
                Port = _port
            };

            var result = _client.Agent.ServiceRegister(registration).Result;
            Console.WriteLine("Service {0} registered. Response status code: {1}", _serviceName, result.StatusCode);

            Task.Run(() => PassTtl(_cts.Token), _cts.Token);
        }

        public void Deregister()
        {
            _cts.Cancel();

            var result = _client.Agent.ServiceDeregister(GetServiceId()).Result;
            Console.WriteLine("Service {0} deregistered. Response status code: {1}", _serviceName, result.StatusCode);
        }

        private string GetServiceId()
        {
            return _serviceName + Assembly.GetExecutingAssembly().Location.GetHashCode();
        }

        private void PassTtl(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                _client.Agent.PassTTL("service:" + GetServiceId(), "Test Check").GetAwaiter().GetResult();

                token.WaitHandle.WaitOne(TimeSpan.FromSeconds(30));
            }
        }

        public void Dispose()
        {
            if (_client != null)
            {
                _client.Dispose();
            }
        }
    }

    public interface IServiceLookup
    {
        Uri GetServiceUri(string serviceName, string tag);
    }

    public class ServiceLookup : IServiceLookup
    {
        public Uri GetServiceUri(string serviceName, string tag)
        {
            using (var client = new ConsulClient())
            {
                var result = client.Catalog.Service(serviceName, tag).Result;

                var service = result.Response.FirstOrDefault();
                
                if (service == null)
                    throw new ApplicationException("Unable to find service in catalog");

                return new Uri(string.Format("{0}:{1}", service.ServiceAddress, service.ServicePort));
            }
        }
    }
}
