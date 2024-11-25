using RefDocGen.Tools.Xml;

namespace RefDocGen.IntegrationTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var emptySummary = XmlDocElements.EmptySummary;
        Assert.Equal("summary", emptySummary.Name);
    }
}
