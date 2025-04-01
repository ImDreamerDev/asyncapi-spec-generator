﻿using AsyncApiSpecGenerator.Attributes;
using System.Reflection;

namespace AsyncApiSpecGenerator;

public class Program
{
    public static void Main(string[] args)
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
            Console.WriteLine("--help: Show this help");
            return;
        }
        var argsList = args.ToList();
        var projectPath = GetArg(argsList, "--projectPath");
        var projectName = GetArg(argsList, "--projectName");
        var projectType = GetArg(argsList, "--projectType");
        var version = GetArg(argsList, "--version");

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
    }

    private static string? GetArg(List<string> args, string argName)
    {
        var index = args.IndexOf(argName);
        if (index == -1)
            return null;

        if (index + 1 > args.Count)
            return null;

        return args[index + 1];
    }

    private static string BuildDllPath(string path)
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

        // Try and build the project
        return BuildProject(workingPath);
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
        process.StartInfo.Arguments = "build " + workingPath;
        process.StartInfo.WorkingDirectory = projectPath;
        process.StartInfo.UseShellExecute = true;
        process.Start();

        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new Exception("Error building project: " + process.StandardError.ReadToEnd());
        }

        Console.WriteLine("Project built successfully");
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
}