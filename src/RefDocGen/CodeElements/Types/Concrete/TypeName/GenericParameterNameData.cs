using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.CodeElements.Types.Tools;

namespace RefDocGen.CodeElements.Types.Concrete.TypeName;

/// <summary>
/// Class representing name-related data of a generic parameter.
/// </summary>
internal class GenericTypeParameterNameData : ITypeNameData
{
    /// <summary>
    /// Dictionary of type parameters declared in the containing type; the keys represent type parameter names.
    /// </summary>
    internal readonly IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericTypeParameterNameData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    /// <param name="availableTypeParameters">Collection of type parameters declared in the containing type; the keys represent type parameter names.</param>
    internal GenericTypeParameterNameData(Type type, IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters)
    {
        this.availableTypeParameters = availableTypeParameters;
        TypeObject = type;
    }

    /// <inheritdoc/>
    public Type TypeObject { get; }

    /// <inheritdoc/>
    public string ShortName => TypeObject.Name;

    /// <inheritdoc/>
    public string FullName => ShortName;

    /// <inheritdoc/>
    public string Id => TypeId.Of(this);

    /// <inheritdoc/>
    public string Namespace => string.Empty;

    /// <inheritdoc/>
    public bool IsArray => TypeObject.IsArray;

    /// <inheritdoc/>
    public bool IsVoid => false;

    /// <inheritdoc/>
    public bool IsPointer => TypeObject.IsPointer;

    /// <inheritdoc/>
    public bool HasTypeParameters => false;

    /// <inheritdoc/>
    public IReadOnlyList<ITypeNameData> TypeParameters => [];

    /// <inheritdoc/>
    public bool IsGenericParameter => true;

    /// <inheritdoc/>
    ITypeNameData? ITypeNameData.DeclaringType => null;

    /// <inheritdoc/>
    public string TypeDeclarationId => FullName;
}
