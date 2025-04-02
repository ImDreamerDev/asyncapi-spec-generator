using AsyncApiSpecGenerator.Attributes;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
namespace AsyncApiSpecGenerator.Models.AsyncApi;

[SuppressMessage("Performance", "CA1822:Mark members as static")]
internal class AsyncApiDefinition
{
    [YamlMember(Alias = "asyncapi")]
    [JsonPropertyName("asyncapi")]
    public string AsyncApi => "3.0.0";
    public AsyncApiInfo Info { get; }
    public Dictionary<string, AsyncApiServer> Servers { get; } = new Dictionary<string, AsyncApiServer>();
    public Dictionary<string, AsyncApiChannel> Channels { get; } = new Dictionary<string, AsyncApiChannel>();
    public Dictionary<string, AsyncApiOperation> Operations { get; } = new Dictionary<string, AsyncApiOperation>();
    public AsyncApiComponents Components { get; } = new AsyncApiComponents();

    public AsyncApiDefinition(string bcName, string projectType, string version)
    {
        Info = new AsyncApiInfo(bcName, projectType, version);
        Servers.Add("ServiceBus",
            new AsyncApiServer(
                "bc-{ENVIRONMENT}-sbu.servicebus.windows.net",
                "AMQP 1.0",
                "Azure Service Bus for {ENVIRONMENT_LIFECYCLE} environment"
            )
        );
    }

    public void AddFromType(AsyncEventAttribute attribute, Type asyncEvent)
    {
        var props = asyncEvent.ToAsyncApiProperties(Components);
        var schma = new AsyncApiSchema(["object"])
        {
            Properties = props
        };

        if (string.IsNullOrEmpty(attribute.ServerName) is false && string.IsNullOrEmpty(attribute.ServerUrl) is false && Servers.ContainsKey(attribute.ServerName) is false)
        {
            Servers.Add(
                attribute.ServerName,
                new AsyncApiServer(
                    attribute.ServerUrl,
                    "AMQP 1.0",
                    "Azure Service Bus for " + asyncEvent.Name
                )
            );

        }

        Channels.Add(
            attribute.TargetQueueOrTopic,
            new AsyncApiChannel(attribute.TargetQueueOrTopic, attribute.Description ?? "",
                new Dictionary<string, AsyncApiMessage>
                {
                    {
                        asyncEvent.Name,
                        new AsyncApiMessage("#/components/messages/" + asyncEvent.Name)
                    }
                }
            ));

        CreateOperations(attribute, asyncEvent);

        Components.Messages.Add(
            asyncEvent.Name,
            new AsyncApiComponentMessage(
                new AsyncApiPayload("#/components/schemas/" + asyncEvent.Name)
            )
        );

        Components.Schemas.Add(
            asyncEvent.Name,
            schma
        );
    }

    /// <summary>
    /// Generates the operations for the AsyncApiDefinition.
    /// Depending on the OperationType specified in the attribute, it will create one or two operations.
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="asyncEvent"></param>
    /// <exception cref="NotSupportedException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private void CreateOperations(AsyncEventAttribute attribute, Type asyncEvent)
    {
        var serverHost = string.IsNullOrEmpty(attribute.ServerName) is false && Servers.TryGetValue(attribute.ServerName, out var server) ? server.Host : Servers.First().Value.Host;

        switch (attribute.OperationType)
        {
            case AsyncApiOperationType.None:
                throw new NotSupportedException("No operation type specified");
            case AsyncApiOperationType.Send:
                Operations.Add(
                    attribute.TargetQueueOrTopic,
                    new AsyncApiOperation("send",
                        new AsyncApiSpecChannelRef("#/channels/" + attribute.TargetQueueOrTopic),
                        "Sends messages to " + attribute.TargetQueueOrTopic + " on the host " + serverHost + (string.IsNullOrEmpty(attribute.SubscriptionName) ? "" : " with subscription " + attribute.SubscriptionName),
                        [
                            new AsyncApiMessage("#/channels/" + attribute.TargetQueueOrTopic + "/messages/" + asyncEvent.Name
                            )
                        ]
                    )
                );
                break;
            case AsyncApiOperationType.Receive:
                Operations.Add(
                    attribute.TargetQueueOrTopic,
                    new AsyncApiOperation("receive",
                        new AsyncApiSpecChannelRef("#/channels/" + attribute.TargetQueueOrTopic),
                        "Receives messages from " + attribute.TargetQueueOrTopic + " on the host " + serverHost + (string.IsNullOrEmpty(attribute.SubscriptionName) ? "" : " with subscription " + attribute.SubscriptionName),
                        [
                            new AsyncApiMessage("#/channels/" + attribute.TargetQueueOrTopic + "/messages/" + asyncEvent.Name
                            )
                        ]
                    )
                );
                break;
            case AsyncApiOperationType.Both:
                Operations.Add(
                    attribute.TargetQueueOrTopic + "-send",
                    new AsyncApiOperation("send",
                        new AsyncApiSpecChannelRef("#/channels/" + attribute.TargetQueueOrTopic),
                        "Sends and receives messages to " + attribute.TargetQueueOrTopic + " on the host " + serverHost + (string.IsNullOrEmpty(attribute.SubscriptionName) ? "" : " with subscription " + attribute.SubscriptionName),
                        [
                            new AsyncApiMessage("#/channels/" + attribute.TargetQueueOrTopic + "/messages/" + asyncEvent.Name
                            )
                        ]
                    )
                );
                Operations.Add(
                    attribute.TargetQueueOrTopic + "-receive",
                    new AsyncApiOperation("receive",
                        new AsyncApiSpecChannelRef("#/channels/" + attribute.TargetQueueOrTopic),
                        "Receives messages from " + attribute.TargetQueueOrTopic + " on the host " + serverHost + (string.IsNullOrEmpty(attribute.SubscriptionName) ? "" : " with subscription " + attribute.SubscriptionName),
                        [
                            new AsyncApiMessage("#/channels/" + attribute.TargetQueueOrTopic + "/messages/" + asyncEvent.Name
                            )
                        ]
                    )
                );
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(attribute));
        }
    }
}