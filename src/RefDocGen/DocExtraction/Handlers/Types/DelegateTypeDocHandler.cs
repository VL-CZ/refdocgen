using RefDocGen.CodeElements.Concrete.Types.Delegate;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Types;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding delegate types.
/// </summary>
internal class DelegateTypeDocHandler : TypeDocHandler
{
    /// <summary>
    /// Adds documentation to the given delegate type.
    /// </summary>
    /// <param name="d">Delegate type to which the documentation is added.</param>
    /// <param name="docComment">Doc comment for the delegate.</param>
    internal void AddDocumentation(DelegateTypeData d, XElement docComment)
    {
        base.AddDocumentation(d, docComment);

        // add param doc comments
        var paramElements = docComment.Descendants(XmlDocIdentifiers.Param);

        foreach (var paramElement in paramElements)
        {
            AddParamDocumentation(d, paramElement);
        }
    }

    /// <summary>
    /// Add the doc comment to the corresponding delegate method parameter.
    /// </summary>
    /// <param name="d">The delegate containing the parameter.</param>
    /// <param name="docComment">XML node of the doc comment for the given delegate method parameter.</param>
    private void AddParamDocumentation(DelegateTypeData d, XElement docComment)
    {
        if (docComment.TryGetNameAttribute(out var nameAttr))
        {
            string paramName = nameAttr.Value;
            var parameter = d.Parameters.FirstOrDefault(p => p.Name == paramName);

            if (parameter is null)
            {
                // TODO: log parameter not found
                return;
            }

            parameter.DocComment = docComment;
        }
    }
}
