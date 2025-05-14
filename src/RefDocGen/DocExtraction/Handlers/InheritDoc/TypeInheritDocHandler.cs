using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Types.Concrete;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.InheritDoc;

/// <summary>
/// Class responsible for handling the 'inheritdoc' comments provided to types and replacing them with the actual documentation.
/// </summary>
/// <remarks>
/// This class isn't intended to handle inheritdoc comments with 'cref' attribute (for further info, see <see cref="InheritDocCrefHandler"/>).
/// </remarks>
internal class TypeInheritDocHandler : InheritDocHandler<TypeDeclaration>
{
    /// <inheritdoc/>
    public TypeInheritDocHandler(TypeRegistry typeRegistry) : base(typeRegistry)
    {
    }

    /// <inheritdoc/>
    protected override InheritDocKind InheritDocKind => InheritDocKind.NonCref;

    /// <inheritdoc/>
    protected override IReadOnlyList<TypeDeclaration> GetParentNodes(TypeDeclaration node)
    {
        return typeRegistry.GetDeclaredBaseTypes(node);
    }

    /// <inheritdoc/>
    protected override XElement? GetRawDocumentation(TypeDeclaration node)
    {
        return node.RawDocComment;
    }
}
