using RefDocGen.CodeElements.Concrete;
using RefDocGen.DocExtraction.Handlers.InheritDoc;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

/// <summary>
/// Class responsible for handling the 'inheritdoc' comments with 'cref' attribute and replacing them with the actual documentation.
/// </summary>
/// <remarks>
/// This class isn't intended to handle inheritdoc comments with 'cref' attribute (for further info, see <see cref="CrefInheritDocHandler"/>).
/// </remarks>
internal class CrefInheritDocHandler : InheritDocHandler<XElement>
{
    /// <inheritdoc/>
    public CrefInheritDocHandler(TypeRegistry typeRegistry) : base(typeRegistry)
    {
    }

    /// <inheritdoc/>
    protected override InheritDocKind InheritDocKind => InheritDocKind.Cref;

    /// <inheritdoc/>
    protected override IReadOnlyList<XElement> GetParentNodes(XElement member)
    {
        var nestedDocs = member.Descendants(XmlDocIdentifiers.InheritDoc);

        var neighbours = new List<XElement>();

        foreach (var nestedDoc in nestedDocs)
        {
            if (nestedDoc.Attribute(XmlDocIdentifiers.Cref) is XAttribute crefAttr)
            {
                string[] splitMemberName = crefAttr.Value.Split(':');
                (string objectIdentifier, string fullObjectName) = (splitMemberName[0], splitMemberName[1]);

                var neighbour = objectIdentifier == MemberTypeId.Type
                    ? (typeRegistry.GetDeclaredType(fullObjectName)?.RawDocComment)
                    : (typeRegistry.GetMember(fullObjectName)?.RawDocComment);

                if (neighbour is not null)
                {
                    neighbours.Add(neighbour);
                }
            }
        }

        return neighbours;
    }

    /// <inheritdoc/>
    protected override XElement? GetRawDocumentation(XElement member)
    {
        return member;
    }
}
