using System.Reflection;
using System.Xml.Linq;
using RefDocGen.CodeElements.Abstract.Types;

namespace RefDocGen.CodeElements.Abstract.Members;

/// <summary>
/// Represents data of a method.
/// </summary>
public interface IMethodData : IExecutableMemberData
{
    /// <summary>
    /// <see cref="System.Reflection.MethodInfo"/> object representing the method.
    /// </summary>
    MethodInfo MethodInfo { get; }

    /// <summary>
    /// Return type of the method.
    /// </summary>
    ITypeNameData ReturnType { get; }

    /// <summary>
    /// Documentation comment for the method return value.
    /// </summary>
    XElement ReturnValueDocComment { get; }
}