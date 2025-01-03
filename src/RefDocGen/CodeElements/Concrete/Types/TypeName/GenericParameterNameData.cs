using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.Tools;

namespace RefDocGen.CodeElements.Concrete.Types.TypeName;

/// <summary>
/// Class representing name-related data of a generic parameter.
/// </summary>
internal class GenericTypeParameterNameData : ITypeNameData
{
    /// <summary>
    /// Dictionary of type parameters declared in the containing type; the keys represent type parameter names.
    /// </summary>
    private readonly IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters;

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
    public string Id
    {
        get
        {
            string paramName = ShortName;
            string idSuffix = "";

            if (IsArray) // Array -> We need to split the type name into 2 parts: type parameter name and brackets
            {
                if (paramName.TryGetIndex('[', out int i))
                {
                    (paramName, idSuffix) = (paramName[..i], paramName[i..]);
                }
            }
            else if (IsPointer) // Pointer -> equivalent to Array type
            {
                if (paramName.TryGetIndex('*', out int i))
                {
                    (paramName, idSuffix) = (paramName[..i], paramName[i..]);
                }
            }

            if (availableTypeParameters.TryGetValue(paramName, out var typeParameter))
            {
                // We need to get the index of the generic parameter, for further info see:
                // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments#d42-id-string-format

                string idPrefix = typeParameter.DeclaredAt == CodeElementKind.Type
                    ? "`" // declared in a type -> single backtick
                    : "``"; // declared in a member -> double backtick

                return idPrefix + typeParameter.Index + idSuffix; // generic param found -> use its index
            }
            else
            {
                return paramName + idSuffix; // generic param not found -> use its name
            }
        }
    }

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

    public string IdWithParameterNames => throw new NotImplementedException();
}
