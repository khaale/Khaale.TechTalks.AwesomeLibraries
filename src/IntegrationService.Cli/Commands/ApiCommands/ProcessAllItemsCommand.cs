using System.Net.Http;
using System.Threading.Tasks;

namespace Khaale.TechTalks.AwesomeLibs.IntegrationService.Cli.Commands
{
    public class ProcessAllItemsCommand : ApiCommandBase
    {
        public ProcessAllItemsCommand()
        {
            this.IsCommand("process-all-items", "Runs processing for all items");

            this.SkipsCommandSummaryBeforeRunning();
        }

        protected override Task<HttpResponseMessage> ExecuteCommand(HttpClient client)
        {
            return client.PostAsync("process/all", null);
        }
    }
}