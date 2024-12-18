using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Tools;
using System.Reflection;

namespace RefDocGen.CodeElements.Concrete.Members;

internal class IndexerData : PropertyData, IIndexerData
{
    internal IndexerData(PropertyInfo propertyInfo, IReadOnlyDictionary<string, TypeParameterData> declaredTypeParameters)
        : base(propertyInfo, declaredTypeParameters)
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
    IReadOnlyList<IParameterData> IExecutableMemberData.Parameters => Parameters;
}
