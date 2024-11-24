using RefDocGen.CodeElements.Abstract.Types;

namespace RefDocGen.CodeElements.Concrete.Types;

/// <summary>
/// Class representing name-related data of a type, including its name, namespace, and generic parameters (if present).
/// <para>
/// Doesn't include any type member data (such as fields, methods, etc.)
/// </para>
/// <para>
/// Note: this class doesn't represent generic type parameters (see <see cref="GenericTypeParameterNameData"/>).
/// </para>
/// </summary>
internal class TypeNameData : ITypeNameData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    /// <param name="declaredTypeParameters">Collection of type parameters declared in the containing type; the keys represent type parameter names.</param>
    internal TypeNameData(Type type, IReadOnlyDictionary<string, TypeParameterDeclaration> declaredTypeParameters)
    {
        TypeObject = type;

        TypeParameters = TypeObject
            .GetGenericArguments()
            .Select(t => t.GetNameData(declaredTypeParameters))
            .ToArray();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    internal TypeNameData(Type type) : this(type, new Dictionary<string, TypeParameterDeclaration>())
    { }

    /// <inheritdoc/>
    public Type TypeObject { get; }

    /// <inheritdoc/>
    public string ShortName
    {
        get
        {
            string name = TypeObject.Name;

            // remove the backtick and number of generic arguments (if present)
            int backTickIndex = name.IndexOf('`');
            if (backTickIndex >= 0)
            {
                name = name[..backTickIndex];
            }

            // remove the reference suffix (if present)
            if (name.EndsWith('&'))
            {
                name = name[..^1];
            }

            return name;
        }
    }

    /// <inheritdoc/>
    public string FullName => TypeObject.Namespace is not null ? $"{TypeObject.Namespace}.{ShortName}" : ShortName;

    /// <inheritdoc/>
    public virtual string Id
    {
        get
        {
            string name = FullName;

            if (HasTypeParameters)
            {
                name = name + '{' + string.Join(",", TypeParameters.Select(p => p.Id)) + '}';
            }

            return name;
        }
    }

    /// <inheritdoc/>
    public bool HasTypeParameters => TypeObject.IsGenericType;

    /// <inheritdoc/>
    public string Namespace => TypeObject.Namespace ?? string.Empty;

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
}
