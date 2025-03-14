namespace AsyncApiSpecGenerator.Models.AsyncApi;

internal class AsyncApiInfo(string bcName, string projectType, string version)
{
    public string Title { get; } = $"{bcName.FirstCharToUpper()} {projectType.ToUpper()} AsyncApi";
    public string Version { get; } = version;
    public string Description { get; } = "Automatically generated AsyncAPI specification for " + bcName;
}