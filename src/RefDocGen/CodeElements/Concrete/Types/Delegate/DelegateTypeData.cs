using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Delegate;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Concrete.Types.Delegate;

/// <summary>
/// Class representing data of a delegate.
/// </summary>
internal class DelegateTypeData : IDelegateTypeData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateTypeData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the delegate.</param>
    public DelegateTypeData(Type type)
    {
        TypeObject = type;
    }

    /// <inheritdoc/>
    public Type TypeObject { get; }

    /// <inheritdoc/>
    public string Id => FullName;

    /// <inheritdoc/>
    public string ShortName => TypeObject.Name;

    /// <inheritdoc/>
    public string FullName => TypeObject.Namespace is not null ? $"{TypeObject.Namespace}.{ShortName}" : ShortName;

    /// <inheritdoc/>
    public string Namespace => TypeObject.Namespace ?? string.Empty;

    /// <inheritdoc/>
    public XElement DocComment { get; internal set; } = XmlDocElements.EmptySummary;

    /// <summary>
    /// Access modifier of the enum.
    /// </summary>
    public AccessModifier AccessModifier =>
        AccessModifierExtensions.GetAccessModifier(
            TypeObject.IsNestedPrivate,
            TypeObject.IsNestedFamily,
            TypeObject.IsNestedAssembly || TypeObject.IsNotPublic,
            TypeObject.IsPublic || TypeObject.IsNestedPublic,
            TypeObject.IsNestedFamANDAssem,
            TypeObject.IsNestedFamORAssem);

    public ITypeNameData ReturnType => throw new NotImplementedException();

    public XElement ReturnValueDocComment { get; internal set; } = XmlDocElements.EmptySummary;

    public IReadOnlyList<IParameterData> Parameters => throw new NotImplementedException();

    public bool HasTypeParameters => throw new NotImplementedException();

    public bool IsGenericParameter => throw new NotImplementedException();

    public bool IsArray => false;

    public bool IsVoid => false;

    public IReadOnlyList<ITypeNameData> TypeParameters => throw new NotImplementedException();

    public bool IsPointer => false;
}
