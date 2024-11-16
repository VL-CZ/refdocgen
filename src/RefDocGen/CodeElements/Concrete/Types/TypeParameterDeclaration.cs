namespace RefDocGen.CodeElements.Concrete.Types;

/// <summary>
/// Represents declaration of a generic type parameter.
/// </summary>
/// <param name="Name">Name of the type parameter (e.g. 'TKey').</param>
/// <param name="Index">Index of the parameter in the declaring type parameter collection.</param>
internal readonly record struct TypeParameterDeclaration(string Name, int Index);
