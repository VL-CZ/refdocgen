using RefDocGen.DocExtraction;
using RefDocGen.MemberData.Interfaces;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace RefDocGen.MemberData;

public record MethodData(MethodInfo MethodInfo) : ICallableMemberData
{
    public string Name => MethodInfo.Name;

    public string ReturnType => MethodInfo.ReturnType.Name;

    public AccessModifier AccessModifier => AccessModifierExtensions.GetAccessModifier(MethodInfo.IsPrivate, MethodInfo.IsFamily,
        MethodInfo.IsAssembly, MethodInfo.IsPublic, MethodInfo.IsFamilyAndAssembly, MethodInfo.IsFamilyOrAssembly);

    /// <inheritdoc/>
    public bool IsStatic => MethodInfo.IsStatic;

    /// <inheritdoc/>
    public bool IsOverridable => MethodInfo.IsVirtual && !MethodInfo.IsFinal;

    /// <inheritdoc/>
    public bool OverridesAnotherMember => !MethodInfo.Equals(MethodInfo.GetBaseDefinition());

    /// <inheritdoc/>
    public bool IsAbstract => MethodInfo.IsAbstract;

    /// <inheritdoc/>
    public bool IsFinal => MethodInfo.IsFinal;

    /// <inheritdoc/>
    public bool IsAsync => MethodInfo.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null;

    /// <inheritdoc/>
    public bool IsSealed => OverridesAnotherMember && IsFinal;

    public XElement DocComment { get; init; } = DocCommentTools.Empty;

    public IEnumerable<MethodParameterData> GetParameters()
    {
        return MethodInfo.GetParameters().Select(p => new MethodParameterData(p));
    }
}
