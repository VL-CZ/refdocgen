using RefDocGen.CodeElements.Members.Concrete;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Tools;

/// <summary>
/// Helper class used for documenting parameters.
/// </summary>
internal class ParameterDocHelper
{
    /// <summary>
    /// Adds parameter doc comments to the corresponding parameters.
    /// </summary>
    /// <param name="paramElements">Enumerable of doc comments for the parameters. (i.e. a collection of 'param' elements)</param>
    /// <param name="parameters">A dictionary of parameters (indexed by its name) to assign the doc comments to.</param>
    internal static void Add(IEnumerable<XElement> paramElements, IReadOnlyDictionary<string, ParameterData> parameters)
    {
        foreach (var paramDocComment in paramElements)
        {
            if (paramDocComment.TryGetNameAttribute(out var nameAttr))
            {
                string paramName = nameAttr.Value;
                var parameter = parameters.GetValueOrDefault(paramName);

                if (parameter is null)
                {
                    // TODO: log parameter not found
                    return;
                }

                parameter.DocComment = paramDocComment;

            }
        }
    }
}
