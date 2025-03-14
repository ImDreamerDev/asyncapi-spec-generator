using AsyncApiSpecGenerator.Attributes;
using AsyncApiSpecGenerator.Models.AsyncApi;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
namespace AsyncApiSpecGenerator;

internal static class Extensions
{
    private static bool IsNullable(this PropertyInfo propertyInfo)
    {
        var nullableAttribute = propertyInfo.GetCustomAttribute<NullableAttribute>();
        // To ensure that we don't see string?[] as nullable we have to inspect the flags
        if (propertyInfo.PropertyType.IsArray)
        {
            return nullableAttribute is not null && propertyInfo.PropertyType.IsArray && nullableAttribute.NullableFlags.Length > 1 && nullableAttribute.NullableFlags[1] != 2;
        }

        return nullableAttribute is not null;
    }

    internal static ISerializer CreateSerializer()
    {
        return new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
            .WithQuotingNecessaryStrings()
            .WithNewLine("\n")
            .Build();
    }

    internal static Dictionary<string, AsyncApiProperty> ToAsyncApiProperties(this Type type, AsyncApiComponents components)
    {
        return type
            .GetProperties()
            .ToDictionary(
                prop =>
                {
                    var nameAttribute = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
                    var newtonsoftNameAttribute = prop.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>();

                    if (nameAttribute is not null)
                    {
                        return nameAttribute.Name;
                    }

                    if (newtonsoftNameAttribute?.PropertyName != null)
                    {
                        return newtonsoftNameAttribute.PropertyName;
                    }


                    return prop.Name;
                },
                prop => AsyncEventTypeHandler.ToAsyncApiSpecType(prop.PropertyType, components, prop.IsNullable(), prop.GetCustomAttribute<AsyncApiDescriptionAttribute>())
            );
    }

    internal static Type[] GetLoadableTypes(this Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            // We only want to log the error if it is one of our assemblies
            Console.WriteLine("Error loading types from assembly: " + assembly.GetName().Name + " skipping..." + "\n" + e.Message + "\n");
            return e.Types.Where(t => t != null).ToArray()!;
        }
    }

    public static string FirstCharToUpper(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        };
}

internal class ProgramLoadContext(string path) : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver = new AssemblyDependencyResolver(path);

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        // We don't want to load the shared assembly
        if (assemblyName.Name == "AsyncApiSpecGenerator.Attributes")
        {
            return null;
        }

        var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);

        return assemblyPath != null ? LoadFromAssemblyPath(assemblyPath) : null;
    }
}