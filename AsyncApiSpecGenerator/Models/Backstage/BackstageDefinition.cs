using System.Diagnostics.CodeAnalysis;
namespace AsyncApiSpecGenerator.Models.Backstage;

[SuppressMessage("Performance", "CA1822:Mark members as static")]
internal class BackstageDefinition(string bcName, string projectType, string version)
{
    public string ApiVersion => "backstage.io/v1alpha1";
    public string Kind => "API";
    public BackstageApiMetadata Metadata { get; } = new BackstageApiMetadata(bcName, projectType, version);
    public BackstageDefinitionData Spec { get; } = new BackstageDefinitionData(bcName, projectType, version);
}