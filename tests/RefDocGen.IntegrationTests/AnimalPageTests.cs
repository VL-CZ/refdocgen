using AngleSharp.Dom;
using Shouldly;

namespace RefDocGen.IntegrationTests;

[Collection(MyTestCollection.Name)]
public class AnimalPageTests : IDisposable
{
    private readonly IDocument document;

    public AnimalPageTests()
    {
        document = Tools.GetDocument("MyLibrary.Animal.html");
    }

    public void Dispose()
    {
        document.Dispose();
    }

    [Fact]
    public void Test_GetSoundMethod()
    {
        var method = document.GetMember("GetSound()");

        string methodName = Tools.GetMemberNameContent(method);
        methodName.ShouldBe("internal abstract string GetSound()");

        string summaryDoc = Tools.GetSummaryDocContent(method);
        summaryDoc.ShouldBe("Abstract method to get the animal's sound.");
    }

    [Fact]
    public void Test_GetAverageLifespanMethod()
    {
        var method = document.GetMember("GetAverageLifespan(System.String)");

        string methodName = Tools.GetMemberNameContent(method);
        methodName.ShouldBe("public static int GetAverageLifespan(string species)");

        string summaryDoc = Tools.GetSummaryDocContent(method);
        summaryDoc.ShouldBe("Static method returning the average lifespan of an animal.");

        var parameter = Tools.GetMemberParameters(method).Single();

        string paramSignature = Tools.GetParameterName(parameter);
        paramSignature.ShouldBe("string species");

        string paramDoc = Tools.GetParameterDoc(method);
        paramDoc.ShouldBe("The species of the animal.");

        string returnDoc = Tools.GetReturnsDoc(method);
        returnDoc.ShouldBe("The average lifespan.");
    }

    [Fact]
    public void Test_GenerateAnimalProfileMethod()
    {
        var method = document.GetMember("GenerateAnimalProfile(System.String,System.String,System.DateTime)");

        string methodName = Tools.GetMemberNameContent(method);
        methodName.ShouldBe("public virtual string GenerateAnimalProfile(string name, string habitat, DateTime dateOfBirth)");

        string summaryDoc = Tools.GetSummaryDocContent(method);
        summaryDoc.ShouldBe("A virtual method to generate an animal profile.");

        var parameters = Tools.GetMemberParameters(method);
        parameters.Length.ShouldBe(3);

        string paramSignature1 = Tools.GetParameterName(parameters[0]);
        paramSignature1.ShouldBe("string name");

        string paramDoc1 = Tools.GetParameterDoc(parameters[0]);
        paramDoc1.ShouldBe("Animal's name.");

        string paramSignature2 = Tools.GetParameterName(parameters[1]);
        paramSignature2.ShouldBe("string habitat");

        string paramDoc2 = Tools.GetParameterDoc(parameters[1]);
        paramDoc2.ShouldBe("Animal's habitat.");

        string paramSignature3 = Tools.GetParameterName(parameters[2]);
        paramSignature3.ShouldBe("DateTime dateOfBirth");

        string paramDoc3 = Tools.GetParameterDoc(parameters[2]);
        paramDoc3.ShouldBe("Animal's birthdate.");

        string returnDoc = Tools.GetReturnsDoc(method);
        returnDoc.ShouldBe("Profile of the animal as a string.");
    }


}
