using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Concrete.Types;
using System.Reflection;

namespace RefDocGen.CodeElements.Concrete.Members;

internal class IndexerData : PropertyData, IIndexerData
{
    internal IndexerData(PropertyInfo propertyInfo, IReadOnlyDictionary<string, TypeParameterDeclaration> declaredTypeParameters)
        : base(propertyInfo, declaredTypeParameters)
    {
        // add parameters
        Parameters = propertyInfo.GetIndexParameters()
            .OrderBy(p => p.Position)
            .Select(p => new ParameterData(p, declaredTypeParameters))
            .ToArray();
    }

    /// <inheritdoc/>
    public override string Id
    {
        get
        {
            // Get the parameters in the format: System.String, System.Int32, etc.
            var parameterNames = Parameters.Select(
                        p => p.IsByRef ? p.Type.Id + "@" : p.Type.Id    // if the param is passed by reference, add '@' suffix
                    );

            return Name + "(" + string.Join(",", parameterNames) + ")";
        }
    }

    /// <summary>
    /// Array of index parameters, ordered by their position.
    /// </summary>
    public IReadOnlyList<ParameterData> Parameters { get; }

    /// <inheritdoc/>
    IReadOnlyList<IParameterData> IIndexerData.Parameters => Parameters;
}
