using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Tools;

namespace RefDocGen.CodeElements.Concrete.Types.TypeName;

/// <summary>
/// Class representing name-related data of a type, including its name, namespace, and generic parameters (if present).
/// <para>
/// Doesn't include any type member data (such as fields, methods, etc.)
/// </para>
/// <para>
/// Note: this class doesn't represent generic type parameters (see <see cref="GenericTypeParameterNameData"/>).
/// </para>
/// </summary>
internal class TypeNameData : TypeNameBaseData, ITypeNameData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    /// <param name="declaredTypeParameters">Collection of type parameters declared in the containing type; the keys represent type parameter names.</param>
    internal TypeNameData(Type type, IReadOnlyDictionary<string, TypeParameterDeclaration> declaredTypeParameters) : base(type)
    {
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
    public override string Id
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
