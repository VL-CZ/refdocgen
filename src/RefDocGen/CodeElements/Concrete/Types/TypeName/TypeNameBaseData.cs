using RefDocGen.CodeElements.Abstract.Types;

namespace RefDocGen.CodeElements.Concrete.Types.TypeName;

/// <summary>
/// Class representing name-related data of any type, including its name, namespace.
/// <para>
/// Doesn't include any type parameters nor member data (such as fields, methods, etc.)
/// </para>
/// </summary>
internal abstract class TypeNameBaseData : ITypeNameBaseData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameBaseData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    protected TypeNameBaseData(Type type)
    {
        TypeObject = type;
    }

    /// <inheritdoc/>
    public Type TypeObject { get; }

    /// <inheritdoc/>
    public abstract string Id { get; }

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
    public string FullName => TypeObject.Namespace is not null
        ? $"{TypeObject.Namespace}.{ShortName}"
        : ShortName;

    /// <inheritdoc/>
    public string Namespace => TypeObject.Namespace ?? string.Empty;
}
