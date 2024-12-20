using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types.Exception;
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
    /// <param name="declaredTypeParameters">Collection of type parameters declared in the containing type; keys represent type parameter names.</param>
    protected ExecutableMemberData(MethodBase methodBase, IReadOnlyDictionary<string, TypeParameterData> declaredTypeParameters) : base(methodBase)
    {
        this.methodBase = methodBase;

        // add parameters
        Parameters = methodBase.GetParameters()
            .OrderBy(p => p.Position)
            .Select(p => new ParameterData(p, declaredTypeParameters))
            .ToArray();
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
    /// Array of method parameters, ordered by their position.
    /// </summary>
    public IReadOnlyList<ParameterData> Parameters { get; }

    /// <inheritdoc/>
    IReadOnlyList<IParameterData> IExecutableMemberData.Parameters => Parameters;

    /// <inheritdoc/>
    public IEnumerable<IExceptionDocumentation> Exceptions { get; internal set; } = [];
}
