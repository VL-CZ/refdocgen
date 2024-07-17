using RefDocGen.DocExtraction;
using RefDocGen.MemberData.Interfaces;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace RefDocGen.MemberData;

/// <summary>
/// Represents data of a method.
/// </summary>
/// <param name="MethodInfo"><see cref="MethodInfo"/> object representing the field.</param>
public record MethodData(MethodInfo MethodInfo) : ICallableMemberData
{
    /// <inheritdoc/>
    public string Name => MethodInfo.Name;

    /// <summary>
    /// Return type of the method.
    /// </summary>
    public string ReturnType => MethodInfo.ReturnType.Name;

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public bool IsVirtual => MethodInfo.IsVirtual;

    /// <summary>
    /// XML doc comment for the method
    /// </summary>
    public XElement DocComment { get; init; } = DocCommentTools.EmptySummary;

    /// <summary>
    /// Gets the method parameters represented as <see cref="MethodParameterData"/> objects, ordered by their position.
    /// </summary>
    /// <returns>Method parameters represented as <see cref="MethodParameterData"/> objects.</returns>
    public IEnumerable<MethodParameterData> GetParameters()
    {
        return MethodInfo.GetParameters().OrderBy(p => p.Position).Select(p => new MethodParameterData(p));
    }
}
