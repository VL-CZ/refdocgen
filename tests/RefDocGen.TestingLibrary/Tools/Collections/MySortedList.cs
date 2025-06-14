namespace RefDocGen.TestingLibrary.Tools.Collections;


internal class MySortedList<T> : MyCollection<T>
    where T : IComparable<T>
{
    /// <inheritdoc/>
    public override void AddRange(IEnumerable<T> range)
    {
        base.AddRange(range);
    }

    public override void Add(T item)
    {
        base.Add(item);
    }
}
