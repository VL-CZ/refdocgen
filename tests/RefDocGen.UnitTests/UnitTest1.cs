using RefDocGen.DocExtraction;

namespace RefDocGen.UnitTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var emptySummary = DocCommentTools.EmptySummary;
        Assert.Equal("summary", emptySummary.Name);
    }
}
