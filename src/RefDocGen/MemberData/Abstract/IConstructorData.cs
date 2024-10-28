using System.Reflection;

namespace RefDocGen.MemberData.Abstract;

/// <summary>
/// Represents data of a constructor.
/// </summary>
public interface IConstructorData : IExecutableMemberData
{
    /// <summary>
    /// <see cref="System.Reflection.ConstructorInfo"/> object representing the constructor.
    /// </summary>
    ConstructorInfo ConstructorInfo { get; }
}
