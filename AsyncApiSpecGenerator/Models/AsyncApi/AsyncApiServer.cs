namespace AsyncApiSpecGenerator.Models.AsyncApi;

internal class AsyncApiServer(string host, string protocol, string description)
{
    public string Host { get; } = host;
    public string Protocol { get; } = protocol;
    public string Description { get; } = description;
}