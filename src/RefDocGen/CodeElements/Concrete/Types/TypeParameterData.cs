using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Concrete.Types;

/// <summary>
/// Class representing the declaration of a generic type parameter.
/// </summary>
internal class TypeParameterData : ITypeParameterData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeParameterData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type parameter.</param>
    /// <param name="index">Index of the parameter in the declaring type's parameter collection.</param>
    public TypeParameterData(Type type, int index)
    {
        TypeObject = type;
        Index = index;
        DocComment = XmlDocElements.EmptyTypeParamWithName(Name);
    }

    /// <inheritdoc/>
    public Type TypeObject { get; }

    /// <inheritdoc/>
    public string Name => TypeObject.Name;

    /// <inheritdoc/>
    public int Index { get; }

    /// <inheritdoc/>
    public XElement DocComment { get; internal set; }

    /// <inheritdoc/>
    public bool IsCovariant => TypeObject.GenericParameterAttributes.HasFlag(GenericParameterAttributes.Covariant);

    /// <inheritdoc/>
    public bool IsContravariant => TypeObject.GenericParameterAttributes.HasFlag(GenericParameterAttributes.Contravariant);
}
