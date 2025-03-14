namespace AsyncApiSpecGenerator.Models.Backstage;

internal class BackstageApiMetadata(string bcName, string projectType, string version)
{
    public string Name { get; } = projectType.ToLower() + "-" + bcName.ToLower() + "-" + "{ENVIRONMENT}" + "-asyncapi-" + version;
    public string Title { get; } = $"{bcName.FirstCharToUpper()} {projectType.ToUpper()} asyncapi ({version})";
    public string Description { get; } = "Automatically generated AsyncAPI specification for " + projectType.ToUpper() + " " + bcName.FirstCharToUpper();
    public string[] Tags { get; } = [projectType.ToLower(), bcName.ToLower(), "{ENVIRONMENT_LIFECYCLE}"];
}