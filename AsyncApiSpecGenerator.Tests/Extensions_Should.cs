using AsyncApiSpecGenerator.Models.AsyncApi;
using Shouldly;
using Xunit;
namespace AsyncApiSpecGenerator.Tests;

public class Extensions_Should
{
    [Fact]
    public void GetsPropertyNamesUsingJson()
    {
        var components = new AsyncApiComponents();
        var type = typeof(Todo);
        var types = type.ToAsyncApiProperties(components);

        types.Count.ShouldBe(3);
        types["demo_title"].Types.ShouldBe(["string"]);
        types["demo_description"].Types.ShouldBe(["string", "null"]);
        types["text_done"].Types.ShouldBe(["boolean"]);
    }

    [Fact]
    public void GetsCorrectTypes()
    {
        var components = new AsyncApiComponents();
        var type = typeof(EventDemo);
        var types = type.ToAsyncApiProperties(components);

        types.Count.ShouldBe(4);
        types["Id"].Types.ShouldBe(["string"]);
        types["Name"].Types.ShouldBe(["string"]);
        types["Date"].Types.ShouldBe(["string"]);
        types["Number"].Types.ShouldBe(["number"]);
    }

    [Fact]
    public void InheritsOrImplements_ReturnsTrue_WhenChildInheritsParent()
    {
        // Arrange
        var childType = typeof(DerivedClass);
        var parentType = typeof(BaseClass);

        // Act
        var result = childType.InheritsOrImplements(parentType);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void InheritsOrImplements_ReturnsTrue_WhenChildImplementsInterface()
    {
        // Arrange
        var childType = typeof(ImplementingClass);
        var parentType = typeof(IInterface);

        // Act
        var result = childType.InheritsOrImplements(parentType);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void InheritsOrImplements_ReturnsFalse_WhenChildDoesNotInheritOrImplementParent()
    {
        // Arrange
        var childType = typeof(UnrelatedClass);
        var parentType = typeof(BaseClass);

        // Act
        var result = childType.InheritsOrImplements(parentType);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void InheritsOrImplements_ThrowsArgumentNullException_WhenChildIsNull()
    {
        // Arrange
        Type childType = null!;
        var parentType = typeof(BaseClass);

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => childType.InheritsOrImplements(parentType));
    }

    [Fact]
    public void InheritsOrImplements_ThrowsArgumentNullException_WhenParentIsNull()
    {
        // Arrange
        var childType = typeof(DerivedClass);
        Type parentType = null!;

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => childType.InheritsOrImplements(parentType));
    }

    private class BaseClass;
    private class DerivedClass : BaseClass;
    private interface IInterface;
    private class ImplementingClass : IInterface;
    private class UnrelatedClass;
}