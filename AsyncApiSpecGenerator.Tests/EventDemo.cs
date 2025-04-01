using AsyncApiSpecGenerator.Attributes;
using AsyncApiSpecGenerator.Models.AsyncApi;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
namespace AsyncApiSpecGenerator.Tests;

[AsyncEvent("cpq-events", AsyncApiOperationType.Send)]
internal class EventDemo
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly Date { get; set; }
    public int Number { get; set; }
}

[AsyncEvent("cpq-events-updates", AsyncApiOperationType.Receive)]
internal class EventDemo2
{
    [AsyncApiDescription(Description = "The identifier of the event.", Example = "14c4700e-a6de-4e63-bcbb-cdb5185d264a")]
    public Guid Id { get; set; }
    public Guid? NullableId { get; set; }
    [AsyncApiDescription(Description = "The name of the event.", MinLength = 10, MaxLength = 100)]
    public string Name { get; set; } = null!;
    [AsyncApiDescription(Description = "A nullable name of the event.", Examples = ["John", "Doe"], MinLength = 10, MaxLength = 100)]
    public string? NullableName { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime? NullableDateTime { get; set; }
    public DateTimeOffset DateTimeOffset { get; set; }
    public DateTimeOffset? NullableDateTimeOffset { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly? NullableDate { get; set; }
    public TimeOnly Time { get; set; }
    public TimeOnly? NullableTime { get; set; }
    public int Number { get; set; }
    public int? NullableNumber { get; set; }
    public EventDemo Complex { get; set; } = null!;
    public EventDemo? NullableComplex { get; set; }
    public List<int> Numbers { get; set; } = null!;
    public List<int>? NullableNumbersList { get; set; }
    public List<int?> NumbersNullableList { get; set; } = null!;
    public string[] Array { get; set; } = null!;
    public string[]? NullableArray { get; set; }
    public string?[] StringNullableArray { get; set; } = null!;
    public Todo Todo { get; set; } = null!;
    public AsyncApiFormat Enum { get; set; }
    public AsyncApiFormat? NullableEnum { get; set; }
    public TestStruct Struct { get; set; }
    public TestStruct? NullableStruct { get; set; }
}

public class Todo
{
    [JsonProperty("demo_title")]
    public string Title { get; set; } = null!;
    [JsonPropertyName("demo_description")]
    public string? Description { get; set; }
    [JsonProperty("newton_done")][JsonPropertyName("text_done")]
    public bool Done { get; set; }
}

public struct TestStruct
{
    public string Name { get; set; }
    public int Number { get; set; }
}

[AsyncEvent("number-events", AsyncApiOperationType.Send, ServerName = "CPQ", ServerUrl = "https://cpq.com")]
internal class EventDemoWithServer
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly Date { get; set; }
    public int Number { get; set; }
}

internal class MultipleClasses
{
    public EventDemo EventDemo { get; set; } = null!;
    public EventDemo EventDemo2 { get; set; } = null!;
}

internal class MultipleStructs
{
    public TestStruct TestStruct { get; set; }
    public TestStruct TestStruct2 { get; set; }
}