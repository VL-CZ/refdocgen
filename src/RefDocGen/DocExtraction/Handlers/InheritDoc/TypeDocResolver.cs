//using RefDocGen.CodeElements.Concrete;
//using RefDocGen.CodeElements.Concrete.Types;
//using System.Xml.Linq;

//namespace RefDocGen.DocExtraction.Handlers.InheritDoc;

//internal record TypeNode(TypeDeclaration Type);

///// <summary>
///// Class responsible for handling the 'inheritdoc' comments and replacing them with the actual documentation.
///// </summary>
//internal class TypeDocResolver : InheritDocResolver<TypeNode>
//{
//    /// <inheritdoc/>
//    public TypeDocResolver(TypeRegistry typeRegistry) : base(typeRegistry)
//    {
//    }

//    protected override XElement? GetRawDocumentation(TypeNode node)
//    {
//        return node.Type.RawDocComment;
//    }

//    /// <inheritdoc/>
//    protected override List<TypeNode> GetParentNodes(TypeNode node)
//    {
//        return GetDeclaredParents(node.Type)
//            .Select(p => new TypeNode(p))
//            .ToList();
//    }
//}
