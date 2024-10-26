using System.Reflection;
using RefDocGen.MemberData.Abstract;

namespace RefDocGen.MemberData;

public interface IConstructorData : IInvokableMemberData
{
    /// <summary>
    /// <see cref="System.Reflection.ConstructorInfo"/> object representing the constructor.
    /// </summary>
    ConstructorInfo ConstructorInfo { get; }
}
