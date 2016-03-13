using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Consul;

namespace Khaale.TechTalks.AwesomeLibs.Shared.ServiceDiscovery
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

    public class RegistrationManagerSafe : IRegistrationManager
    {
        private readonly IRegistrationManager _underlying;

        public RegistrationManagerSafe(IRegistrationManager underlying)
        {
            _underlying = underlying;
        }

        public void Register()
        {
            try
            {
                _underlying.Register();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to register the service: " + ex);
            }
        }

        public void Deregister()
        {
            try
            {
                _underlying.Deregister();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to deregister the service: " + ex);
            }
        }
    }
}
