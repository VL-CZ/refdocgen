using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types.Attribute;
using RefDocGen.CodeElements.Abstract.Types.Exception;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Tools;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RefDocGen.CodeElements.Concrete.Members;

/// <summary>
/// Class representing data of an executable member (i.e. method or a constructor).
/// Note that properties are excluded from this definition.
/// </summary>
internal abstract class ExecutableMemberData : MemberData, IExecutableMemberData
{
    /// <summary>
    /// <see cref="MethodBase"/> object representing the member.
    /// </summary>
    private readonly MethodBase methodBase;

    /// <summary>
    /// Create new <see cref="ExecutableMemberData"/> instance.
    /// </summary>
    /// <param name="methodBase"><see cref="MethodBase"/> object representing the member.</param>
    /// <param name="containingType">Type that contains the member.</param>
    /// <param name="attributes">Collection of attributes applied to the member.</param>
    /// <param name="parameters">Dictionary of member parameters, the keys represent parameter names.</param>
    protected ExecutableMemberData(
        MethodBase methodBase,
        TypeDeclaration containingType,
        IReadOnlyDictionary<string, ParameterData> parameters,
        IReadOnlyList<IAttributeData> attributes)
        : base(methodBase, containingType, attributes)
    {
        this.methodBase = methodBase;
        Parameters = parameters;
    }

    /// <inheritdoc/>
    public override string Id => MemberId.Of(this);

    /// <inheritdoc/>
    public abstract bool OverridesAnotherMember { get; }

    /// <inheritdoc/>
    public override AccessModifier AccessModifier => AccessModifierExtensions.GetAccessModifier(methodBase.IsPrivate, methodBase.IsFamily,
        methodBase.IsAssembly, methodBase.IsPublic, methodBase.IsFamilyAndAssembly, methodBase.IsFamilyOrAssembly);

    /// <inheritdoc/>
    public override bool IsStatic => methodBase.IsStatic;

    /// <inheritdoc/>
    public bool IsOverridable => methodBase.IsVirtual && !methodBase.IsFinal;

    /// <inheritdoc/>
    public bool IsAbstract => methodBase.IsAbstract;

    /// <inheritdoc/>
    public bool IsFinal => methodBase.IsFinal;

    /// <inheritdoc/>
    public bool IsAsync => methodBase.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null;

    /// <inheritdoc/>
    public bool IsSealed => OverridesAnotherMember && IsFinal;

    /// <inheritdoc/>
    public bool IsVirtual => methodBase.IsVirtual;

    /// <summary>
    /// Dictionary of method parameters, the keys represents parameter names.
    /// </summary>
    internal IReadOnlyDictionary<string, ParameterData> Parameters { get; }

    /// <inheritdoc/>
    IReadOnlyList<IParameterData> IExecutableMemberData.Parameters => Parameters.Values
        .OrderBy(p => p.Position)
        .ToList();

    /// <inheritdoc/>
    public IEnumerable<IExceptionDocumentation> DocumentedExceptions { get; internal set; } = [];

    /// <inheritdoc/>
    public bool IsExplicitImplementation => ExplicitInterfaceType is not null;

    /// <inheritdoc/>
    public virtual ITypeNameData? ExplicitInterfaceType => Tools.ExplicitInterfaceType.Of(this);
}
