using RefDocGen.MemberData.Abstract;
using System.Reflection;

namespace RefDocGen.MemberData;

/// <summary>
/// Represents data of a constructor.
/// </summary>
/// <param name="ConstructorInfo"><see cref="System.Reflection.ConstructorInfo"/> object representing the constructor.</param>
public record ConstructorData(ConstructorInfo ConstructorInfo) : InvokableMemberData(ConstructorInfo)
{
    /// <inheritdoc/>
    public override bool OverridesAnotherMember => false;

    /// <inheritdoc/>
    public override string Name => string.Empty;
}

