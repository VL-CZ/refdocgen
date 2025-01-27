using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using System.Reflection;

namespace RefDocGen.AssemblyAnalysis.MemberCreators;

/// <summary>
/// Class responsible for creating <see cref="PropertyData"/> instances.
/// </summary>
internal static class PropertyDataCreator
{
    /// <summary>
    /// Creates a <see cref="PropertyData"/> instance from the corresponding <paramref name="property"/>.
    /// </summary>
    /// <param name="property"><see cref="PropertyInfo"/> object representing the property.</param>
    /// <param name="containingType">Type containing the property.</param>
    /// <param name="availableTypeParameters">A dictionary of available type parameters; the keys represent type parameter names.</param>
    /// <returns>A <see cref="PropertyData"/> instance representing the property.</returns>
    internal static PropertyData CreateFrom(PropertyInfo property, TypeDeclaration containingType, Dictionary<string, TypeParameterData> availableTypeParameters)
    {
        var getterMethod = property.GetMethod is not null
            ? MethodDataCreator.CreateFrom(property.GetMethod, containingType, availableTypeParameters)
            : null;

        var setterMethod = property.SetMethod is not null
            ? MethodDataCreator.CreateFrom(property.SetMethod, containingType, availableTypeParameters)
            : null;

        return new PropertyData(
            property,
            getterMethod,
            setterMethod,
            containingType,
            availableTypeParameters,
            MemberCreatorHelper.GetAttributeData(property, availableTypeParameters));
    }
}
