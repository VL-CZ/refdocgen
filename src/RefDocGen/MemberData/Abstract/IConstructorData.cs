using System.Reflection;

namespace RefDocGen.MemberData.Abstract;

public interface IConstructorData : IExecutableMemberData
{
    /// <summary>
    /// <see cref="System.Reflection.ConstructorInfo"/> object representing the constructor.
    /// </summary>
    ConstructorInfo ConstructorInfo { get; }
}
