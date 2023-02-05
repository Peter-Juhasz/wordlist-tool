using System.CommandLine;
using WordlistTool.Cli;

var root = new RootCommand();
root.AddCommands();
await root.InvokeAsync(args);
