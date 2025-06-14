namespace RefDocGen.TestingLibrary.Tools;

/// <summary>
/// Predicate about an object.
/// </summary>
/// <param name="obj">The provided object.</param>
/// <returns>True or false.</returns>
internal delegate bool ObjectPredicate(object obj);

/// <summary>
/// Predicate about a generic type T.
/// </summary>
/// <typeparam name="T">The type of the object.</typeparam>
/// <param name="obj">The provided object.</param>
/// <returns>True or false.</returns>
internal delegate bool MyPredicate<T>(T obj);
