using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types.Attribute;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Tools;
using System.Reflection;

namespace RefDocGen.CodeElements.Concrete.Members;

/// <inheritdoc cref="IIndexerData"/>
internal class IndexerData : PropertyData, IIndexerData
{
    /// <summary>
    /// Create new instance of <see cref="IndexerData"/> class.
    /// </summary>
    /// <param name="propertyInfo"><see cref="PropertyInfo"/> object representing the indexer.</param>
    /// <param name="containingType">Type that contains the member.</param>
    /// <param name="availableTypeParameters">Collection of type parameters declared in the containing type; the keys represent type parameter names.</param>
    /// <param name="attributes">Collection of attributes applied to the indexer.</param>
    /// <param name="parameters">Dictionary of indexer parameters, the keys represent parameter names.</param>
    /// <param name="getterMethod"><inheritdoc cref="PropertyData.Getter"/></param>
    /// <param name="setterMethod"><inheritdoc cref="PropertyData.Setter"/></param>
    internal IndexerData(
        PropertyInfo propertyInfo,
        MethodData? getterMethod,
        MethodData? setterMethod,
        TypeDeclaration containingType,
        IReadOnlyDictionary<string, ParameterData> parameters,
        IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters,
        IReadOnlyList<IAttributeData> attributes)
            : base(propertyInfo, getterMethod, setterMethod, containingType, availableTypeParameters, attributes)
    {
        Parameters = parameters;
    }

    /// <inheritdoc/>
    public override string Id => MemberId.Of(this);

    /// <summary>
    /// Dictionary of index parameters, the keys represent parameter names.
    /// </summary>
    internal IReadOnlyDictionary<string, ParameterData> Parameters { get; }

    /// <inheritdoc/>
    IReadOnlyList<IParameterData> IParametricMemberData.Parameters => Parameters.Values
        .OrderBy(p => p.Position)
        .ToList();
}
