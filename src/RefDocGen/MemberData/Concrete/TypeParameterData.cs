using RefDocGen.MemberData.Abstract;

namespace RefDocGen.MemberData.Concrete;

/// <summary>
/// Class representing name-related data of a type, including its name, namespace, and generic parameters (if present).
/// <para>
/// Doesn't include any type member data (such as fields, methods, etc.)
/// </para>
/// </summary>
internal record TypeParameterNameData : ITypeNameData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    public TypeParameterNameData(Type type, int order)
    {
        TypeObject = type;
        Order = order;
    }

    /// <inheritdoc/>
    public Type TypeObject { get; }

    public int Order { get; }

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
    public virtual string Id => $"`{Order}";

    /// <inheritdoc/>
    public bool HasGenericParameters => false;

    /// <inheritdoc/>
    public string? Namespace => null;

    /// <inheritdoc/>
    public IReadOnlyList<ITypeNameData> GenericParameters => [];

    /// <inheritdoc/>
    public bool IsArray => false;

    /// <inheritdoc/>
    public bool IsVoid => false;

    /// <inheritdoc/>
    public bool IsPointer => false;
}
