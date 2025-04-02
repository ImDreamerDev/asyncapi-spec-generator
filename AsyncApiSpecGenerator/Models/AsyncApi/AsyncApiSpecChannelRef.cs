using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
namespace AsyncApiSpecGenerator.Models.AsyncApi;

internal class AsyncApiSpecChannelRef(string @ref)
{
    [JsonPropertyName("$ref")]
    [YamlMember(Alias = "$ref")]
    public string Ref { get; } = @ref;
}