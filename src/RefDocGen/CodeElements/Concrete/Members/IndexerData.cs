using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Tools;
using System.Reflection;

namespace RefDocGen.CodeElements.Concrete.Members;

internal class IndexerData : PropertyData, IIndexerData
{
    internal IndexerData(PropertyInfo propertyInfo, TypeDeclaration declaringType, IReadOnlyDictionary<string, TypeParameterData> declaredTypeParameters)
        : base(propertyInfo, declaringType, declaredTypeParameters)
    {
        // add parameters
        Parameters = propertyInfo.GetIndexParameters()
            .OrderBy(p => p.Position)
            .Select(p => new ParameterData(p, declaredTypeParameters))
            .ToArray();
    }

    /// <inheritdoc/>
    public override string Id => MemberId.Of(this);

    /// <summary>
    /// Array of index parameters, ordered by their position.
    /// </summary>
    public IReadOnlyList<ParameterData> Parameters { get; }

    /// <inheritdoc/>
    public IReadOnlyList<ITypeParameterData> TypeParameters => [];

    /// <inheritdoc/>
    public override ITypeNameData? ExplicitInterfaceType => null;

    /// <inheritdoc/>
    public bool IsConstructor => false;

    /// <inheritdoc/>
    IReadOnlyList<IParameterData> IExecutableMemberData.Parameters => Parameters;
}
