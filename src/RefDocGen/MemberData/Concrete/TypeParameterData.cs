//using RefDocGen.MemberData.Abstract;

//namespace RefDocGen.MemberData.Concrete;

///// <summary>
///// Class representing name-related data of a type, including its name, namespace, and generic parameters (if present).
///// <para>
///// Doesn't include any type member data (such as fields, methods, etc.)
///// </para>
///// </summary>
//internal record TypeParameterNameData : ITypeNameData
//{
//    private IReadOnlyList<TypeParameterNameData> declaredTypeParameters;

//    /// <summary>
//    /// Initializes a new instance of the <see cref="TypeNameData"/> class.
//    /// </summary>
//    /// <param name="type"><see cref="Type"/> object representing the type.</param>
//    public TypeParameterNameData(Type type, IReadOnlyList<TypeParameterNameData> declaredTypeParameters)
//    {
//        this.declaredTypeParameters = declaredTypeParameters;
//        TypeObject = type;
//    }

//    /// <inheritdoc/>
//    public Type TypeObject { get; }

//    /// <inheritdoc/>
//    public string ShortName => TypeObject.Name;

//    /// <inheritdoc/>
//    public string FullName => TypeObject.Namespace is not null ? $"{TypeObject.Namespace}.{ShortName}" : ShortName;

//    /// <inheritdoc/>
//    public virtual string Id => $"`0";

//    /// <inheritdoc/>
//    public bool HasGenericParameters => false;

//    /// <inheritdoc/>
//    public string? Namespace => null;

//    /// <inheritdoc/>
//    public IReadOnlyList<ITypeNameData> GenericParameters => [];

//    /// <inheritdoc/>
//    public bool IsArray => false;

//    /// <inheritdoc/>
//    public bool IsVoid => false;

//    /// <inheritdoc/>
//    public bool IsPointer => false;
//}
