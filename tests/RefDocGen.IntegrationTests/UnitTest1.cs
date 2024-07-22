using RefDocGen.DocExtraction;

namespace RefDocGen.IntegrationTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var emptySummary = DocCommentTools.EmptySummaryNode;
        Assert.Equal("summary", emptySummary.Name);
    }
}
