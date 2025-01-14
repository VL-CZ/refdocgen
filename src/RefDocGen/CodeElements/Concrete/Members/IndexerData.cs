using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
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
    /// <param name="containingType"> Type that contains the member.</param>
    /// <param name="availableTypeParameters"></param>
    internal IndexerData(PropertyInfo propertyInfo, TypeDeclaration containingType, IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters)
        : base(propertyInfo, containingType, availableTypeParameters)
    {
        // add parameters
        Parameters = propertyInfo.GetIndexParameters()
            .Select(p => new ParameterData(p, availableTypeParameters))
            .ToDictionary(p => p.Name);
    }

    /// <inheritdoc/>
    public override string Id => MemberId.Of(this);

    /// <summary>
    /// Dictionary of index parameters, the keys represent parameter names.
    /// </summary>
    internal IReadOnlyDictionary<string, ParameterData> Parameters { get; }

    /// <inheritdoc/>
    public override ITypeNameData? ExplicitInterfaceType => null;

    /// <inheritdoc/>
    IReadOnlyList<IParameterData> IExecutableMemberData.Parameters => Parameters.Values
        .OrderBy(p => p.Position)
        .ToList();
}
