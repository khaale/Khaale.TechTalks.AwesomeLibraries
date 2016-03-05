using System.Net.Http;
using System.Threading.Tasks;

namespace Khaale.TechTalks.AwesomeLibraries.IntegrationService.Cli.Commands
{
    public class SchedulerResumeCommand : ApiCommandBase
    {
        public SchedulerResumeCommand()
        {
            IsCommand("scheduler-resume", "Resumes all scheduled jobs.");
            SkipsCommandSummaryBeforeRunning();
        }

        protected override Task<HttpResponseMessage> ExecuteCommand(HttpClient client)
        {
            return client.PostAsync("scheduler/resume", null);
        }
    }
}