// See https://aka.ms/new-console-template for more information

using static Ze.Cli.Bash.BashModule;

Console.WriteLine("test");
RunBashScript("echo 'Hello World'");
/*
using Bearz.Std;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Ze.Cli;
using Ze.Cli.Bash;

var r = Bash.Script(
"""
echo "test"
echo -e "\e[38;5;97mhello"
echo "\n"

""",
new CliStartInfo()
{
    StdOut = Stdio.Piped,
});

Console.WriteLine("piped");
foreach (var line in r.StdOut)
{
    Console.WriteLine(line);
}
*/

/*
var services = new ServiceCollection();
services.AddLogging();
var cb = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

services.AddSingleton<IConfiguration>(cb);

var sr = services.BuildServiceProvider();

var tasks = new TaskCollection();
tasks.AddTask("default", () =>
{
    Console.WriteLine("Hello task");
});

var runner = new WorkflowRunner(sr);
await runner.RunTaskAsync("default", tasks);
*/