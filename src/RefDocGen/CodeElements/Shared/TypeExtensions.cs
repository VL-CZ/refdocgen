using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.CodeElements.Types.Concrete;
using RefDocGen.CodeElements.Types.Concrete.TypeName;
using RefDocGen.Tools;

namespace RefDocGen.CodeElements.Shared;

/// <summary>
/// Class containing extension methods for <see cref="Type"/> class.
/// </summary>
internal static class TypeExtensions
{
    /// <summary>
    /// Transform the <see cref="Type"/> instance into the corresponding <see cref="ITypeNameData"/> object.
    /// </summary>
    /// <param name="type">Type to transform.</param>
    /// <param name="availableTypeParameters">Collection of type parameters declared in the containing type; the keys represent type parameter names.</param>
    /// <returns><see cref="ITypeNameData"/> instance corresponding to the given type.</returns>
    internal static ITypeNameData GetTypeNameData(this Type type, IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters)
    {
        return type.IsGenericParameter || type.GetBaseElementType().IsGenericParameter
            ? new GenericTypeParameterNameData(type, availableTypeParameters)
            : new TypeNameData(type, availableTypeParameters);
    }

    /// <summary>
    /// Transform the <see cref="Type"/> instance into the corresponding <see cref="ITypeNameData"/> object.
    /// </summary>
    /// <param name="type">Type to transform.</param>
    /// <returns><see cref="ITypeNameData"/> instance corresponding to the given type.</returns>
    internal static ITypeNameData GetTypeNameData(this Type type)
    {
        return type.GetTypeNameData(new Dictionary<string, TypeParameterData>());
    }
}
