using RefDocGen.CodeElements.Abstract.Types;

namespace RefDocGen.CodeElements.Concrete.Types;

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
    public virtual string ShortName => TypeObject.Name;

    /// <inheritdoc/>
    public string FullName => TypeObject.Namespace is not null
        ? $"{TypeObject.Namespace}.{ShortName}"
        : ShortName;

    /// <inheritdoc/>
    public string Namespace => TypeObject.Namespace ?? string.Empty;
}
