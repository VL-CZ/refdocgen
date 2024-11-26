using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Concrete.Types;


/// <summary>
/// Class representing the declaration of a generic type parameter.
/// </summary>
internal class TypeParameterDeclaration : ITypeParameterDeclaration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeParameterDeclaration"/> class.
    /// </summary>
    /// <param name="name">Name of the type parameter (e.g., 'TKey').</param>
    /// <param name="index">Index of the parameter in the declaring type's parameter collection.</param>
    public TypeParameterDeclaration(string name, int index)
    {
        Name = name;
        Index = index;
        DocComment = XmlDocElements.EmptyTypeParamWithName(name);
    }

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public int Index { get; }

    /// <inheritdoc/>
    public XElement DocComment { get; internal set; }
}
