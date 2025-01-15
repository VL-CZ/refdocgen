using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Tools;

namespace RefDocGen.CodeElements.Concrete.Types.TypeName;

/// <summary>
/// Class representing name-related data of a type, including its name, namespace, and generic parameters (if present).
/// <para>
/// Doesn't include any type member data (such as fields, methods, etc.)
/// </para>
/// </summary>
/// <remarks>
/// <para>
/// Note: this class doesn't represent generic type parameters (see <see cref="GenericTypeParameterNameData"/>).
/// </para>
/// <para>
/// Note: this class doesn't repreesent declaration of a type (see <see cref="TypeDeclaration"/>).
/// </para>
/// </remarks>
internal class TypeNameData : TypeNameBaseData, ITypeNameData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    /// <param name="availableTypeParameters">Collection of type parameters declared in the containing type; the keys represent type parameter names.</param>
    internal TypeNameData(Type type, IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters) : base(type)
    {
        TypeParameters = TypeObject
            .GetGenericArguments()
            .Select(t => t.GetTypeNameData(availableTypeParameters))
            .ToArray();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    internal TypeNameData(Type type) : this(type, new Dictionary<string, TypeParameterData>())
    { }

    /// <inheritdoc/>
    public override string Id => TypeId.Of(this);

    /// <inheritdoc/>
    public override bool HasTypeParameters => TypeObject.IsGenericType;

    /// <inheritdoc/>
    public IReadOnlyList<ITypeNameData> TypeParameters { get; }

    /// <inheritdoc/>
    public bool IsArray => TypeObject.IsArray;

    /// <inheritdoc/>
    public bool IsVoid => TypeObject == typeof(void);

    /// <inheritdoc/>
    public bool IsPointer => TypeObject.IsPointer;

    /// <inheritdoc/>
    public bool IsGenericParameter => false;

    /// <inheritdoc/>
    public string TypeDeclarationId => TypeId.Of(this, true);
}
