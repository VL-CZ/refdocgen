using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types.TypeName;
using RefDocGen.CodeElements.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Concrete.Types;

internal abstract class TypeDeclaration : TypeNameBaseData, ITypeDeclaration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameBaseData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    /// <param name="typeParameterDeclarations">Collection of the type parameters declared in the type; the keys represent type parameter names.</param>
    protected TypeDeclaration(Type type, IReadOnlyDictionary<string, TypeParameterData> typeParameterDeclarations) : base(type)
    {
        TypeParameterDeclarations = typeParameterDeclarations;

        BaseType = type.BaseType?.GetTypeNameData(typeParameterDeclarations);

        Interfaces = type.GetInterfaces()
            .Select(i => i.GetTypeNameData(typeParameterDeclarations))
            .ToArray();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameBaseData"/> class with no type parameters.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    protected TypeDeclaration(Type type)
        : this(type, new Dictionary<string, TypeParameterData>())
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

    /// <inheritdoc/>
    public override string Id => TypeId.Of(this);

    /// <inheritdoc/>
    public XElement SummaryDocComment { get; internal set; } = XmlDocElements.EmptySummary;

    /// <inheritdoc/>
    public XElement RemarksDocComment { get; internal set; } = XmlDocElements.EmptyRemarks;

    /// <inheritdoc/>
    public IEnumerable<XElement> SeeAlsoDocComments { get; internal set; } = [];

    /// <inheritdoc/>
    public bool HasTypeParameters => TypeParameterDeclarations.Count > 0;

    /// <inheritdoc/>
    public ITypeNameData? BaseType { get; }

    /// <inheritdoc/>
    public IReadOnlyList<ITypeNameData> Interfaces { get; }

    /// <summary>
    /// Collection of type parameters declared in this type; the keys represent type parameter names.
    /// </summary>
    public IReadOnlyDictionary<string, TypeParameterData> TypeParameterDeclarations { get; }

    /// <inheritdoc/>
    IReadOnlyList<ITypeParameterData> ITypeDeclaration.TypeParameterDeclarations =>
        TypeParameterDeclarations.Values.OrderBy(t => t.Index).ToList();

    /// <inheritdoc/>
    bool ITypeNameData.IsGenericParameter => false;

    /// <inheritdoc/>
    bool ITypeNameData.IsArray => false;

    /// <inheritdoc/>
    bool ITypeNameData.IsVoid => false;

    /// <inheritdoc/>
    IReadOnlyList<ITypeNameData> ITypeNameData.TypeParameters => TypeParameterDeclarations.Values
        .OrderBy(t => t.Index)
        .Select(tp => tp.TypeObject.GetTypeNameData(TypeParameterDeclarations))
        .ToList();

    /// <inheritdoc/>
    bool ITypeNameData.IsPointer => false;

    /// <summary>
    /// Dictionary of all members declared in the type; keys are the corresponding member IDs.
    /// </summary>
    internal abstract IReadOnlyDictionary<string, MemberData> AllMembers { get; }

    /// <summary>
    /// Raw doc comment provided to the type.
    ///
    /// <para>
    /// <see langword="null"/> if the type isn't documented.
    /// </para>
    /// </summary>
    internal XElement? RawDocComment { get; set; }
}
