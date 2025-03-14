using YamlDotNet.Serialization;
namespace AsyncApiSpecGenerator.Models.AsyncApi;

internal class AsyncApiPayload(string @ref)
{
    [YamlMember(Alias = "$ref")]
    public string Ref { get; } = @ref;
}