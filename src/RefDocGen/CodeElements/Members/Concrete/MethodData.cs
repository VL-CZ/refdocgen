using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Members.Tools;
using RefDocGen.CodeElements.Shared;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Attribute;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.CodeElements.Types.Concrete;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Members.Concrete;

/// <summary>
/// Class representing data of a method.
/// </summary>
internal class MethodData : MethodLikeMemberData, IMethodData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodData"/> class.
    /// </summary>
    /// <param name="methodInfo"><see cref="System.Reflection.MethodInfo"/> object representing the method.</param>
    /// <param name="typeParameterDeclarations">Collection of the type parameters declared by the method.</param>
    /// <param name="availableTypeParameters">
    /// Collection of type parameters declared in the containing type; the keys represent type parameter names.
    /// Includes <paramref name="typeParameterDeclarations"/>.
    /// </param>
    /// <param name="containingType">Type that contains the member.</param>
    /// <param name="attributes">Collection of attributes applied to the method.</param>
    /// <param name="parameters">Dictionary of method parameters, the keys represent parameter names.</param>
    internal MethodData(
        MethodInfo methodInfo,
        TypeDeclaration containingType,
        IReadOnlyDictionary<string, ParameterData> parameters,
        IReadOnlyDictionary<string, TypeParameterData> typeParameterDeclarations,
        IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters,
        IReadOnlyList<IAttributeData> attributes) : base(methodInfo, containingType, parameters, attributes)
    {
        MethodInfo = methodInfo;
        ReturnType = methodInfo.ReturnType.GetTypeNameData(availableTypeParameters);
        TypeParameters = typeParameterDeclarations;
        IsExtensionMethod = MethodInfo.IsDefined(typeof(ExtensionAttribute), true);

        if (OverridesAnotherMember) // set base declaring type
        {
            BaseDeclaringType = methodInfo.GetBaseDefinition().DeclaringType?.GetTypeNameData();
        }

        if (!ContainingType.IsInterface && !IsExplicitImplementation)
        {
            var interfaceTypes = new List<ITypeNameData>();

            foreach (var interfaceType in ContainingType.Interfaces)
            {
                var interfaceMap = ContainingType.TypeObject.GetInterfaceMap(interfaceType.TypeObject);
                if (interfaceMap.TargetMethods.Contains(MethodInfo))
                {
                    interfaceTypes.Add(interfaceType);
                }
            }

            ImplementedInterfaces = interfaceTypes; // set implemented interfaces
        }
    }

    /// <inheritdoc/>
    public override string Id => MemberId.Of(this);

    /// <inheritdoc/>
    public override string Name => MemberName.Of(this);

    /// <inheritdoc/>
    public MethodInfo MethodInfo { get; }

    /// <inheritdoc/>
    public ITypeNameData ReturnType { get; }

    /// <inheritdoc/>
    public XElement ReturnValueDocComment { get; internal set; } = XmlDocElements.EmptyReturns;

    /// <inheritdoc/>
    public bool OverridesAnotherMember => !MethodInfo.Equals(MethodInfo.GetBaseDefinition()) && !IsInherited;

    /// <inheritdoc/>
    public bool IsOverridable => MethodInfo.IsVirtual && !MethodInfo.IsFinal;

    /// <inheritdoc/>
    public bool IsAbstract => MethodInfo.IsAbstract;

    /// <inheritdoc/>
    public bool IsFinal => MethodInfo.IsFinal;

    /// <inheritdoc/>
    public bool IsAsync => MethodInfo.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null;

    /// <inheritdoc/>
    public bool IsSealed => OverridesAnotherMember && IsFinal;

    /// <inheritdoc/>
    public bool IsVirtual => MethodInfo.IsVirtual;

    /// <inheritdoc/>
    public bool IsExtensionMethod { get; }

    /// <inheritdoc/>
    public bool IsExplicitImplementation => ExplicitInterfaceType is not null;

    /// <inheritdoc/>
    public ITypeNameData? ExplicitInterfaceType => Tools.ExplicitInterfaceType.Of(this);

    /// <inheritdoc/>
    public ITypeNameData? BaseDeclaringType { get; }

    /// <summary>
    /// Collection of type parameters declared in the method; the keys represent type parameter names.
    /// </summary>
    internal IReadOnlyDictionary<string, TypeParameterData> TypeParameters { get; }

    /// <inheritdoc/>
    IReadOnlyList<ITypeParameterData> IMethodData.TypeParameters => TypeParameters.Values
        .OrderBy(t => t.Index)
        .ToList();

    /// <inheritdoc/>
    public IEnumerable<ITypeNameData> ImplementedInterfaces { get; } = [];
}
