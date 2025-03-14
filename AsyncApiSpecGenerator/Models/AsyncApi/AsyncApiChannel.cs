namespace AsyncApiSpecGenerator.Models.AsyncApi;

internal class AsyncApiChannel(string address, string description, Dictionary<string, AsyncApiMessage> messages)
{
    public string Address { get; } = address;
    public string Description { get; } = description;
    public Dictionary<string, AsyncApiMessage> Messages { get; } = messages;
}