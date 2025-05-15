using RefDocGen.CodeElements.Members.Concrete;
using RefDocGen.CodeElements.Types.Concrete;
using System.Reflection;

namespace RefDocGen.AssemblyAnalysis.MemberCreators;

/// <summary>
/// Class responsible for creating <see cref="FieldData"/> instances.
/// </summary>
internal static class FieldDataCreator
{
    /// <summary>
    /// Creates a <see cref="FieldData"/> instance from the corresponding <paramref name="field"/>.
    /// </summary>
    /// <param name="field"><see cref="FieldInfo"/> object representing the field.</param>
    /// <param name="containingType">Type containing the field.</param>
    /// <param name="availableTypeParameters">A dictionary of available type parameters; the keys represent type parameter names.</param>
    /// <returns>A <see cref="FieldData"/> instance representing the field.</returns>
    internal static FieldData CreateFrom(FieldInfo field, TypeDeclaration containingType, Dictionary<string, TypeParameterData> availableTypeParameters)
    {
        return new FieldData(
            field,
            containingType,
            availableTypeParameters,
            MemberCreatorHelper.GetAttributeData(field, availableTypeParameters));
    }
}
