using RefDocGen.MemberData.Abstract;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.MemberData.Concrete;

/// <summary>
/// Class representing data of a method.
/// </summary>
internal class MethodData : ExecutableMemberData, IMethodData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodData"/> class.
    /// </summary>
    /// <param name="methodInfo"><see cref="System.Reflection.MethodInfo"/> object representing the method.</param>
    public MethodData(MethodInfo methodInfo) : base(methodInfo)
    {
        MethodInfo = methodInfo;
    }

    /// <inheritdoc/>
    public MethodInfo MethodInfo { get; }

    /// <inheritdoc/>
    public override string Name => MethodInfo.Name;

    /// <inheritdoc/>
    public string ReturnType => MethodInfo.ReturnType.Name;

    /// <inheritdoc/>
    public XElement ReturnValueDocComment { get; internal set; } = XmlDocElementFactory.EmptyReturns;

    /// <inheritdoc/>
    public override bool OverridesAnotherMember => !MethodInfo.Equals(MethodInfo.GetBaseDefinition());
}