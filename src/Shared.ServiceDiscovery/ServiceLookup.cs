using System;
using System.Configuration;
using System.Linq;
using Consul;

namespace Khaale.TechTalks.AwesomeLibs.Shared.ServiceDiscovery
{
    public interface IServiceLookup
    {
        Uri GetServiceUri(string serviceName);
    }

    public class ServiceLookup : IServiceLookup
    {
        private readonly string _tag;

        public ServiceLookup(string tag)
        {
            _tag = tag;
        }

        public Uri GetServiceUri(string serviceName)
        {
            using (var client = new ConsulClient())
            {
                var result = client.Catalog.Service(serviceName, _tag).Result;

                var service = result.Response.FirstOrDefault();
                
                if (service == null)
                    throw new ApplicationException("Unable to find service in catalog");

                return new Uri(string.Format("{0}:{1}", service.ServiceAddress, service.ServicePort));
            }
        }
    }

    public class ServiceLookupFromAppConfig : IServiceLookup
    {
        public Uri GetServiceUri(string serviceName)
        {
            return new Uri(ConfigurationManager.AppSettings[string.Format("services.{0}.uri", serviceName)]);
        }
    }

    public class ServiceLookupWithFallback : IServiceLookup
    {
        private readonly IServiceLookup _primary;
        private readonly IServiceLookup _fallback;

        public ServiceLookupWithFallback(IServiceLookup primary, IServiceLookup fallback)
        {
            _primary = primary;
            _fallback = fallback;
        }

        public Uri GetServiceUri(string serviceName)
        {
            try
            {
                return _primary.GetServiceUri(serviceName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to get service {0} from primary source due to {1}. Trying fallback..", serviceName, ex);
                return _fallback.GetServiceUri(serviceName);
            }
        }
    }
}