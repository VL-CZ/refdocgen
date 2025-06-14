using System.Collections;

namespace RefDocGen.ExampleLibrary.Tools.Collections;

/// <summary>
/// Represents a custom collection containing the items of type <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public class MyCollection<T> : IEnumerable, IMyCollection<T>
{
    /// <summary>
    /// Status of the collection.
    /// </summary>
    private enum Status
    {
        /// <summary>
        /// Empty collection.
        /// </summary>
        Empty,

        /// <summary>
        /// Non-empty collection.
        /// </summary>
        NonEmpty
    };

    /// <summary>
    /// Generic collection enumerator.
    /// </summary>
    /// <typeparam name="T2"></typeparam>
    private class GenericEnumerator<T2>
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="item"></param>
        /// <param name="another"></param>
        private void Handle(T item, T2 another)
        {

        }
    }

    /// <summary>
    /// Custom collection enumerator.
    /// </summary>
    private class MyCollectionEnumerator : IEnumerator<T>
    {
        /// <summary>
        /// Gets the current object.
        /// </summary>
        public T Current => throw new NotImplementedException();

        /// <inheritdoc cref="Current"/>
        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Moves to the next element.
        /// </summary>
        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Resets the enumerator.
        /// </summary>
        public void Reset()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Gets the number of elements contained in the collection. <typeparamref name="T"/>
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Gets a value indicating whether the collection is read-only.
    /// </summary>
    /// <value>Indicates whether the collection is read-only.</value>
    public bool IsReadOnly { get; }

    /// <summary>
    /// Adds an item to the collection.
    /// </summary>
    /// <param name="item">The item to add to the collection.</param>
    /// <exception cref="NotImplementedException"></exception>
    /// <exception cref="ArgumentNullException">If the argument is null.</exception>
    public virtual void Add(T item)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Enumerate the given collection
    /// </summary>
    /// <returns>Tuple containing (index, item)</returns>
    public IEnumerable<(int index, T item)> Enumerate()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public virtual void AddRange(IEnumerable<T> range)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes all items from the collection.
    /// </summary>
    public void Clear()
    {
        throw new NotImplementedException();
    }

    /// <include file='./docs.xml' path='doc/members/member[@name="M:MyNamespace.MyClass.MyMethod"]/*'/>
    public bool Contains(T item)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Copies the elements of the collection to an array, starting at a specific array index.
    /// </summary>
    /// <param name="array">The one-dimensional array that is the destination of the elements copied from the collection.</param>
    /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
    public void CopyTo(T[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public void Execute(Func<T, T> operation)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An enumerator for the collection.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add a generic item into the collection.
    /// </summary>
    /// <typeparam name="T2">Type of the item to add.</typeparam>
    /// <param name="item">Item to add.</param>
    public void AddGeneric<T2>(T2 item)
        where T2 : class
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes the first occurrence of a specific item from the collection.
    /// </summary>
    /// <param name="item">The item to remove from the collection.</param>
    /// <returns><c>true</c> if the item was successfully removed; otherwise, <c>false</c>.</returns>
    public bool Remove(T item)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An enumerator for the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets a custom enumerator.
    /// </summary>
    private MyCollectionEnumerator GetCustomEnumerator()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    bool IMyCollection<T>.CanAdd()
    {
        return false;
    }

    /// <summary>
    /// Return an item at the given index.
    /// </summary>
    /// <param name="index">An item index.</param>
    /// <returns>The item at the given index.</returns>
    public T this[int index]
    {
        get
        {
            return default;
        }
        private set
        {
            _ = value;
        }
    }

    /// <summary>
    /// Return an item at the given index.
    /// </summary>
    /// <param name="index">An Index struct.</param>
    /// <returns>The item at the given index.</returns>
    public T this[Index index]
    {
        get
        {
            return default;
        }
        private set
        {
            _ = value;
        }
    }
}
