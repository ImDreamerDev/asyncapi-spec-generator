using Shouldly;
using Xunit;
namespace AsyncApiSpecGenerator.Tests;

public class AsyncEventGenerator_Should
{
    [Fact]
    public void Generate()
    {
        // Arrange
        const string projectName = "Test";
        const string projectType = "BC";
        var types = typeof(AsyncEventGenerator_Should).Assembly.GetTypes();
        const string version = "1.0";

        // Act
        var result = AsyncEventGenerator.GenerateEvents(projectName, types, projectType, version);

        // Assert
        result.ShouldNotBeNull();
        result.Kind.ShouldBe("API");
        result.ApiVersion.ShouldBe("backstage.io/v1alpha1");

        result.Metadata.Name.ShouldBe(projectType.ToLower() + "-" + projectName.ToLower() + "-{ENVIRONMENT}-asyncapi-" + version);
        result.Metadata.Title.ShouldBe(projectName.FirstCharToUpper() + " " + projectType.ToUpper() + " asyncapi (" + version + ")");
        result.Metadata.Description.ShouldBe("Automatically generated AsyncAPI specification for " + projectType.ToUpper() + " " + projectName.FirstCharToUpper());
        result.Metadata.Tags.ShouldBe([projectType.ToLower(), projectName.ToLower(), "{ENVIRONMENT_LIFECYCLE}"]);

        result.Spec.Type.ShouldBe("asyncapi");
        result.Spec.Lifecycle.ShouldBe("{ENVIRONMENT_LIFECYCLE}");
        result.Spec.Owner.ShouldBe("{OWNER}");

        result.Spec.Definition.AsyncApi.ShouldBe("3.0.0");
        result.Spec.Definition.Info.Title.ShouldBe(projectName.FirstCharToUpper() +" " + projectType.ToUpper() + " AsyncApi");
        result.Spec.Definition.Info.Version.ShouldBe(version);
        result.Spec.Definition.Info.Description.ShouldBe("Automatically generated AsyncAPI specification for " + projectName);

        result.Spec.Definition.Servers.Count.ShouldBe(2);
        result.Spec.Definition.Servers["ServiceBus"].Host.ShouldBe("bc-{ENVIRONMENT}-sbu.servicebus.windows.net");
        result.Spec.Definition.Servers["ServiceBus"].Protocol.ShouldBe("AMQP 1.0");
        result.Spec.Definition.Servers["ServiceBus"].Description.ShouldBe("Azure Service Bus for {ENVIRONMENT_LIFECYCLE} environment");
        result.Spec.Definition.Servers["CPQ"].Host.ShouldBe("https://cpq.com");
        result.Spec.Definition.Servers["CPQ"].Protocol.ShouldBe("AMQP 1.0");
        result.Spec.Definition.Servers["CPQ"].Description.ShouldBe("Azure Service Bus for EventDemoWithServer");

        result.Spec.Definition.Channels.Count.ShouldBe(3);
        result.Spec.Definition.Channels["cpq-events"].Address.ShouldBe("cpq-events");
        result.Spec.Definition.Channels["cpq-events"].Description.ShouldBeEmpty();
        result.Spec.Definition.Channels["cpq-events"].Messages.Count.ShouldBe(1);
        result.Spec.Definition.Channels["cpq-events"].Messages["EventDemo"].Ref.ShouldBe("#/components/messages/EventDemo");

        result.Spec.Definition.Channels["cpq-events-updates"].Address.ShouldBe("cpq-events-updates");
        result.Spec.Definition.Channels["cpq-events-updates"].Description.ShouldBeEmpty();
        result.Spec.Definition.Channels["cpq-events-updates"].Messages.Count.ShouldBe(1);
        result.Spec.Definition.Channels["cpq-events-updates"].Messages["EventDemo2"].Ref.ShouldBe("#/components/messages/EventDemo2");

        result.Spec.Definition.Channels["number-events"].Address.ShouldBe("number-events");
        result.Spec.Definition.Channels["number-events"].Description.ShouldBeEmpty();
        result.Spec.Definition.Channels["number-events"].Messages.Count.ShouldBe(1);
        result.Spec.Definition.Channels["number-events"].Messages["EventDemoWithServer"].Ref.ShouldBe("#/components/messages/EventDemoWithServer");

        result.Spec.Definition.Operations.Count.ShouldBe(3);
        result.Spec.Definition.Operations["cpq-events"].Action.ShouldBe("send");
        result.Spec.Definition.Operations["cpq-events"].Channel.Ref.ShouldBe("#/channels/cpq-events");
        result.Spec.Definition.Operations["cpq-events"].Messages[0].Ref.ShouldBe("#/channels/cpq-events/messages/EventDemo");
        result.Spec.Definition.Operations["cpq-events"].Summary.ShouldBe("Sends messages to cpq-events on the host bc-{ENVIRONMENT}-sbu.servicebus.windows.net");

        result.Spec.Definition.Operations["cpq-events-updates"].Action.ShouldBe("receive");
        result.Spec.Definition.Operations["cpq-events-updates"].Channel.Ref.ShouldBe("#/channels/cpq-events-updates");
        result.Spec.Definition.Operations["cpq-events-updates"].Messages[0].Ref.ShouldBe("#/channels/cpq-events-updates/messages/EventDemo2");
        result.Spec.Definition.Operations["cpq-events-updates"].Summary.ShouldBe("Receives messages from cpq-events-updates on the host bc-{ENVIRONMENT}-sbu.servicebus.windows.net");

        result.Spec.Definition.Operations["number-events"].Action.ShouldBe("send");
        result.Spec.Definition.Operations["number-events"].Channel.Ref.ShouldBe("#/channels/number-events");
        result.Spec.Definition.Operations["number-events"].Messages[0].Ref.ShouldBe("#/channels/number-events/messages/EventDemoWithServer");
        result.Spec.Definition.Operations["number-events"].Summary.ShouldBe("Sends messages to number-events on the host https://cpq.com");


        result.Spec.Definition.Components.Schemas.Count.ShouldBe(7);
    }
}