using RefDocGen.CodeElements.Types.Concrete;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Tools;

/// <summary>
/// Helper class used for documenting type parameters.
/// </summary>
internal class TypeParameterDocHelper
{
    /// <summary>
    /// Adds parameter doc comments to the corresponding type parameters.
    /// </summary>
    /// <param name="typeParamElements">Enumerable of doc comments for the type parameters. (i.e. a collection of 'typeparam' elements)</param>
    /// <param name="typeParams">A dictionary of type parameters (indexed by its name) to assign the doc comments to.</param>
    internal static void Add(IEnumerable<XElement> typeParamElements, IReadOnlyDictionary<string, TypeParameterData> typeParams)
    {
        foreach (var paramDocComment in typeParamElements)
        {
            if (paramDocComment.TryGetNameAttribute(out var nameAttr))
            {
                var typeParameter = typeParams.GetValueOrDefault(nameAttr.Value);

                if (typeParameter is null)
                {
                    return; // type parameter not found
                }

                typeParameter.DocComment = paramDocComment;
            }
        }
    }
}
