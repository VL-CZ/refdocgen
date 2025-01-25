using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Attribute;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types.TypeName;
using RefDocGen.CodeElements.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Concrete.Types;

/// <inheritdoc cref="ITypeDeclaration"/>
internal abstract class TypeDeclaration : TypeNameBaseData, ITypeDeclaration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameBaseData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    /// <param name="typeParameters">Collection of the type parameters declared in the type; the keys represent type parameter names.</param>
    /// <param name="attributes">Collection of attributes applied to the type.</param>
    protected TypeDeclaration(Type type, IReadOnlyDictionary<string, TypeParameterData> typeParameters, IReadOnlyList<IAttributeData> attributes)
        : base(type)
    {
        TypeParameters = typeParameters;
        Attributes = attributes;

        BaseType = type.BaseType?.GetTypeNameData(typeParameters);

        Interfaces = type.GetInterfaces()
            .Select(i => i.GetTypeNameData(typeParameters))
            .ToArray();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameBaseData"/> class with no type parameters.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    /// <param name="attributes">Collection of attributes applied to the type.</param>
    protected TypeDeclaration(Type type, IReadOnlyList<IAttributeData> attributes)
        : this(type, new Dictionary<string, TypeParameterData>(), attributes)
    {
    }

    /// <inheritdoc/>
    public AccessModifier AccessModifier =>
        AccessModifierExtensions.GetAccessModifier(
            TypeObject.IsNestedPrivate,
            TypeObject.IsNestedFamily,
            TypeObject.IsNestedAssembly || TypeObject.IsNotPublic,
            TypeObject.IsPublic || TypeObject.IsNestedPublic,
            TypeObject.IsNestedFamANDAssem,
            TypeObject.IsNestedFamORAssem);

    /// <summary>
    /// Identifier of the type. Consists of fully qualified type name, backtick symbol and type parameters count
    /// <example>
    /// Example: for <c>MyNamespace.MyClass&lt;T1,T2&gt;</c> it returns
    /// <code>
    /// MyNamespace.MyClass`2
    /// </code>
    /// </example>
    /// </summary>
    public override string Id => TypeId.Of(this);

    /// <inheritdoc/>
    public XElement SummaryDocComment { get; internal set; } = XmlDocElements.EmptySummary;

    /// <inheritdoc/>
    public XElement RemarksDocComment { get; internal set; } = XmlDocElements.EmptyRemarks;

    /// <inheritdoc/>
    public IEnumerable<XElement> SeeAlsoDocComments { get; internal set; } = [];

    /// <inheritdoc/>
    public override bool HasTypeParameters => TypeParameters.Count > 0;

    /// <inheritdoc/>
    public ITypeNameData? BaseType { get; }

    /// <inheritdoc/>
    public IReadOnlyList<ITypeNameData> Interfaces { get; }

    /// <summary>
    /// Collection of type parameters declared in this type; the keys represent type parameter names.
    /// </summary>
    public IReadOnlyDictionary<string, TypeParameterData> TypeParameters { get; }

    /// <inheritdoc/>
    IReadOnlyList<ITypeParameterData> ITypeDeclaration.TypeParameters =>
        TypeParameters.Values
        .OrderBy(t => t.Index)
        .ToList();

    /// <summary>
    /// Dictionary of all members declared in the type; keys are the corresponding member IDs.
    /// </summary>
    internal abstract IReadOnlyDictionary<string, MemberData> AllMembers { get; private protected set; }

    /// <summary>
    /// Raw doc comment provided to the type.
    ///
    /// <para>
    /// <see langword="null"/> if the type isn't documented.
    /// </para>
    /// </summary>
    internal XElement? RawDocComment { get; set; }

    /// <inheritdoc/>
    public IReadOnlyList<IAttributeData> Attributes { get; }
}
