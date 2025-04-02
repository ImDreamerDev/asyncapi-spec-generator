using AsyncApiSpecGenerator.Attributes;
using AsyncApiSpecGenerator.Models.AsyncApi;
namespace AsyncApiSpecGenerator;

internal static class AsyncEventTypeHandler
{
    internal static AsyncApiProperty ToAsyncApiSpecType(Type type, AsyncApiComponents components, bool nullableProperty = false, AsyncApiDescriptionAttribute? descriptionAttribute = null)
    {
        var isNullable = Nullable.GetUnderlyingType(type) is not null;
        type = isNullable ? Nullable.GetUnderlyingType(type)! : type;

        var result = HandleNumberType(type, isNullable, descriptionAttribute);

        if (result is not null)
        {
            return result;
        }

        result = HandleStringType(type, isNullable, nullableProperty, descriptionAttribute);

        if (result is not null)
        {
            return result;
        }

        if (type == typeof(bool))
        {
            return Create("boolean", isNullable || nullableProperty, descriptionAttribute);
        }

        result = HandleArrayType(type, components, isNullable, nullableProperty, descriptionAttribute);

        if (result is not null)
        {
            return result;
        }

        if (type.IsClass || type.IsValueType)
        {
            return HandleClassAndValueType(type, components, isNullable, nullableProperty, descriptionAttribute);
        }

        throw new NotImplementedException(type.ToString());
    }

    private static AsyncApiProperty? HandleNumberType(Type type, bool isNullable, AsyncApiDescriptionAttribute? descriptionAttribute)
    {
        var result = Create("number", isNullable, descriptionAttribute);
        if (type == typeof(byte))
        {
            result.Format = AsyncApiFormat.Byte;
            return result;
        }

        if (type == typeof(sbyte))
        {
            result.Format = AsyncApiFormat.SByte;
            return result;
        }

        if (type == typeof(decimal))
        {
            result.Format = AsyncApiFormat.Decimal;
            return result;
        }

        if (type == typeof(double))
        {
            result.Format = AsyncApiFormat.Double;
            return result;
        }

        if (type == typeof(float))
        {
            result.Format = AsyncApiFormat.Float;
            return result;
        }

        if (type == typeof(int) || type == typeof(nint))
        {
            result.Format = AsyncApiFormat.Int32;
            return result;
        }

        if (type == typeof(uint) || type == typeof(nuint))
        {
            result.Format = AsyncApiFormat.UInt32;
            return result;
        }

        if (type == typeof(long))
        {
            result.Format = AsyncApiFormat.Int64;
            return result;
        }

        if (type == typeof(ulong))
        {
            result.Format = AsyncApiFormat.UInt64;
            return result;
        }

        if (type == typeof(short))
        {
            result.Format = AsyncApiFormat.Int16;
            return result;
        }

        if (type != typeof(ushort))
            // We purposely return null here, as we want to handle the type in the next method
            return null;

        result.Format = AsyncApiFormat.UInt16;
        return result;
    }

    private static AsyncApiProperty? HandleStringType(Type type, bool isNullable, bool nullableProperty, AsyncApiDescriptionAttribute? descriptionAttribute)
    {
        var result = Create("string", isNullable || nullableProperty, descriptionAttribute);

        if (type == typeof(char))
        {
            result.Format = AsyncApiFormat.Char;
            return result;
        }

        if (type == typeof(string))
        {
            return result;
        }

        if (type == typeof(DateTime) || type == typeof(DateTimeOffset))
        {
            result.Format = AsyncApiFormat.DateTime;
            return result;
        }

        if (type == typeof(DateOnly))
        {
            result.Format = AsyncApiFormat.Date;
            return result;
        }

        if (type == typeof(TimeOnly))
        {
            result.Format = AsyncApiFormat.Time;
            return result;
        }

        if (type == typeof(Guid))
        {
            result.Format = AsyncApiFormat.UUID;
            return result;
        }

        if (type.IsEnum is false)
            // We purposely return null here, as we want to handle the type in the next method
            return null;

        result.Enum = Enum.GetNames(type);
        return result;
    }

    private static AsyncApiProperty? HandleArrayType(Type type, AsyncApiComponents components, bool isNullable, bool nullableProperty, AsyncApiDescriptionAttribute? descriptionAttribute)
    {
        var result = Create("array", isNullable || nullableProperty, descriptionAttribute);
        switch (type.IsGenericType)
        {
            case true when type.GetGenericTypeDefinition() == typeof(HashSet<>):
                result.Items = [ToAsyncApiSpecType(type.GetGenericArguments()[0], components)];
                result.UniqueItems = true;
                return result;
            case true when type.GetGenericTypeDefinition().InheritsOrImplements(typeof(IEnumerable<>)):
                result.Items = [ToAsyncApiSpecType(type.GetGenericArguments()[0], components)];
                return result;
        }

        if (type.IsArray is false)
            // We purposely return null here, as we want to handle the type in the next method
            return null;

        result.Items = [ToAsyncApiSpecType(type.GetElementType()!, components)];
        return result;
    }

    private static AsyncApiProperty HandleClassAndValueType(Type type, AsyncApiComponents components, bool isNullable, bool nullableProperty, AsyncApiDescriptionAttribute? descriptionAttribute)
    {
        var result = Create("object", isNullable || nullableProperty, descriptionAttribute);
        var name = type.Name;
        if (isNullable || nullableProperty)
        {
            name = $"{type.Name}_Nullable";
        }

        if (components.Schemas.ContainsKey(name))
        {
            result.Ref = $"#/components/schemas/{name}";
            return result;
        }

        var schema = new AsyncApiSchema(isNullable == false && nullableProperty == false ? ["object"] : ["object", "null"])
        {
            Properties = type.ToAsyncApiProperties(components)
        };

        components.Schemas.Add(name, schema);
        result.Ref = $"#/components/schemas/{name}";
        return result;
    }

    private static AsyncApiProperty Create(string type, bool isNullable, AsyncApiDescriptionAttribute? descriptionAttribute)
    {
        if (isNullable)
        {
            return new AsyncApiProperty([type, "null"], descriptionAttribute);
        }

        // We purposely return null here, as we want to handle the type in the next method
        return new AsyncApiProperty([type], descriptionAttribute);
    }
}