using System.Net.Http;
using System.Threading.Tasks;

namespace Khaale.TechTalks.AwesomeLibs.IntegrationService.Cli.Commands
{
    public class ProcessItemCommand : ApiCommandBase
    {
        private int _itemId;

        public ProcessItemCommand()
        {
            this.IsCommand("process-item", "Runs single item processing");
            
            this.HasRequiredOption("id=", "Item id", (int a) => { _itemId = a; });

            this.SkipsCommandSummaryBeforeRunning();
        }

        protected override Task<HttpResponseMessage> ExecuteCommand(HttpClient client)
        {
            return client.PostAsync("process/" + _itemId, null);
        }
    }
}
