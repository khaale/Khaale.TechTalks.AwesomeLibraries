using System;
using ManyConsole;
using ConsoleModeCommand = Khaale.TechTalks.AwesomeLibs.IntegrationService.Cli.Commands.ConsoleModeCommand;

namespace Khaale.TechTalks.AwesomeLibs.IntegrationService.Cli
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
