using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using System.Reflection;

namespace RefDocGen.AssemblyAnalysis.MemberCreators;

/// <summary>
/// Class responsible for creating <see cref="ConstructorData"/> instances.
/// </summary>
internal static class ConstructorDataCreator
{
    /// <summary>
    /// Creates a <see cref="ConstructorData"/> instance from the corresponding <paramref name="constructor"/>.
    /// </summary>
    /// <param name="constructor"><see cref="ConstructorInfo"/> object representing the constructor.</param>
    /// <param name="containingType">Type containing the constructor.</param>
    /// <param name="availableTypeParameters">A dictionary of available type parameters; the keys represent type parameter names.</param>
    /// <returns>A <see cref="ConstructorData"/> instance representing the constructor.</returns>
    internal static ConstructorData CreateFrom(ConstructorInfo constructor, TypeDeclaration containingType, Dictionary<string, TypeParameterData> availableTypeParameters)
    {
        return new ConstructorData(
            constructor,
            containingType,
            Helper.GetParametersDictionary(constructor, availableTypeParameters),
            Helper.GetAttributeData(constructor, availableTypeParameters));
    }
}
