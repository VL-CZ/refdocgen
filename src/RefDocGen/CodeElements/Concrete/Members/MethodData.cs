using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Tools;
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
    /// <param name="declaredTypeParameters">Collection of type parameters declared in the containing type; the keys represent type parameter names.</param>
    internal MethodData(MethodInfo methodInfo, TypeDeclaration declaringType, IReadOnlyDictionary<string, TypeParameterData> declaredTypeParameters)
        : base(methodInfo, declaringType, declaredTypeParameters)
    {
        MethodInfo = methodInfo;
        ReturnType = methodInfo.ReturnType.GetTypeNameData(declaredTypeParameters);

        ExplicitInterfaceType = Tools.ExplicitInterfaceType.Of(this);
    }

    /// <inheritdoc/>
    public override string Name => IsExplicitImplementation
        ? MethodInfo.Name.Split('.').Last()
        : MethodInfo.Name;

    /// <summary>
    /// Initializes a new instance of the <see cref="MethodData"/> class.
    /// </summary>
    /// <param name="methodInfo"><see cref="System.Reflection.MethodInfo"/> object representing the method.</param>
    internal MethodData(MethodInfo methodInfo, TypeDeclaration declaringType) : this(methodInfo, declaringType, new Dictionary<string, TypeParameterData>())
    { }

    /// <inheritdoc/>
    public MethodInfo MethodInfo { get; }

    /// <inheritdoc/>
    public ITypeNameData ReturnType { get; }

    /// <inheritdoc/>
    public XElement ReturnValueDocComment { get; internal set; } = XmlDocElements.EmptyReturns;

    /// <inheritdoc/>
    public override bool OverridesAnotherMember => !MethodInfo.Equals(MethodInfo.GetBaseDefinition());

    /// <inheritdoc/>
    public override bool IsConstructor => false;

    /// <inheritdoc/>
    public override ITypeNameData? ExplicitInterfaceType { get; }
}
