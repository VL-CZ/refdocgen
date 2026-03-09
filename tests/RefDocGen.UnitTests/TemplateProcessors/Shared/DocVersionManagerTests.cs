using RefDocGen.TemplateProcessors.Shared.DocVersioning;
using RefDocGen.Tools.Exceptions;
using Shouldly;
using System.Text.Json;

namespace RefDocGen.UnitTests.TemplateProcessors.Shared;

/// <summary>
/// Class containing tests for <see cref="DocVersionManager"/> class.
/// </summary>
public class DocVersionManagerTests : IDisposable
{
    /// <summary>
    /// Temporary directory used as the base output directory for tests.
    /// </summary>
    private readonly string tempDirectory;

    public DocVersionManagerTests()
    {
        tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDirectory);
    }

    public void Dispose()
    {
        Directory.Delete(tempDirectory, true);
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void Constructor_ThrowsDuplicateDocVersionNameException_WhenVersionAlreadyExistsAndForceCreateIsFalse()
    {
        var firstManager = new DocVersionManager(tempDirectory, "v1.0");
        firstManager.SaveCurrentVersionData([]);

        Should.Throw<DuplicateDocVersionNameException>(() => new DocVersionManager(tempDirectory, "v1.0", forceCreate: false));
    }

    [Fact]
    public void Constructor_DoesNotThrow_WhenVersionAlreadyExistsAndForceCreateIsTrue()
    {
        var firstManager = new DocVersionManager(tempDirectory, "v1.0");
        firstManager.SaveCurrentVersionData([]);

        Should.NotThrow(() => new DocVersionManager(tempDirectory, "v1.0", forceCreate: true));
    }

    [Fact]
    public void Constructor_DeletesExistingVersionDirectory_WhenForceCreateIsTrue()
    {
        string versionDir = Path.Join(tempDirectory, "v1.0");
        Directory.CreateDirectory(versionDir);
        File.WriteAllText(Path.Join(versionDir, "page.html"), "<html/>");

        var firstManager = new DocVersionManager(tempDirectory, "v1.0");
        firstManager.SaveCurrentVersionData(["page.html"]);

        _ = new DocVersionManager(tempDirectory, "v1.0", forceCreate: true);

        Directory.Exists(versionDir).ShouldBeFalse();
    }

    [Fact]
    public void Constructor_RemovesExistingVersionFromVersionsList_WhenForceCreateIsTrue()
    {
        var firstManager = new DocVersionManager(tempDirectory, "v1.0");
        firstManager.SaveCurrentVersionData([]);

        var secondManager = new DocVersionManager(tempDirectory, "v1.0", forceCreate: true);
        secondManager.SaveCurrentVersionData([]);

        string json = File.ReadAllText(Path.Join(tempDirectory, "versions.json"));
        var versions = JsonSerializer.Deserialize<List<JsonElement>>(json);

        versions.ShouldNotBeNull();
        versions.Count.ShouldBe(1); // only one entry, not two
        versions[0].GetProperty("Version").GetString().ShouldBe("v1.0");
    }
}
