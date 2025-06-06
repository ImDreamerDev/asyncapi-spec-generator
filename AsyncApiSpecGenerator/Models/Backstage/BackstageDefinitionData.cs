using AsyncApiSpecGenerator.Models.AsyncApi;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
namespace AsyncApiSpecGenerator.Models.Backstage;

[SuppressMessage("Performance", "CA1822:Mark members as static")]
internal class BackstageDefinitionData(string bcName, string projectType, string version)
{
    public string Type => "asyncapi";
    public string Lifecycle => "{ENVIRONMENT_LIFECYCLE}";
    public string Owner => "{OWNER}";
    [YamlIgnore]
    [JsonIgnore]
    public AsyncApiDefinition Definition { get; } = new AsyncApiDefinition(bcName, projectType, version);
    // Backstage expects the definition to be a string
    [YamlMember(Alias = "Definition", ScalarStyle = ScalarStyle.Literal)]
    [JsonPropertyName("Definition")]
    public string BackstageDefinition => Extensions.CreateSerializer().Serialize(Definition);
}