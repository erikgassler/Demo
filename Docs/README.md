# Erik Gassler Demo

Demo website used for showcasing concepts.

## Concepts on display

|Concept|Details|
|--|--|
[Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)|This website project uses Microsoft's Blazor framework to allow UI work to be engineered using C# and .NET language and libraries. This code is then compiled into WebAssembly that will be run by the browser when loading the site.
.NET 6.0|This solution utilizes .NET 6.0 runtimes for all of its projects.
Test-Driven Development|This solution is prioritized to be developed using test-driven development with a focus on unit testing.
Ready-To-Run, No dev files|This solution excludes the use of any files that require custom configurations from a developer after pulling the project to their local. All config data is expected to be loaded from environment variables. App is expected to be runnable immediataly after a fresh clone without any further setup needed - though some features may be disabled if configuration is missing.
[Task Proxy](https://www.taskproxy.com/home) | This website project utilizes Task Proxy for extended developer documentation, scripts, and tools.

## Crypto Report Demo

### Navigate to Crypto page

![Crypto Report Page - First View](https://raw.github.com/erikgassler/Demo/master/Docs/Crypto-Start.png)

### Click Start Ingestion

![Crypto Report Page - First View](https://raw.github.com/erikgassler/Demo/master/Docs/Crypto-Run-Ingestion.png)

### Click Load Latest Data - Repeat when finished to load next batch

![Crypto Report Page - First View](https://raw.github.com/erikgassler/Demo/master/Docs/Crypto-Run-Report.png)

## Local Setup

|Step|Details|
|--|--|
Environment Variables|Add environment variables to your local machine. See PowerShell script `Docs/Update Env Variables.ps1`.
IIS Setup|Setup IIS server if desired to run on local domain instead of Visual Studio debugger. See PowerShell script `Docs/Demo IIS Setup.ps1`.
IIS Mime Types|You may need to add some additional MIME types to your IIS app deployment. Add MIME types `.blat` and `.dat` with value `application/octet-stream`.
Publish|Setup and run a publish profile in Visual Studio. Assure Target Location is directed to your desired deployment folder.
SQL Database|Install SQL Server on your local machine and run scripts in SqlScripts folder using Sql Server Management Studio to setup your database.

## Author

**[Erik Gassler](https://www.erikgassler.com/home)** - Just a simpleton who likes making stuff with bits and bytes. Visit [my Patreon page](https://www.patreon.com/stoicdreams) if you would like to provide support.

## License

This project is licensed under the The [MIT License (MIT)](https://github.com/erikgassler/Demo/blob/master/Docs/LICENSE.txt)
