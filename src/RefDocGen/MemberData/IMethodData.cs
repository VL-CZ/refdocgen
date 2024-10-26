using System.Reflection;
using System.Xml.Linq;
using RefDocGen.MemberData.Abstract;

namespace RefDocGen.MemberData;

public interface IMethodData : IInvokableMemberData
{
    /// <summary>
    /// <see cref="System.Reflection.MethodInfo"/> object representing the method.
    /// </summary>
    MethodInfo MethodInfo { get; }

    /// <summary>
    /// Return type of the method.
    /// </summary>
    string ReturnType { get; }

    /// <summary>
    /// Documentation comment for the method return value.
    /// </summary>
    XElement ReturnValueDocComment { get; }
}
