using AsyncApiSpecGenerator.Attributes;
using AsyncApiSpecGenerator.Models.AsyncApi;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Text.Json;
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
    
    internal static JsonSerializerOptions CreateJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };
    }

    internal static Dictionary<string, AsyncApiProperty> ToAsyncApiProperties(this Type type, AsyncApiComponents components)
    {
        return type
            .GetProperties()
            .ToDictionary(
                prop =>
                {
                    var jsonPropertyNameAttribute = prop.GetCustomAttributesData().FirstOrDefault(data => data.AttributeType.FullName == "System.Text.Json.Serialization.JsonPropertyNameAttribute");

                    if (jsonPropertyNameAttribute is not null)
                    {
                        return jsonPropertyNameAttribute.ConstructorArguments[0].Value!.ToString()!;
                    }

                    var newtonsoftNameAttribute = prop.GetCustomAttributesData().FirstOrDefault(data => data.AttributeType.FullName == "Newtonsoft.Json.JsonPropertyAttribute");

                    if (newtonsoftNameAttribute is not null)
                    {
                        return newtonsoftNameAttribute.ConstructorArguments[0].Value!.ToString()!;
                    }

                    return prop.Name.FirstCharToLower();
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
            var types = e.Message.Split('\n');
            var missingTypes = string.Join("\n\t", types
                .Select(t =>
                {
                    var split = t.Split('\'');
                    if (split.Length <= 1)
                        return "";

                    var typeName = split[1];
                    var splitTypeName = typeName.Split(',');
                    return string.Join(',', splitTypeName[..2]);
                })
                .ToHashSet());

            Console.WriteLine("Error loading types from assembly: " + assembly.GetName().Name + " skipping..." + missingTypes + "\n");
            return e.Types.Where(t => t != null).ToArray()!;
        }
    }

    internal static string FirstCharToUpper(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        };
    
    internal static string FirstCharToLower(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => string.Concat(input[0].ToString().ToLower(), input.AsSpan(1))
        };

    internal static bool InheritsOrImplements(this Type child, Type parent)
    {
        ArgumentNullException.ThrowIfNull(child);
        ArgumentNullException.ThrowIfNull(parent);

        var currentChild = child.IsGenericType ? child.GetGenericTypeDefinition() : child;

        while (currentChild != typeof(object))
        {
            if (parent == currentChild || HasAnyInterfaces(parent, currentChild))
                return true;

            currentChild = currentChild.BaseType is { IsGenericType: true }
                ? currentChild.BaseType.GetGenericTypeDefinition()
                : currentChild.BaseType;

            if (currentChild == null)
                return false;
        }
        return false;
    }

    private static bool HasAnyInterfaces(Type parent, Type child)
    {
        return child.GetInterfaces()
            .Any(childInterface =>
            {
                var currentInterface = childInterface.IsGenericType
                    ? childInterface.GetGenericTypeDefinition()
                    : childInterface;

                return currentInterface == parent;
            });
    }
}

internal class ProgramLoadContext(string path) : AssemblyLoadContext
{
    private static readonly Dictionary<string, Assembly?> _loadedAssemblies = new Dictionary<string, Assembly?>();
    private readonly AssemblyDependencyResolver _resolver = new AssemblyDependencyResolver(path);

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        // We don't want to load the shared assembly
        if (assemblyName.Name == "AsyncApiSpecGenerator.Attributes")
        {
            return null;
        }

        if (_loadedAssemblies.TryGetValue(assemblyName.FullName, out var load))
        {
            return load;
        }

        var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);

        var result = assemblyPath != null ? LoadFromAssemblyPath(assemblyPath) : null;

        _loadedAssemblies.Add(assemblyName.FullName, result);
        return result;
    }
}