// See https://aka.ms/new-console-template for more information

using System.CommandLine.Parsing;

using Bearz.Extensions.Hosting.CommandLine;

using Casa;

var builder = new ConsoleApplicationBuilder();
Startup.Configure(builder);

var app = builder.Build();
await app.InvokeAsync(args);