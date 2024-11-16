using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.Tools;

namespace RefDocGen.CodeElements.Concrete.Types;

/// <summary>
/// Class representing name-related data of a generic parameter.
/// </summary>
internal record GenericTypeParameterNameData : ITypeNameData
{
    /// <summary>
    /// Dictionary of type parameters declared in the containing type; the keys represent type parameter names.
    /// </summary>
    private readonly IReadOnlyDictionary<string, TypeParameterDeclaration> declaredTypeParameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericTypeParameterNameData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    /// <param name="declaredTypeParameters">Collection of type parameters declared in the containing type; the keys represent type parameter names.</param>
    internal GenericTypeParameterNameData(Type type, IReadOnlyDictionary<string, TypeParameterDeclaration> declaredTypeParameters)
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
            string paramName = ShortName;
            string idSuffix = "";

            if (IsArray) // Array -> We need to split the type name into 2 parts: parameter name and brackets
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

            return declaredTypeParameters.TryGetValue(paramName, out var typeParameter)
                ? "`" + typeParameter.Order + idSuffix // type found -> use its Order
                : "`" + typeParameter.Name + idSuffix; // type not found -> use arbitrary value (e.g. Name)
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
