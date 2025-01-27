using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using System.Reflection;

namespace RefDocGen.AssemblyAnalysis.MemberCreators;

/// <summary>
/// Class responsible for creating <see cref="IndexerData"/> instances.
/// </summary>
internal static class IndexerDataCreator
{
    /// <summary>
    /// Creates a <see cref="IndexerData"/> instance from the corresponding <paramref name="propertyInfo"/>.
    /// </summary>
    /// <param name="propertyInfo"><see cref="PropertyInfo"/> object representing the indexer.</param>
    /// <param name="containingType">Type containing the indexer.</param>
    /// <param name="availableTypeParameters">A dictionary of available type parameters; the keys represent type parameter names.</param>
    /// <returns>A <see cref="IndexerData"/> instance representing the indexer.</returns>
    internal static IndexerData CreateFrom(PropertyInfo propertyInfo, TypeDeclaration containingType, Dictionary<string, TypeParameterData> availableTypeParameters)
    {
        var getterMethod = propertyInfo.GetMethod is not null
            ? MethodDataCreator.CreateFrom(propertyInfo.GetMethod, containingType, availableTypeParameters)
            : null;

        var setterMethod = propertyInfo.SetMethod is not null
            ? MethodDataCreator.CreateFrom(propertyInfo.SetMethod, containingType, availableTypeParameters)
            : null;

        return new IndexerData(
            propertyInfo,
            getterMethod,
            setterMethod,
            containingType,
            Helper.GetParametersDictionary(propertyInfo, availableTypeParameters),
            availableTypeParameters,
            Helper.GetAttributeData(propertyInfo, availableTypeParameters));
    }
}
