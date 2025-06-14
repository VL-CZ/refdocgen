namespace RefDocGen.TestingLibrary.Tools;

/// <summary>
/// An interface with a covariant type.
/// </summary>
/// <typeparam name="T">Covariant type</typeparam>
internal interface ICovariant<out T>
{
}

/// <summary>
/// An interface with a contravariant type.
/// </summary>
/// <typeparam name="T">Contravariant type</typeparam>
internal interface IContravariant<in T> { }
