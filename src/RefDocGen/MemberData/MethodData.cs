using RefDocGen.DocExtraction.Tools;
using RefDocGen.MemberData.Abstract;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.MemberData;

/// <summary>
/// Represents data of a method.
/// </summary>
/// <param name="MethodInfo"><see cref="System.Reflection.MethodInfo"/> object representing the method.</param>
public record MethodData(MethodInfo MethodInfo) : InvokableMemberData(MethodInfo)
{
    /// <inheritdoc/>
    public override string Name => MethodInfo.Name;

    /// <summary>
    /// Return type of the method.
    /// </summary>
    public string ReturnType => MethodInfo.ReturnType.Name;

    /// <summary>
    /// Documentation comment for the method return value.
    /// </summary>
    public XElement ReturnValueDocComment { get; init; } = EmptyDocCommentNode.Returns;

    /// <inheritdoc/>
    public override bool OverridesAnotherMember => !MethodInfo.Equals(MethodInfo.GetBaseDefinition());


}
