using RefDocGen.MemberData.Abstract;
using RefDocGen.Tools;
using System;

namespace RefDocGen.MemberData.Concrete;

/// <summary>
/// Class representing name-related data of a type, including its name, namespace, and generic parameters (if present).
/// <para>
/// Doesn't include any type member data (such as fields, methods, etc.)
/// </para>
/// </summary>
internal record TypeNameData : ITypeNameData
{
    private IReadOnlyList<TypeParameterDeclaration> declaredTypeParameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    public TypeNameData(Type type, IReadOnlyList<TypeParameterDeclaration> declaredTypeParameters)
    {
        this.declaredTypeParameters = declaredTypeParameters;
        TypeObject = type;

        GenericParameters = TypeObject
            .GetGenericArguments()
            .Select(t => new TypeNameData(t, declaredTypeParameters))
            .ToArray();
    }

    public TypeNameData(Type type) : this(type, []) { }

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
            if (TypeObject.IsGenericParameter)
            {
                if (declaredTypeParameters.Any(p => p.Name == ShortName))
                {
                    return "`" + declaredTypeParameters.First(p => p.Name == ShortName).Order;
                }
            }
            else if (IsArray && TypeObject.GetBaseElementType().IsGenericParameter)
            {
                string typeName = ShortName;
                int i = typeName.IndexOf('[');

                if (declaredTypeParameters.Any(p => p.Name == typeName[..i]))
                {
                    return "`" + declaredTypeParameters.First(p => p.Name == typeName[..i]).Order + typeName[i..];
                }

            }


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
