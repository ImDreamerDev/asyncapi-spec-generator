namespace AsyncApiSpecGenerator.Models.AsyncApi;

internal class AsyncApiComponents
{
    public Dictionary<string, AsyncApiComponentMessage> Messages { get; } = new Dictionary<string, AsyncApiComponentMessage>();
    public Dictionary<string, AsyncApiSchema> Schemas { get; } = new Dictionary<string, AsyncApiSchema>();
}