namespace RefDocGen.IntegrationTests.Fixtures;

/// <summary>
/// Marks the collection of tests that use the <see cref="VersionedDocumentationFixture"/>.
/// </summary>
[CollectionDefinition(Name)]
#pragma warning disable CA1711
public class VersionedDocumentationTestCollection : ICollectionFixture<VersionedDocumentationFixture>
{
    /// <summary>
    /// Name of this test collection.
    /// </summary>
    internal const string Name = "versioned-documentation-test-collection";
}

#pragma warning restore CA1711
