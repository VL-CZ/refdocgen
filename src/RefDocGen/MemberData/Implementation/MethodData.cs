using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.MemberData.Implementation;

/// <summary>
/// Represents data of a method.
/// </summary>
/// <param name="MethodInfo"><see cref="System.Reflection.MethodInfo"/> object representing the method.</param>
internal record MethodData(MethodInfo MethodInfo) : InvokableMemberData(MethodInfo), IMethodData
{
    /// <inheritdoc/>
    public override string Name => MethodInfo.Name;

    /// <inheritdoc/>
    public string ReturnType => MethodInfo.ReturnType.Name;

    /// <inheritdoc/>
    public XElement ReturnValueDocComment { get; init; } = XmlDocElementFactory.EmptyReturns;

    /// <inheritdoc/>
    public override bool OverridesAnotherMember => !MethodInfo.Equals(MethodInfo.GetBaseDefinition());
}
