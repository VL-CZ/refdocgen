using RefDocGen.CodeElements.Abstract.Types;

namespace RefDocGen.CodeElements.Concrete.Types;

/// <summary>
/// Class representing name-related data of a type, including its name, namespace, and generic parameters (if present).
/// <para>
/// Doesn't include any type member data (such as fields, methods, etc.)
/// </para>
/// </summary>
internal record GenericTypeParameterNameData : ITypeNameData
{
    /// <summary>
    /// Dictionary of type parameters declared in the containing type; the keys represent type parameter names.
    /// </summary>
    private readonly IReadOnlyDictionary<string, TypeParameterDeclaration> declaredTypeParameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    /// <param name="declaredTypeParameters">Collection of type parameters declared in the containing type; the keys represent type parameter names.</param>
    public GenericTypeParameterNameData(Type type, IReadOnlyDictionary<string, TypeParameterDeclaration> declaredTypeParameters)
    {
        this.declaredTypeParameters = declaredTypeParameters;
        TypeObject = type;
    }

    /// <inheritdoc/>
    public Type TypeObject { get; }

    /// <inheritdoc/>
    public string ShortName => TypeObject.Name;

    /// <inheritdoc/>
    public string FullName => ShortName;

    /// <inheritdoc/>
    public string Id
    {
        get
        {
            if (IsArray)
            {
                string typeName = ShortName;
                int i = typeName.IndexOf('[');

                if (declaredTypeParameters.TryGetValue(typeName[..i], out var typeParameter))
                {
                    return "`" + typeParameter.Order + typeName[i..];
                }
                else
                {
                    return typeParameter.Name;
                }

            }
            else
            {
                if (declaredTypeParameters.TryGetValue(ShortName, out var typeParameter))
                {
                    return "`" + typeParameter.Order;
                }
                else
                {
                    return typeParameter.Name;
                }
            }
        }
    }


    /// <inheritdoc/>
    public string? Namespace => null;

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

}
