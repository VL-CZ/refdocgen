using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Tools;
using RefDocGen.Tools;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Runtime.CompilerServices;
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
    /// <param name="availableTypeParameters">Collection of type parameters declared in the containing type; the keys represent type parameter names.</param>
    /// <param name="containingType">Type that contains the member.</param>
    internal MethodData(MethodInfo methodInfo, TypeDeclaration containingType, IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters)
        : base(methodInfo, containingType)
    {
        MethodInfo = methodInfo;
        ReturnType = methodInfo.ReturnType.GetTypeNameData(availableTypeParameters);
        IsExtensionMethod = MethodInfo.IsDefined(typeof(ExtensionAttribute), true);

        // add type parameters
        TypeParameterDeclarations = methodInfo.GetGenericArguments()
                .Select((ga, i) => new TypeParameterData(ga, i, CodeElementKind.Member))
                .ToDictionary(t => t.Name);

        // add the dicitonaries
        var allParams = availableTypeParameters
            .Merge(TypeParameterDeclarations);

        // add parameters
        Parameters = methodInfo.GetParameters()
            .Select((p, index) =>
            {
                return new ParameterData(
                    p, allParams, IsExtensionMethod && index == 0);
            })
            .ToDictionary(t => t.Name);
    }

    /// <inheritdoc/>
    public override string Id => MemberId.Of(this);

    /// <inheritdoc/>
    public override string Name => MemberName.Of(this);

    /// <inheritdoc cref="MethodData(MethodInfo, TypeDeclaration, IReadOnlyDictionary{string, TypeParameterData})"/>
    internal MethodData(MethodInfo methodInfo, TypeDeclaration containingType) : this(methodInfo, containingType, new Dictionary<string, TypeParameterData>())
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
    public bool IsExtensionMethod { get; }

    /// <summary>
    /// Collection of type parameters declared in the member; the keys represent type parameter names.
    /// </summary>
    internal IReadOnlyDictionary<string, TypeParameterData> TypeParameterDeclarations { get; }

    /// <inheritdoc/>
    IReadOnlyList<ITypeParameterData> IMethodData.TypeParameters => TypeParameterDeclarations.Values
        .OrderBy(t => t.Index)
        .ToList();

    /// <inheritdoc/>
    internal override IReadOnlyDictionary<string, ParameterData> Parameters { get; }
}
