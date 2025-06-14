using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;
using System.Text.Json;

namespace RefDocGen.IntegrationTests;

/// <summary>
/// This class tests that documentation versioning works as expected.
/// </summary>
[Collection(VersionedDocumentationTestCollection.Name)]
public class DocVersionTests
{
    /// <summary>
    /// Available versions of the documentation.
    /// </summary>
    private static readonly string[] versions = ["v1.0", "v1.1", "v2.0"];

    /// <summary>
    /// Collection of pages to test
    /// </summary>
    private static readonly string[] pagesToTest = ["htmlPage.html", "folder/anotherPage.html", "api/index.html", "api/RefDocGen.TestingLibrary.html", "api/RefDocGen.TestingLibrary.Dog.html"];

    [Theory]
    [MemberData(nameof(GetVersionedPages))]
    public void VersionList_Matches(string pageName, string version)
    {
        using var document = DocumentationTools.GetPage(pageName, VersionedDocumentationFixture.outputDir, version);

        string versionList = document.GetVersionList();

        string[]? versions = JsonSerializer.Deserialize<string[]>(versionList);

        versions.ShouldBe(DocVersionTests.versions);
    }

    [Theory]
    [MemberData(nameof(GetVersionedPages))]
    public void CurrentVersion_Matches(string pageName, string version)
    {
        using var document = DocumentationTools.GetPage(pageName, VersionedDocumentationFixture.outputDir, version);

        string currentVersion = document.GetCurrentVersion();

        currentVersion.ShouldBe(version);
    }

    /// <summary>
    /// Get all combinations of (page, version) as <see cref="TheoryData"/>.
    /// </summary>
    /// <returns>All combinations of (page, version) as <see cref="TheoryData"/>.</returns>
    public static TheoryData<string, string> GetVersionedPages()
    {
        var data = new TheoryData<string, string>();

        foreach (string page in pagesToTest)
        {
            foreach (string v in versions)
            {
                data.Add(page, v);
            }
        }

        return data;
    }
}
