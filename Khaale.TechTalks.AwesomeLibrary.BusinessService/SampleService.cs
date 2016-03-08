using System;
using Khaale.TechTalks.AwesomeLibraries.Shared.ServiceDiscovery;

namespace Khaale.TechTalks.AwesomeLibrary.BusinessService
{
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
}