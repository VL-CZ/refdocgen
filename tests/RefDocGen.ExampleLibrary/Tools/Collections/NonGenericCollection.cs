using System.Collections;

namespace RefDocGen.ExampleLibrary.Tools.Collections
{
    /// <summary>
    /// Non generic collection interface.
    /// </summary>
    interface INonGenericCollection
    {
        int Count { get; }

        T Get<T>(int index);
    }

    /// <summary>
    /// Non generic collection class.
    /// </summary>
    internal class NonGenericCollection : ICollection, INonGenericCollection
    {
        public int Count => throw new NotImplementedException();

        public object SyncRoot => throw new NotImplementedException();

        /// <summary>
        /// Gets a value indicating whether the collection is thread-safe (synchronized) or not.
        /// </summary>
        bool ICollection.IsSynchronized => false;

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the item at the given index.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="index">Index of the item.</param>
        /// <returns>The item at the given index.</returns>
        public T Get<T>(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Add a new item.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="item">The item to add.</param>
        public void Add<T>(T item)
        {

        }
    }
}
