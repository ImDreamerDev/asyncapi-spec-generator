using AsyncApiSpecGenerator.Attributes;
using AsyncApiSpecGenerator.Models.Backstage;
using System.Reflection;
namespace AsyncApiSpecGenerator;

internal static class AsyncEventGenerator
{
    internal static BackstageDefinition? GenerateEvents(string projectName, ICollection<Type> types, string projectType, string version)
    {
        var spec = new BackstageDefinition(projectName, projectType, version);
        var addedTypes = new HashSet<string>();

        foreach (var type in types)
        {
            var asyncEventAttribute = type.GetCustomAttribute<AsyncEventAttribute>();
            if (asyncEventAttribute is null)
            {
                continue;
            }

            if (addedTypes.Contains(type.FullName!))
                continue;

            spec.Spec.Definition.AddFromType(asyncEventAttribute, type);
            addedTypes.Add(type.FullName!);
            Console.WriteLine("Added '{0}' to AsyncApi spec", type.FullName);
        }

        Console.WriteLine("Found {0} types with AsyncEvent", addedTypes.Count);
        return addedTypes.Count == 0 ? null : spec;
    }
}