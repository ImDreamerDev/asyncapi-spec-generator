using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
namespace AsyncApiSpecGenerator.Models.AsyncApi;

internal class AsyncApiPayload(string @ref)
{
    [YamlMember(Alias = "$ref")]
    [JsonPropertyName("$ref")]
    public string Ref { get; } = @ref;
}