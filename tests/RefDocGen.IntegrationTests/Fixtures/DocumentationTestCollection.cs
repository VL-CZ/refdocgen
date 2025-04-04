namespace RefDocGen.IntegrationTests.Fixtures;

/// <summary>
/// Marks the collection of tests that use the <see cref="DocumentationFixture"/>.
/// </summary>
[CollectionDefinition(Name)]
#pragma warning disable CA1711
public class DocumentationTestCollection : ICollectionFixture<DocumentationFixture>
{
    /// <summary>
    /// Name of this test collection.
    /// </summary>
    internal const string Name = "documentation-test-collection";
}

#pragma warning restore CA1711
