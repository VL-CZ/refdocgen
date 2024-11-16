using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.Tools;

namespace RefDocGen.CodeElements.Concrete.Types;

/// <summary>
/// Class representing name-related data of a type, including its name, namespace, and generic parameters (if present).
/// <para>
/// Doesn't include any type member data (such as fields, methods, etc.)
/// </para>
/// </summary>
internal record TypeNameData : ITypeNameData
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
    public TypeNameData(Type type, IReadOnlyDictionary<string, TypeParameterDeclaration> declaredTypeParameters)
    {
        this.declaredTypeParameters = declaredTypeParameters;
        TypeObject = type;

        TypeParameters = TypeObject
            .GetGenericArguments()
            .Select(t => new TypeNameData(t, declaredTypeParameters))
            .ToArray();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    public TypeNameData(Type type) : this(type, new Dictionary<string, TypeParameterDeclaration>())
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
            if (IsGenericParameter)
            {
                if (declaredTypeParameters.TryGetValue(ShortName, out var typeParameter))
                {
                    return "`" + typeParameter.Order;
                }
            }
            else if (IsArray && TypeObject.GetBaseElementType().IsGenericParameter)
            {
                string typeName = ShortName;
                int i = typeName.IndexOf('[');

                if (declaredTypeParameters.TryGetValue(typeName[..i], out var typeParameter))
                {
                    return "`" + typeParameter.Order + typeName[i..];
                }

            }


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
    public string? Namespace => TypeObject.Namespace;

    /// <inheritdoc/>
    public IReadOnlyList<ITypeNameData> TypeParameters { get; }

    /// <inheritdoc/>
    public bool IsArray => TypeObject.IsArray;

    /// <inheritdoc/>
    public bool IsVoid => TypeObject == typeof(void);

    /// <inheritdoc/>
    public bool IsPointer => TypeObject.IsPointer;

    /// <inheritdoc/>
    public bool IsGenericParameter => TypeObject.IsGenericParameter;
}
