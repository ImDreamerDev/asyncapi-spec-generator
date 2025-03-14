using System.Runtime.Serialization;
namespace AsyncApiSpecGenerator.Models.AsyncApi;

/// <summary>
/// <a href="https://www.asyncapi.com/docs/reference/specification/v3.0.0#dataTypeFormat">Source</a>
/// </summary>
internal enum AsyncApiFormat
{
    [EnumMember(Value = "int32")]
    Int32,
    [EnumMember(Value = "int64")]
    Int64,
    [EnumMember(Value = "float")]
    Float,
    [EnumMember(Value = "double")]
    Double,
    [EnumMember(Value = "byte")]
    Byte,
    [EnumMember(Value = "binary")]
    Binary,
    [EnumMember(Value = "date")]
    Date,
    [EnumMember(Value = "date-time")]
    DateTime,
    [EnumMember(Value = "password")]
    Password,
    [EnumMember(Value = "email")]
    Email,
    [EnumMember(Value = "uuid")]
    UUID,
    [EnumMember(Value = "time")]
    Time,
    // Custom formats
    [EnumMember(Value = "decimal")]
    Decimal,
    [EnumMember(Value = "int16")]
    Int16,
    [EnumMember(Value = "uint16")]
    UInt16,
    [EnumMember(Value = "uint32")]
    UInt32,
    [EnumMember(Value = "uint64")]
    UInt64,
    [EnumMember(Value = "sbyte")]
    SByte,
    [EnumMember(Value = "char")]
    Char
}