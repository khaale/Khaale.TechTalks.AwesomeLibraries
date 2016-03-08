using System.Net.Http;
using System.Threading.Tasks;

namespace Khaale.TechTalks.AwesomeLibs.IntegrationService.Cli.Commands
{
    public class SchedulerPauseCommand : ApiCommandBase
    {
        public SchedulerPauseCommand()
        {
            IsCommand("scheduler-pause", "Pauses all scheduled jobs.");
            SkipsCommandSummaryBeforeRunning();
        }

        protected override Task<HttpResponseMessage> ExecuteCommand(HttpClient client)
        {
            return client.PostAsync("scheduler/pause", null);
        }
    }
}