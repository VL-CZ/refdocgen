using RefDocGen.CodeElements.Concrete.Members;
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
    /// <param name="parameters">A collection of parameters to assign the doc comments to.</param>
    internal static void Add(IEnumerable<XElement> paramElements, IReadOnlyList<ParameterData> parameters)
    {
        foreach (var paramDocComment in paramElements)
        {
            if (paramDocComment.TryGetNameAttribute(out var nameAttr))
            {
                string paramName = nameAttr.Value;
                var parameter = parameters.FirstOrDefault(p => p.Name == paramName);

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
