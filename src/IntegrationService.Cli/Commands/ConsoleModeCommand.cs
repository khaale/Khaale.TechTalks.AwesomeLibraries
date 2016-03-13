using System.Collections.Generic;
using System.Linq;
using ManyConsole;

namespace Khaale.TechTalks.AwesomeLibs.IntegrationService.Cli.Commands
{
    public class ConsoleModeCommand : ManyConsole.ConsoleModeCommand
    {
        public override IEnumerable<ConsoleCommand> GetNextCommands()
        {
            return ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program)).Where(c => !(c is ConsoleModeCommand));
        }
    }
}
