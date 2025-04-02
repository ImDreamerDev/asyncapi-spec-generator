using AsyncApiSpecGenerator.Models.AsyncApi;
using Shouldly;
using Xunit;
namespace AsyncApiSpecGenerator.Tests;

public class AsyncEventTypeHandler_Should
{
    [Fact]
    public void ToAsyncApiSpecType_Int()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(int), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Int32);
    }

    [Fact]
    public void ToAsyncApiSpecType_Long()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(long), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Int64);
    }

    [Fact]
    public void ToAsyncApiSpecType_Ulong()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(ulong), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.UInt64);
    }

    [Fact]
    public void ToAsyncApiSpecType_Float()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(float), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Float);
    }

    [Fact]
    public void ToAsyncApiSpecType_Double()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(double), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Double);
    }

    [Fact]
    public void ToAsyncApiSpecType_String()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(string), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["string"]);
        typeInfo.Format.ShouldBeNull();
    }

    [Fact]
    public void ToAsyncApiSpecType_DateTime()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(DateTime), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["string"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.DateTime);
    }

    [Fact]
    public void ToAsyncApiSpecType_DateTimeOffset()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(DateTimeOffset), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["string"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.DateTime);
    }

    [Fact]
    public void ToAsyncApiSpecType_TimeOnly()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(TimeOnly), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["string"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Time);
    }

    [Fact]
    public void ToAsyncApiSpecType_DateOnly()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(DateOnly), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["string"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Date);
    }

    [Fact]
    public void ToAsyncApiSpecType_Guid()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(Guid), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["string"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.UUID);
    }

    [Fact]
    public void ToAsyncApiSpecType_Enum()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(AsyncApiFormat), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["string"]);
        typeInfo.Format.ShouldBeNull();
        typeInfo.Enum.ShouldBe(Enum.GetNames<AsyncApiFormat>());
    }

    [Fact]
    public void ToAsyncApiSpecType_Bool()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(bool), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["boolean"]);
        typeInfo.Format.ShouldBeNull();
    }

    [Fact]
    public void ToAsyncApiSpecType_List()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(List<int>), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["array"]);
        typeInfo.Format.ShouldBeNull();
    }

    [Fact]
    public void ToAsyncApiSpecType_IReadOnlyList()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(IReadOnlyList<int>), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["array"]);
        typeInfo.Format.ShouldBeNull();
    }

    [Fact]
    public void ToAsyncApiSpecType_IList()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(IList<int>), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["array"]);
        typeInfo.Format.ShouldBeNull();
    }

    [Fact]
    public void ToAsyncApiSpecType_ICollection()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(ICollection<int>), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["array"]);
        typeInfo.Format.ShouldBeNull();
    }

    [Fact]
    public void ToAsyncApiSpecType_IEnumerable()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(IEnumerable<int>), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["array"]);
        typeInfo.Format.ShouldBeNull();
    }

    [Fact]
    public void ToAsyncApiSpecType_Array()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(int[]), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["array"]);
        typeInfo.Format.ShouldBeNull();
    }

    [Fact]
    public void ToAsyncApiSpecType_Class()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(EventDemo), components);

        components.Schemas.Count.ShouldBe(1);
        components.Schemas["EventDemo"].Types.ShouldBe(["object"]);
        components.Schemas["EventDemo"].Properties.Count.ShouldBe(4);
        components.Schemas["EventDemo"].Properties["Id"].Types.ShouldBe(["string"]);
        components.Schemas["EventDemo"].Properties["Name"].Types.ShouldBe(["string"]);
        components.Schemas["EventDemo"].Properties["Date"].Types.ShouldBe(["string"]);
        components.Schemas["EventDemo"].Properties["Number"].Types.ShouldBe(["number"]);
        typeInfo.Types.ShouldBe(["object"]);
        typeInfo.Format.ShouldBeNull();
        typeInfo.Ref.ShouldBe("#/components/schemas/EventDemo");
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableInt()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(int?), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number", "null"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Int32);
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableLong()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(long?), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number", "null"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Int64);
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableUlong()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(ulong?), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number", "null"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.UInt64);
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableFloat()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(float?), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number", "null"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Float);
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableDouble()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(double?), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number", "null"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Double);
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableString()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(string), components, true);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["string", "null"]);
        typeInfo.Format.ShouldBeNull();
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableDateTime()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(DateTime?), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["string", "null"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.DateTime);
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableDateTimeOffset()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(DateTimeOffset?), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["string", "null"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.DateTime);
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableTimeOnly()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(TimeOnly?), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["string", "null"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Time);
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableDateOnly()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(DateOnly?), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["string", "null"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Date);
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableGuid()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(Guid?), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["string", "null"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.UUID);
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableEnum()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(AsyncApiFormat?), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["string", "null"]);
        typeInfo.Format.ShouldBeNull();
        typeInfo.Enum.ShouldBe(Enum.GetNames<AsyncApiFormat>());
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableBool()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(bool?), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["boolean", "null"]);
        typeInfo.Format.ShouldBeNull();
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableList()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(List<int>), components, true);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["array", "null"]);
        typeInfo.Format.ShouldBeNull();
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableIReadOnlyList()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(IReadOnlyList<int>), components, true);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["array", "null"]);
        typeInfo.Format.ShouldBeNull();
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableIList()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(IList<int>), components, true);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["array", "null"]);
        typeInfo.Format.ShouldBeNull();
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableICollection()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(ICollection<int>), components, true);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["array", "null"]);
        typeInfo.Format.ShouldBeNull();
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableIEnumerable()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(IEnumerable<int>), components, true);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["array", "null"]);
        typeInfo.Format.ShouldBeNull();
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableArray()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(int[]), components, true);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["array", "null"]);
        typeInfo.Format.ShouldBeNull();
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableClass()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(EventDemo), components, true);

        components.Schemas.Count.ShouldBe(1);
        components.Schemas["EventDemo_Nullable"].Types.ShouldBe(["object", "null"]);
        components.Schemas["EventDemo_Nullable"].Properties.Count.ShouldBe(4);
        components.Schemas["EventDemo_Nullable"].Properties["Id"].Types.ShouldBe(["string"]);
        components.Schemas["EventDemo_Nullable"].Properties["Name"].Types.ShouldBe(["string"]);
        components.Schemas["EventDemo_Nullable"].Properties["Date"].Types.ShouldBe(["string"]);
        components.Schemas["EventDemo_Nullable"].Properties["Number"].Types.ShouldBe(["number"]);
        typeInfo.Types.ShouldBe(["object", "null"]);
        typeInfo.Format.ShouldBeNull();
        typeInfo.Ref.ShouldBe("#/components/schemas/EventDemo_Nullable");
    }

    [Fact]
    public void ToAsyncApiSpecType_ValueType()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(TestStruct), components);

        components.Schemas.Count.ShouldBe(1);
        components.Schemas["TestStruct"].Types.ShouldBe(["object"]);
        components.Schemas["TestStruct"].Properties.Count.ShouldBe(2);
        components.Schemas["TestStruct"].Properties["Name"].Types.ShouldBe(["string"]);
        components.Schemas["TestStruct"].Properties["Number"].Types.ShouldBe(["number"]);
        typeInfo.Types.ShouldBe(["object"]);
        typeInfo.Format.ShouldBeNull();
        typeInfo.Ref.ShouldBe("#/components/schemas/TestStruct");
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableValueType()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(TestStruct?), components);

        components.Schemas.Count.ShouldBe(1);
        components.Schemas["TestStruct_Nullable"].Types.ShouldBe(["object", "null"]);
        components.Schemas["TestStruct_Nullable"].Properties.Count.ShouldBe(2);
        components.Schemas["TestStruct_Nullable"].Properties["Name"].Types.ShouldBe(["string"]);
        components.Schemas["TestStruct_Nullable"].Properties["Number"].Types.ShouldBe(["number"]);
        typeInfo.Types.ShouldBe(["object", "null"]);
        typeInfo.Format.ShouldBeNull();
        typeInfo.Ref.ShouldBe("#/components/schemas/TestStruct_Nullable");
    }

    [Fact]
    public void ToAsyncApiSpecType_HashSet()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(HashSet<int>), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["array"]);
        typeInfo.Format.ShouldBeNull();
        typeInfo.Items!.Length.ShouldBe(1);
        typeInfo.UniqueItems = true;
        typeInfo.Items[0].Types.ShouldBe(["number"]);
    }

    [Fact]
    public void ToAsyncApiSpecType_Class_Found()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(MultipleClasses), components);

        components.Schemas.Count.ShouldBe(2);
        components.Schemas["EventDemo"].Types.ShouldBe(["object"]);
        components.Schemas["EventDemo"].Properties.Count.ShouldBe(4);
        typeInfo.Ref.ShouldBe("#/components/schemas/MultipleClasses");
    }

    [Fact]
    public void ToAsyncApiSpecType_Struct_Found()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(MultipleStructs), components);

        components.Schemas.Count.ShouldBe(2);
        components.Schemas["TestStruct"].Types.ShouldBe(["object"]);
        components.Schemas["TestStruct"].Properties.Count.ShouldBe(2);
        typeInfo.Ref.ShouldBe("#/components/schemas/MultipleStructs");
    }

    [Fact]
    public void ToAsyncApiSpecType_ComplexType()
    {
        var components = new AsyncApiComponents();
        AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(EventDemo2), components);

        components.Schemas.Count.ShouldBe(6);
        components.Schemas["EventDemo2"].Types.ShouldBe(["object"]);
        components.Schemas["EventDemo2"].Properties.Count.ShouldBe(27);
    }

    [Fact]
    public void ToAsyncApiSpecType_Byte()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(byte), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Byte);
    }

    [Fact]
    public void ToAsyncApiSpecType_SByte()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(sbyte), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.SByte);
    }

    [Fact]
    public void ToAsyncApiSpecType_Short()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(short), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Int16);
    }

    [Fact]
    public void ToAsyncApiSpecType_UShort()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(ushort), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.UInt16);
    }

    [Fact]
    public void ToAsyncApiSpecType_Char()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(char), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["string"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Char);
    }

    [Fact]
    public void ToAsyncApiSpecType_Decimal()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(decimal), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Decimal);
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableByte()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(byte?), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number", "null"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Byte);
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableSByte()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(sbyte?), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number", "null"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.SByte);
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableShort()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(short?), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number", "null"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Int16);
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableUShort()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(ushort?), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number", "null"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.UInt16);
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableChar()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(char?), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["string", "null"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Char);
    }

    [Fact]
    public void ToAsyncApiSpecType_NullableDecimal()
    {
        var components = new AsyncApiComponents();
        var typeInfo = AsyncEventTypeHandler.ToAsyncApiSpecType(typeof(decimal?), components);

        components.Schemas.Count.ShouldBe(0);
        typeInfo.Types.ShouldBe(["number", "null"]);
        typeInfo.Format.ShouldBe(AsyncApiFormat.Decimal);
    }
}