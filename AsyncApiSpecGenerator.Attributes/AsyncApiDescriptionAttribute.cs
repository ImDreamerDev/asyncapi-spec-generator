namespace AsyncApiSpecGenerator.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class AsyncApiDescriptionAttribute : Attribute
{
    public string? Description { get; set; }
    public int MinLength { get; set; } = int.MinValue;
    public int MaxLength { get; set; } = int.MinValue;
    public string? Example { get; set; }
    public string[]? Examples { get; set; }

    public uint? GetMinLength()
    {
        if (MinLength == int.MinValue)
            return null;
        return (uint)MinLength;
    }

    public uint? GetMaxLength()
    {
        if (MaxLength == int.MinValue)
            return null;
        return (uint)MaxLength;
    }

    public string[]? GetExamples()
    {
        if (Examples is not null && Examples.Length > 0)
        {
            return Examples;
        }

        return Example is not null ? [Example] : null;
    }
}