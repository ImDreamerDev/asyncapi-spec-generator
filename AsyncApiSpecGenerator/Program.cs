using AsyncApiSpecGenerator;
using AsyncApiSpecGenerator.Attributes;
using System.Reflection;

if (args.Length == 0)
{
    PrintUsage();
    return;
}

if (args is ["--help"])
{
    Console.WriteLine("Usage: AsyncApiGenerator --projectPath <path> --projectName <bcName> --projectType <projectType> --version <version>");
    Console.WriteLine("--projectPath: Path to the project ex: " + @".\src\Project.Api");
    Console.WriteLine("--projectName: The name of the business component");
    Console.WriteLine("--projectType: The type of project, e.g. BC, ADPT, etc.");
    Console.WriteLine("--version: The version of the project");
    Console.WriteLine("--help: Show this help");
    return;
}

var argsList = args.Select(s => s.ToLower()).ToList();
var projectPath = GetArg("--projectPath");
var projectName = GetArg("--projectName");
var projectType = GetArg("--projectType");
var version = GetArg("--version");

if (projectPath is null)
{
    Console.WriteLine("Missing projectPath");
    PrintUsage();
    return;
}

if (projectName is null || projectType is null || version is null)
{
    Console.WriteLine("Missing " + (projectName is null ? "projectName " : "") + (projectType is null ? "projectType " : "") + (version is null ? "version" : ""));
    PrintUsage();
    return;
}

var workingPath = BuildDllPath(projectPath);

var apiLoader = new ProgramLoadContext(workingPath);
var asm = apiLoader.LoadFromAssemblyPath(workingPath);
// Since the types are lazy loaded we need to load the assembly to get the types
asm.GetLoadableTypes();


var types = AppDomain.CurrentDomain
    .GetAssemblies()
    .SelectMany(a => a.GetLoadableTypes())
    .Where(type => type.GetCustomAttribute<AsyncEventAttribute>() != null)
    .ToArray();

var spec = AsyncEventGenerator.GenerateEvents(projectName, types, projectType, version);

if (spec is null)
{
    Console.WriteLine("No events found, exiting");
    return;
}

var serializer = Extensions.CreateSerializer();

var fileNameSpec = $"{projectType}-{projectName}-{version}-asyncapi.yaml".ToLower();
var fileNameBackstage = $"{projectType}-{projectName}-{version}-asyncapi-backstage.yaml".ToLower();

var asyncApiOnly = serializer.Serialize(spec.Spec.Definition);
var backstage = serializer.Serialize(spec);

File.WriteAllText(fileNameSpec, asyncApiOnly);
File.WriteAllText(fileNameBackstage, backstage);

Console.WriteLine($"Generated AsyncApi for {projectName} {projectType} {version} to {fileNameSpec}");
Console.WriteLine($"Generated Backstage for {projectName} {projectType} {version} to {fileNameBackstage}");
return;


string? GetArg(string argName)
{
    var index = argsList.IndexOf(argName.ToLower());
    return index == -1 ? null : args[index + 1];
}

string BuildDllPath(string path)
{
    var workingPath = Path.GetFullPath(path);

    var fileName = Path.GetFileName(workingPath);

    var releaseDllPath = Path.Combine(workingPath, "bin", "Release", "net8.0", fileName + ".dll");
    var debugDllPath = Path.Combine(workingPath, "bin", "Debug", "net8.0", fileName + ".dll");

    if (File.Exists(releaseDllPath))
    {
        return releaseDllPath;
    }

    if (File.Exists(debugDllPath))
    {
        return debugDllPath;
    }

    throw new FileNotFoundException("Unable to find release or debug DLL, make sure that you have built the project. Path was for release: " + releaseDllPath + " and for debug: " + debugDllPath);
}

void PrintUsage()
{
    Console.WriteLine("Usage: AsyncApiGenerator --projectPath <path> --projectName <projectName> --projectType <projectType> --version <version>");
    Console.WriteLine("Write --help for more information");
}