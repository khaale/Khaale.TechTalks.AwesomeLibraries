using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManyConsole;

namespace Khaale.TechTalks.AwesomeLibraries.IntegrationService.Cli.Commands
{
    public class ConsoleModeCommand : ManyConsole.ConsoleModeCommand
    {
        public override IEnumerable<ConsoleCommand> GetNextCommands()
        {
            return ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program)).Where(c => !(c is ConsoleModeCommand));
        }
    }
}
