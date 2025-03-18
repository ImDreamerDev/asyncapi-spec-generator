using AsyncApiSpecGenerator.Models.AsyncApi;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
namespace AsyncApiSpecGenerator.Models.Backstage;

internal class BackstageDefinitionData(string bcName, string projectType, string version)
{
    public string Type => "asyncapi";
    public string Lifecycle => "{ENVIRONMENT_LIFECYCLE}";
    public string Owner => "{OWNER}";
    [YamlIgnore]
    public AsyncApiDefinition Definition { get; } = new AsyncApiDefinition(bcName, projectType, version);
    // Backstage expects the definition to be a string
    [YamlMember(Alias = "Definition", ScalarStyle = ScalarStyle.Literal)]
    public string BackstageDefinition => Extensions.CreateSerializer().Serialize(Definition);
}