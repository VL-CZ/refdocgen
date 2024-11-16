using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Concrete.Members;

/// <summary>
/// Class representing data of a method.
/// </summary>
internal class MethodData : ExecutableMemberData, IMethodData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodData"/> class.
    /// </summary>
    /// <param name="methodInfo"><see cref="System.Reflection.MethodInfo"/> object representing the method.</param>
    internal MethodData(MethodInfo methodInfo, IReadOnlyDictionary<string, TypeParameterDeclaration> declaredTypeParameters)
        : base(methodInfo, declaredTypeParameters)
    {
        MethodInfo = methodInfo;
        ReturnType = new TypeNameData(methodInfo.ReturnType);
    }

    internal MethodData(MethodInfo methodInfo) : this(methodInfo, new Dictionary<string, TypeParameterDeclaration>())
    { }

    /// <inheritdoc/>
    public MethodInfo MethodInfo { get; }

    /// <inheritdoc/>
    public override string Name => MethodInfo.Name;

    /// <inheritdoc/>
    public ITypeNameData ReturnType { get; }

    /// <inheritdoc/>
    public XElement ReturnValueDocComment { get; internal set; } = XmlDocElementFactory.EmptyReturns;

    /// <inheritdoc/>
    public override bool OverridesAnotherMember => !MethodInfo.Equals(MethodInfo.GetBaseDefinition());
}
