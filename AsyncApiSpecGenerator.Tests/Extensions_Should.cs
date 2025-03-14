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
    
}