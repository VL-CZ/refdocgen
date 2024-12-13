using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Tools;
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

    /// <inheritdoc/>
    public IEnumerable<ITypeNameData> Constraints => TypeObject.GetGenericParameterConstraints().Select(p => p.GetNameData());

    /// <inheritdoc/>
    public IEnumerable<string> SpecialConstraints
    {
        get
        {
            List<SpecialConstraint> values = [];
            if (TypeObject.GenericParameterAttributes.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint))
            {
                values.Add(SpecialConstraint.Class);
            }
            if (TypeObject.GenericParameterAttributes.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint))
            {
                values.Add(SpecialConstraint.Struct);
            }
            if (TypeObject.GenericParameterAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
            {
                values.Add(SpecialConstraint.New);
            }

            return values.Select(v => v.ToString());
        }
    }
}

enum SpecialConstraint { Class, Struct, New }
