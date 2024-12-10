using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
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

        BaseType = type.BaseType is not null
            ? new TypeNameData(type.BaseType)
            : null;

        Interfaces = type.GetInterfaces()
            .Select(i => new TypeNameData(i))
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
    public XElement DocComment { get; internal set; } = XmlDocElements.EmptySummary;

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
        .Select(tp => tp.TypeObject.GetNameData(TypeParameterDeclarations))
        .ToList();

    /// <inheritdoc/>
    bool ITypeNameData.IsPointer => false;
}
