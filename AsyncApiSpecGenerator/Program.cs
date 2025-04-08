using AsyncApiSpecGenerator.Attributes;
using DotNet.Testcontainers.Builders;
using Microsoft.Extensions.Logging.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace AsyncApiSpecGenerator;

public class Program
{
    private static readonly HashSet<string> _checkedDlls = [];

    public static async Task Main(string[] args)
    {
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
            Console.WriteLine("--json: Output the AsyncApi spec in JSON format (default is YAML)");
            Console.WriteLine("--env: The environment to use, e.g. dev, test, prod (optional)");
            Console.WriteLine("--lifecycle: The lifecycle of the project, e.g. dev, test, prod (optional)");
            Console.WriteLine("--owner: The owner of the project, e.g. team name (optional)");
            Console.WriteLine("--noValidate: Skip validation");
            Console.WriteLine("--help: Show this help");
            return;
        }
        var argsList = args.Select(input => input.StartsWith("--") ? input.ToLower() : input).ToList();
        var projectPaths = GetProjects(argsList);
        var projectName = GetArg(argsList, "--projectName");
        var projectType = GetArg(argsList, "--projectType");
        var version = GetArg(argsList, "--version");

        var env = GetArg(argsList, "--env");
        var lifecycle = GetArg(argsList, "--lifecycle");
        var owner = GetArg(argsList, "--owner");

        if (projectPaths is [])
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

        List<Type> types = [];

        foreach (var projectPath in projectPaths)
        {
            if (string.IsNullOrEmpty(projectPath) || Directory.Exists(projectPath) is false)
            {
                Console.WriteLine("Invalid project path: " + projectPath);
                continue;
            }

            Console.WriteLine("Starting to process path: " + Path.GetFileName(projectPath));
            types.AddRange(LoadTypes(projectPath, projectName));
            Console.WriteLine("Finished processing " + Path.GetFileName(projectPath));
        }

        var spec = AsyncEventGenerator.GenerateEvents(projectName, types.ToArray(), projectType, version);

        if (spec is null)
        {
            Console.WriteLine("No events found, exiting");
            return;
        }
        var isJson = HasFlag(argsList, "--json");

        var fileNameSpec = ($"{projectType}-{projectName}-{version}-asyncapi" + (isJson ? ".json" : ".yaml")).ToLower();
        var fileNameBackstage = ($"{projectType}-{projectName}-{version}-asyncapi-backstage" + (isJson ? ".json" : ".yaml")).ToLower();

        string asyncApiOnly;
        string backstage;

        if (HasFlag(argsList, "--json"))
        {
            var options = Extensions.CreateJsonSerializerOptions();
            asyncApiOnly = JsonSerializer.Serialize(spec.Spec.Definition, options);
            if (HasFlag(argsList, "--noValidate"))
                await Validate(projectName, asyncApiOnly);
            else
                Console.WriteLine("Skipping validation");
            backstage = JsonSerializer.Serialize(spec, options);
        }
        else
        {
            var serializer = Extensions.CreateSerializer();
            asyncApiOnly = JsonSerializer.Serialize(spec.Spec.Definition);
            if (HasFlag(argsList, "--noValidate"))
                await Validate(projectName, asyncApiOnly);
            else
                Console.WriteLine("Skipping validation");
            backstage = serializer.Serialize(spec);
        }

        if (string.IsNullOrEmpty(env) is false)
        {
            asyncApiOnly = asyncApiOnly.Replace("{ENVIRONMENT}", env);
        }

        if (string.IsNullOrEmpty(lifecycle) is false)
        {
            asyncApiOnly = asyncApiOnly.Replace("{ENVIRONMENT_LIFECYCLE}", lifecycle);
        }

        if (string.IsNullOrEmpty(owner) is false)
        {
            asyncApiOnly = asyncApiOnly.Replace("{OWNER}", owner);
        }

        File.WriteAllText(fileNameSpec, asyncApiOnly);
        File.WriteAllText(fileNameBackstage, backstage);

