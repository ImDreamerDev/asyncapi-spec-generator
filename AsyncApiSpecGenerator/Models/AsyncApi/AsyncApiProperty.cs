using AsyncApiSpecGenerator.Attributes;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
namespace AsyncApiSpecGenerator.Models.AsyncApi;

internal class AsyncApiProperty(string[] types, AsyncApiDescriptionAttribute? descriptionAttribute)
{

    /// <summary>
    /// We are doing a hack here to support both single and multiple items.
    /// Since it doesn't display correctly as an array in the generated YAML.
    /// </summary>
    [YamlMember(Alias = "type")]
    [JsonPropertyName("type")]
    public object Type => Types.Length == 1 ? Types[0] : Types;

    [YamlIgnore]
    [JsonIgnore]
    public string[] Types { get; } = types ?? throw new ArgumentNullException(nameof(types));

    public string? Description { get; } = descriptionAttribute?.Description;

    /// <summary>
    /// The value of this keyword MUST be an array.  Elements of this array, if any, MUST be strings, and MUST be unique.
    /// An object instance is valid against this keyword if every item in the array is the name of a property in the instance.
    /// Omitting this keyword has the same behavior as an empty array.
    /// <a href="https://datatracker.ietf.org/doc/html/draft-handrews-json-schema-validation-01#section-6.5.3">Source</a>
    /// </summary>
    public string[]? Required { get; set; }

    /// <summary>
    /// The value of "multipleOf" MUST be a number, strictly greater than 0.
    /// A numeric instance is valid only if division by this keyword's value results in an integer.
    /// <a href="https://datatracker.ietf.org/doc/html/draft-handrews-json-schema-validation-01#section-6.2.1">Source</a>
    /// </summary>
    public int? MultipleOf { get; set; }

    /// <summary>
    /// The value of "maximum" MUST be a number, representing an inclusive upper limit for a numeric instance.
    /// If the instance is a number, then this keyword validates only if the instance is less than or exactly equal to "maximum".
    /// <a href="https://datatracker.ietf.org/doc/html/draft-handrews-json-schema-validation-01#section-6.2.2">Source</a>
    /// </summary>
    public int? Maximum { get; set; }

    /// <summary>
    /// The value of "exclusiveMaximum" MUST be number, representing an exclusive upper limit for a numeric instance.
    /// If the instance is a number, then the instance is valid only if it has a value strictly less than (not equal to) "exclusiveMaximum".
    /// <a href="https://datatracker.ietf.org/doc/html/draft-handrews-json-schema-validation-01#section-6.2.3">Source</a>
    /// </summary>
    public int? ExclusiveMaximum { get; set; }

    /// <summary>
    /// The value of "minimum" MUST be a number, representing an inclusive lower limit for a numeric instance.
    /// If the instance is a number, then this keyword validates only if the instance is greater than or exactly equal to "minimum".
    /// <a href="https://datatracker.ietf.org/doc/html/draft-handrews-json-schema-validation-01#section-6.2.4">Source</a>
    /// </summary>
    public int? Minimum { get; set; }


    /// <summary>
    /// The value of "exclusiveMinimum" MUST be number, representing an exclusive lower limit for a numeric instance.
    /// If the instance is a number, then the instance is valid only if it has a value strictly greater than (not equal to) "exclusiveMinimum".
    /// <a href="https://datatracker.ietf.org/doc/html/draft-handrews-json-schema-validation-01#section-6.2.5">Source</a>
    /// </summary>
    public int? ExclusiveMinimum { get; set; }

    /// <summary>
    /// The value of this keyword MUST be a non-negative integer.
    /// A string instance is valid against this keyword if its length is less than, or equal to, the value of this keyword.
    /// The length of a string instance is defined as the number of its characters as defined by RFC 7159 [RFC7159].
    /// <a href="https://datatracker.ietf.org/doc/html/draft-handrews-json-schema-validation-01#section-6.3.1">Source</a>
    /// </summary>
    public uint? MaxLength { get; } = descriptionAttribute?.GetMaxLength();

    /// <summary>
    /// The value of this keyword MUST be a non-negative integer.
    /// A string instance is valid against this keyword if its length is greater than, or equal to, the value of this keyword.
    /// The length of a string instance is defined as the number of its characters as defined by RFC 7159 [RFC7159].
    /// Omitting this keyword has the same behavior as a value of 0.
    /// <a href="https://datatracker.ietf.org/doc/html/draft-handrews-json-schema-validation-01#section-6.3.2"> Source</a>
    /// </summary>
    public uint? MinLength { get; } = descriptionAttribute?.GetMinLength();

    /// <summary>
    /// The value of this keyword MUST be a string.
    /// This string SHOULD be a valid regular expression, according to the ECMA 262 regular expression dialect.
    /// <a href="https://datatracker.ietf.org/doc/html/draft-handrews-json-schema-validation-01#section-6.3.3">Source</a>
    /// </summary>
    public string? Pattern { get; set; }

    /// <summary>
    /// The value of this keyword MUST be a non-negative integer.
    /// An array instance is valid against "maxItems" if its size is less than, or equal to, the value of this keyword.
    /// <a href="https://datatracker.ietf.org/doc/html/draft-handrews-json-schema-validation-01#section-6.4.3">Source</a>
    /// </summary>
    public int? MaxItems { get; set; }

    /// <summary>
    /// The value of this keyword MUST be a non-negative integer.
    /// An array instance is valid against "msetems" if its size is greater than, or equal to, the value of this keyword.
    /// Omitting this keyword has the same behavior as a value of 0.
    /// <a href="https://datatracker.ietf.org/doc/html/draft-handrews-json-schema-validation-01#section-6.4.4">Source</a>
    /// </summary>
    public int? MinItems { get; set; }

