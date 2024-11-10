using RefDocGen.MemberData.Abstract;

namespace RefDocGen.MemberData.Concrete;

/// <summary>
/// Class representing name-related data of a type, including its name, namespace, and generic parameters (if present).
/// <para>
/// Doesn't include any type member data (such as fields, methods, etc.)
/// </para>
/// </summary>
internal class TypeNameData : ITypeNameData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    public TypeNameData(Type type)
    {
        TypeObject = type;
        GenericParameters = TypeObject
            .GetGenericArguments()
            .Select(t => new TypeNameData(t))
            .ToArray();
    }

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
    public string Id
    {
        get
        {
            string name = FullName;

            if (HasGenericParameters)
            {
                name = name + '{' + string.Join(",", GenericParameters.Select(p => p.Id)) + '}';
            }

            return name.Replace('&', '@');
        }
    }

    /// <inheritdoc/>
    public bool HasGenericParameters => TypeObject.IsGenericType;

    /// <inheritdoc/>
    public string? Namespace => TypeObject.Namespace;

    /// <inheritdoc/>
    public IReadOnlyList<ITypeNameData> GenericParameters { get; }

    /// <inheritdoc/>
    public bool IsArray => TypeObject.IsArray;

    /// <inheritdoc/>
    public bool IsVoid => TypeObject == typeof(void);

    /// <inheritdoc/>
    public bool IsPointer => TypeObject.IsPointer;
}
