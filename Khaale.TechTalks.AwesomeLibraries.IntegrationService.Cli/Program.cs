using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManyConsole;
using ConsoleModeCommand = Khaale.TechTalks.AwesomeLibraries.IntegrationService.Cli.Commands.ConsoleModeCommand;

namespace Khaale.TechTalks.AwesomeLibraries.IntegrationService.Cli
{
    class Program
    {
        static int Main(string[] args)
        {
            var consoleModeCommand= new ConsoleModeCommand();

            return ConsoleCommandDispatcher.DispatchCommand(consoleModeCommand, args, Console.Out);
        }
    }
}