    /// <summary>
    /// The value of this keyword MUST be a boolean.
    /// If this keyword has boolean value false, the instance validates successfully.
    /// If it has boolean value true, the instance validates successfully if all of its elements are unique.
    /// Omitting this keyword has the same behavior as a value of false.
    /// <a href="https://datatracker.ietf.org/doc/html/draft-handrews-json-schema-validation-01#section-6.4.5">Source</a>
    /// </summary>
    public bool? UniqueItems { get; set; }

    /// <summary>
    /// The value of this keyword MUST be a non-negative integer.
    /// An object instance is valid against "maxProperties" if its number of properties is less than, or equal to, the value of this keyword.
    /// <a href="https://datatracker.ietf.org/doc/html/draft-handrews-json-schema-validation-01#section-6.5.1">Source</a>
    /// </summary>
    public int? MaxProperties { get; set; }

    /// <summary>
    /// The value of this keyword MUST be a non-negative integer.
    /// An object instance is valid against "minProperties" if its number of properties is greater than, or equal to, the value of this keyword.
    /// Omitting this keyword has the same behavior as a value of 0.
    /// <a href="https://datatracker.ietf.org/doc/html/draft-handrews-json-schema-validation-01#section-6.5.2">Source</a>
    /// </summary>
    public int? MinProperties { get; set; }

    /// <summary>
    /// The value of this keyword MUST be an array.
    /// This array SHOULD have at least one element.
    /// Elements in the array SHOULD be unique.
    /// An instance validates successfully against this keyword if its value is equal to one of the elements in this keyword's array value.
    /// Elements in the array might be of any value, including null.
    /// <a href="https://datatracker.ietf.org/doc/html/draft-handrews-json-schema-validation-01#section-6.1.2">Source</a>
    /// </summary>
    public string[]? Enum { get; set; }

    /// <summary>
    /// The value of this keyword MAY be of any type, including null.
    /// An instance validates successfully against this keyword if its value is equal to the value of the keyword.
    /// <a href="https://datatracker.ietf.org/doc/html/draft-handrews-json-schema-validation-01#section-6.1.3">Source</a>
    /// </summary>
    public string? Const { get; set; }

    /// <summary>
    /// The value of this keyword MUST be an array.
    /// There are no restrictions placed on the values within the array.
    /// When multiple occurrences of this keyword are applicable to a single sub-instance,
    /// implementations MUST provide a flat array of all values rather than an array of arrays.
    /// This keyword can be used to provide sample JSON values associated with a particular schema, for the purpose of illustrating usage.
    /// It is RECOMMENDED that these values be valid against the associated schema.
    /// Implementations MAY use the value(s) of "default", if present, as an additional example.
    /// If "examples" is absent, "default" MAY still be used in this manner.
    /// <a href="https://datatracker.ietf.org/doc/html/draft-handrews-json-schema-validation-01#section-10.4">Source</a>
    /// </summary>
    public object[]? Examples { get; } = ToExample(types, descriptionAttribute);

    /// <summary>
    /// We are doing a hack here to support both single and multiple items.
    /// Since it doesn't display correctly as an array in the generated YAML.
    /// </summary>
    [YamlMember(Alias = "items")]
    [JsonPropertyName("items")]
    public object? Item => Items?.Length == 1 ? Items[0] : Items;

    /// <summary>
    /// The value of "items" MUST be either a valid JSON Schema or an array of valid JSON Schemas.
    ///This keyword determines how child instances validate for arrays, and does not directly validate the immediate instance itself.
    /// If "items" is a schema, validation succeeds if all elements in the array successfully validate against that schema.
    /// If "items" is an array of schemas, validation succeeds if each element of the instance validates against the schema at the same position, if any.
    /// Omitting this keyword has the same behavior as an empty schema.
    /// <a href="https://datatracker.ietf.org/doc/html/draft-handrews-json-schema-validation-01#section-6.4.1">Source</a>
    /// </summary>
    [YamlIgnore]
    [JsonIgnore]
    public AsyncApiProperty[]? Items { get; set; }

    public AsyncApiFormat? Format { get; set; }

    [YamlMember(Alias = "$ref")]
    [JsonPropertyName("$ref")]
    public string? Ref { get; set; }

    /// <summary>
    /// Use it to specify that property has a predefined value if no other value is present.
    /// Unlike JSON Schema, the value MUST conform to the defined type for the Schema Object defined at the same level.
    /// For example, of type is string, then default can be "foo" but cannot be 1.
    /// <a href="https://www.asyncapi.com/docs/reference/specification/v3.0.0#properties">Source</a>
    /// </summary>
    public string? Default { get; set; }

    private static object[]? ToExample(string[] types, AsyncApiDescriptionAttribute? asyncApiDescriptionAttribute)
    {
        var examples = asyncApiDescriptionAttribute?.GetExamples();

        if (examples is null || examples.Length is 0)
            return null;

        if (types.Contains("boolean"))
        {
            return examples.Select(x => bool.TryParse(x, out var result) && result).OfType<object>().ToArray();
        }

        if (types.Contains("number"))
        {
            return examples.Select<string, object?>(x =>
            {
                if (decimal.TryParse(x, out var result))
                    return result;
                if (long.TryParse(x, out var resultLong))
                    return resultLong;
                if (ulong.TryParse(x, out var resultULong))
                    return resultULong;

                return null;
            }).OfType<object>().ToArray();
        }

        if (types.Contains("object") is false && types.Contains("array") is false)
            return examples.OfType<object>().ToArray();

        Console.Error.WriteLine("[WARN] Unsupported example type. Object and array are not supported. Please use examples on the field level.");
        return null;
    }
}