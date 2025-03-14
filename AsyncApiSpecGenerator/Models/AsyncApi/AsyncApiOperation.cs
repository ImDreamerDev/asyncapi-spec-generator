namespace AsyncApiSpecGenerator.Models.AsyncApi;

internal class AsyncApiOperation(string action, AsyncApiSpecChannelRef channel, string summary, AsyncApiMessage[] messages)
{
    public string Action { get; } = action;
    public AsyncApiSpecChannelRef Channel { get; } = channel;
    public string Summary { get; } = summary;
    public AsyncApiMessage[] Messages { get; } = messages;

}