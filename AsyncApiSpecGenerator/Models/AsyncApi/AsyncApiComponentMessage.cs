namespace AsyncApiSpecGenerator.Models.AsyncApi;

internal class AsyncApiComponentMessage(AsyncApiPayload payload)
{
    public AsyncApiPayload Payload { get; } = payload;
}