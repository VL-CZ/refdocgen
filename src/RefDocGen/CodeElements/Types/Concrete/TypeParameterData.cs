using RefDocGen.CodeElements.Shared;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Types.Concrete;

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
    /// <param name="declaredAt">Kind of the code elements, where the type parameter is declared.</param>
    public TypeParameterData(Type type, int index, CodeElementKind declaredAt)
    {
        TypeObject = type;
        Index = index;
        DeclaredAt = declaredAt;

        DocComment = XmlDocElements.EmptyTypeParamWithName(Name);

        TypeConstraints = TypeObject
            .GetGenericParameterConstraints()
            .Except([typeof(ValueType)]) // exclude `NotNullableValueType` constraint, which is a part of special constraints
            .Select(p => p.GetTypeNameData());
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
    public IEnumerable<ITypeNameData> TypeConstraints { get; }

    /// <inheritdoc/>
    public IEnumerable<SpecialTypeConstraint> SpecialConstraints
    {
        get
        {
            List<SpecialTypeConstraint> values = [];

            if (TypeObject.GenericParameterAttributes.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint))
            {
                values.Add(SpecialTypeConstraint.ReferenceType);
            }
            if (TypeObject.GenericParameterAttributes.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint))
            {
                values.Add(SpecialTypeConstraint.NotNullableValueType);
            }
            if (TypeObject.GenericParameterAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint)
                && !values.Contains(SpecialTypeConstraint.NotNullableValueType))
            {
                values.Add(SpecialTypeConstraint.DefaultConstructor);
            }

            return values;
        }
    }

    /// <inheritdoc/>
    public CodeElementKind DeclaredAt { get; }
}