        Console.WriteLine($"Generated AsyncApi for {projectName} {projectType} {version} to {Path.GetFullPath(fileNameSpec)}");
        Console.WriteLine($"Generated Backstage for {projectName} {projectType} {version} to {fileNameBackstage}");
    }

    private static string[] GetProjects(List<string> args)
    {
        return args
            .Select((arg, index) => new { arg, index })
            .Where(x => x.arg.Equals("--projectPath", StringComparison.CurrentCultureIgnoreCase))
            .Select(x => args[x.index + 1])
            .ToArray();
    }

    private static string? GetArg(List<string> args, string argName)
    {
        var index = args.IndexOf(argName.ToLower());
        if (index == -1)
            return null;

        if (index + 1 > args.Count)
            return null;

        return args[index + 1];
    }

    private static bool HasFlag(List<string> args, string flag)
    {
        return args.Contains(flag.ToLower());
    }

    private static string? BuildDllPath(string path)
    {
        var workingPath = Path.GetFullPath(path);

        var fileName = Path.GetFileName(workingPath);

        var releasePath = Path.Combine(workingPath, "bin", "Release");
        if (Path.Exists(releasePath))
        {
            var newestReleasePath = Directory.GetDirectories(releasePath)
                .Select(d => new DirectoryInfo(d))
                .OrderByDescending(d => d.LastWriteTime)
                .FirstOrDefault();
            var releaseDllPath = Path.Combine(newestReleasePath?.FullName ?? "", fileName + ".dll");

            if (File.Exists(releaseDllPath))
            {
                Console.WriteLine("Found release DLL: " + releaseDllPath);
                return releaseDllPath;
            }

            Console.WriteLine("Failed to find release DLL. Used: " + releaseDllPath);
        }

        var debugPath = Path.Combine(workingPath, "bin", "Debug");
        if (Path.Exists(debugPath))
        {
            var newestDebugPath = Directory.GetDirectories(debugPath)
                .Select(d => new DirectoryInfo(d))
                .OrderByDescending(d => d.LastWriteTime)
                .FirstOrDefault();
            var debugDllPath = Path.Combine(newestDebugPath?.FullName ?? "", fileName + ".dll");

            if (File.Exists(debugDllPath))
            {
                Console.WriteLine("Found debug DLL: " + debugDllPath);
                return debugDllPath;
            }

            Console.WriteLine("Failed to find debug DLL. Used: " + debugDllPath);
        }

        Console.WriteLine("Failed to find release path. Used: " + releasePath);
        Console.WriteLine("Failed to find debug path. Used: " + debugPath);
        return null;
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Usage: AsyncApiGenerator --projectPath <path> --projectName <projectName> --projectType <projectType> --version <version>");
        Console.WriteLine("Write --help for more information");
    }

    private static string BuildProject(string path)
    {
        var workingPath = Path.GetFullPath(path);
        Console.WriteLine("Project not built, trying to build it");
        // Try and build the project
        var projectPath = Path.GetDirectoryName(workingPath);
        //use dotnet build to build the project
        var process = new System.Diagnostics.Process();
        process.StartInfo.FileName = "dotnet";
        process.StartInfo.Arguments = "build --interactive --nologo --verbosity q " + workingPath;
        process.StartInfo.WorkingDirectory = projectPath;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.CreateNoWindow = true;
        process.OutputDataReceived += (_, args) => Console.WriteLine("\t" + args.Data);
        process.ErrorDataReceived += (_, args) => Console.Error.WriteLine("\t" + args.Data);

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new Exception("Error building project: " + process.StandardError.ReadToEnd());
        }

        Console.WriteLine("Project built successfully");

        var buildPath = BuildDllPath(path);

        if (buildPath != null)
        {
            return buildPath;
        }

        throw new FileNotFoundException("Unable to find release or debug DLL, make sure that you have built the project.");
    }

    private static List<Type> LoadTypes(string projectPath, string projectName)
    {
        var asyncEvents = new List<Type>();
        var workingPath = BuildDllPath(projectPath) ?? BuildProject(projectPath);

        var path = Path.GetDirectoryName(workingPath)!;

        foreach (var dll in Directory.GetFiles(path, "*.dll"))
        {
            var fileName = Path.GetFileName(dll);

            if (_checkedDlls.Add(fileName) is false)
                continue;

            if (fileName.Contains(projectName, StringComparison.InvariantCultureIgnoreCase) is false)
                continue;

            var loader = new ProgramLoadContext(dll);
            var asm = loader.LoadFromAssemblyPath(dll);
            Console.WriteLine("Loading assembly: " + fileName);
            // Since the types are lazy loaded we need to load the assembly to get the types
            var types = asm.GetLoadableTypes().Where(type => type.GetCustomAttribute<AsyncEventAttribute>() != null).ToList();
            asyncEvents.AddRange(types);
        }

        return asyncEvents;
    }

    private static async Task Validate(string projectName, string spec)
    {
        var bytes = Encoding.Default.GetBytes(spec);

        var container = new ContainerBuilder()
            .WithImage("asyncapi/cli:latest")
            .WithResourceMapping(bytes, "/config/" + projectName)
            .WithCommand("validate", "/config/" + projectName)
            .WithLogger(NullLogger.Instance)
            .Build();

        await container.StartAsync();

        var exitCode = await container.GetExitCodeAsync();
        var outLog = await container.GetLogsAsync(timestampsEnabled: false);
        if (exitCode == 1)
        {
            var errors = outLog.Stdout.Split("Errors")[^1];
            throw new ValidationException("Validation failed: " + errors);
        }

        if (exitCode != 0)
        {
            throw new Exception("Validation failed: " + outLog);
        }

        if (outLog.Stdout.Contains($"File /config/{projectName} is valid!") is false)
            throw new Exception("Failed to validate: " + outLog);

        Console.WriteLine("Validation succeeded");
    }
}