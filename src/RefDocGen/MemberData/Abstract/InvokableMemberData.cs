using RefDocGen.DocExtraction;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace RefDocGen.MemberData.Abstract;

/// <summary>
/// Represents data of a invokable member (i.e. method or a constructor).
/// </summary>
public abstract record class InvokableMemberData : ICallableMemberData
{
    /// <summary>
    /// <see cref="MethodBase"/> object representing the member.
    /// </summary>
    private readonly MethodBase methodBase;

    /// <summary>
    /// Create new <see cref="InvokableMemberData"/> instance.
    /// </summary>
    /// <param name="methodBase"><see cref="MethodBase"/> object representing the member.</param>
    protected InvokableMemberData(MethodBase methodBase)
    {
        this.methodBase = methodBase;

        // add parameters
        Parameters = methodBase.GetParameters()
            .OrderBy(p => p.Position)
            .Select(p => new ParameterData(p))
            .ToArray();
    }

    /// <inheritdoc/>
    public abstract string Name { get; }

    /// <inheritdoc/>
    public abstract bool OverridesAnotherMember { get; }

    /// <inheritdoc/>
    public AccessModifier AccessModifier => AccessModifierExtensions.GetAccessModifier(methodBase.IsPrivate, methodBase.IsFamily,
        methodBase.IsAssembly, methodBase.IsPublic, methodBase.IsFamilyAndAssembly, methodBase.IsFamilyOrAssembly);

    /// <inheritdoc/>
    public bool IsStatic => methodBase.IsStatic;

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

    /// <inheritdoc/>
    public XElement DocComment { get; init; } = DocCommentTools.EmptySummaryNode;

    /// <summary>
    /// Array of method parameters, ordered by their position.
    /// </summary>
    public ParameterData[] Parameters { get; }
}
