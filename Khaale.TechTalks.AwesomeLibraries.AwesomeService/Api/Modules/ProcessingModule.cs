using System;
using Nancy;

namespace Khaale.TechTalks.AwesomeLibraries.AwesomeService.Api.Modules
{
    public class ProcessingModule : NancyModule
    {
        public ProcessingModule()
        {
            Post["/process/all"] = _ =>
            {
                Console.WriteLine("API: /process/all");
                return Response.AsJson(new { status = "All items were processed." });
            };

            Post["/process/{id}"] = _ =>
            {
                Console.WriteLine("API: /process/{0}", _.id);
                return Response.AsJson(new { status = string.Format("Item {0} was processed.", _.id) });
            };
        }
    }
}
