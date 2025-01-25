using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Tools;
using System.Reflection;

namespace RefDocGen.CodeElements.Concrete.Types;

/// <inheritdoc/>
internal class AttributeData : IAttributeData
{
    /// <summary>
    /// Creates a new instance of <see cref="AttributeData"/> class.
    /// </summary>
    /// <param name="attr"><inheritdoc cref="CustomAttributeData"/></param>
    /// <param name="availableTypeParameters">Collection of the type parameters declared in the type; the keys represent type parameter names.</param>
    internal AttributeData(CustomAttributeData attr, IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters)
    {
        CustomAttributeData = attr;
        Type = attr.AttributeType.GetTypeNameData(availableTypeParameters);

        ConstructorArgumentValues = attr.ConstructorArguments
            .Select(a => a.Value)
            .ToArray();

        NamedArguments = attr.NamedArguments
            .Select(a => new NamedAttributeArgument(a.MemberName, a.TypedValue.Value))
            .ToArray();
    }

    /// <inheritdoc/>
    public IReadOnlyList<object?> ConstructorArgumentValues { get; }

    /// <inheritdoc/>
    public IReadOnlyList<NamedAttributeArgument> NamedArguments { get; }

    /// <inheritdoc/>
    public ITypeNameData Type { get; }

    /// <inheritdoc/>
    public CustomAttributeData CustomAttributeData { get; }
}
