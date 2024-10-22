using RefDocGen.DocExtraction.Tools;

namespace RefDocGen.UnitTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var emptySummary = EmptyDocCommentNode.Summary;
        Assert.Equal("summary", emptySummary.Name);
    }
}
