using System.Reflection;
using System.Xml.Linq;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.TypeName;

namespace RefDocGen.CodeElements.Members.Abstract;

/// <summary>
/// Represents data of a method.
/// </summary>
public interface IMethodData : IParameterizedMemberData, ICallableMemberData
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

    /// <summary>
    /// Collection of generic type parameters declared in the method.
    /// </summary>
    IReadOnlyList<ITypeParameterData> TypeParameters { get; }
}
