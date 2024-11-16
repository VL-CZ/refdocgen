namespace RefDocGen.CodeElements.Concrete.Types;

/// <summary>
/// Represents declaration of a generic type parameter.
/// </summary>
/// <param name="Name">Name of the type parameter (e.g. 'TKey').</param>
/// <param name="Order">Index of the parameter in the parameter collection of the declaring type.</param>
internal readonly record struct TypeParameterDeclaration(string Name, int Order);
