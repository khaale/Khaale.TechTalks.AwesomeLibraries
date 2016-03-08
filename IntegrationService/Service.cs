using System;
using Khaale.TechTalks.AwesomeLibs.Shared.ServiceDiscovery;

namespace Khaale.TechTalks.AwesomeLibs.AwesomeService
{
    public class Service
    {
        private readonly IRegistrationManager _registrationManager;

        public Service(IRegistrationManager registrationManager)
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