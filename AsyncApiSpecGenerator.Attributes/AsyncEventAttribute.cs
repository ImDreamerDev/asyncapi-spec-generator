namespace AsyncApiSpecGenerator.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class AsyncEventAttribute(string targetQueueOrTopic, AsyncApiOperationType operationType) : Attribute
{
    public AsyncApiOperationType OperationType { get; } = operationType;
    public string TargetQueueOrTopic { get; } = targetQueueOrTopic;
    public string? SubscriptionName { get; set; }
    public string? Description { get; set; }
    public string? ServerName { get; set; }
    public string? ServerUrl { get; set; }
}