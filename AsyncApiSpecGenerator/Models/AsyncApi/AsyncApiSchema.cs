using YamlDotNet.Serialization;
namespace AsyncApiSpecGenerator.Models.AsyncApi;

internal class AsyncApiSchema(string[] types)
{
    [YamlMember(Alias = "type")]
    public string[] Types { get; } = types;
    public Dictionary<string, AsyncApiProperty> Properties { get; init; } = new Dictionary<string, AsyncApiProperty>();

    /// <summary>
    /// The value of "additionalProperties" MUST be a valid JSON Schema.
    /// This keyword determines how child instances validate for objects, and does not directly validate the immediate instance itself.
    /// Validation with "additionalProperties" applies only to the child values of instance names that do not match any names in "properties",
    /// and do not match any regular expression in "patternProperties".
    /// For all such properties, validation succeeds if the child instance validates against the "additionalProperties" schema.
    /// Omitting this keyword has the same behavior as an empty schema.
    /// </summary>
    public bool AdditionalProperties { get; init; } = false;
}