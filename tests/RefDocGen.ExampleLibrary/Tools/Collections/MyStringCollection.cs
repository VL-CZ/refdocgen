namespace RefDocGen.ExampleLibrary.Tools.Collections;

/// <summary>
/// Custom string collection.
/// </summary>
/// <seealso cref="MyCollection{T}">My collection class</seealso>
/// <seealso cref="ICollection{T}"/>
internal class MyStringCollection : MyCollection<string>
{
    /// <inheritdoc/>
    public override void Add(string item)
    {
        base.Add(item);
    }

    /// <inheritdoc/>
    public override void AddRange(IEnumerable<string> range)
    {
        base.AddRange(range);
    }
}
