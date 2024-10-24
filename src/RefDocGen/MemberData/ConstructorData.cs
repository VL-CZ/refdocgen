using RefDocGen.MemberData.Abstract;
using System.Reflection;

namespace RefDocGen.MemberData;

/// <summary>
/// Represents data of a constructor.
/// </summary>
/// <param name="ConstructorInfo"><see cref="System.Reflection.ConstructorInfo"/> object representing the constructor.</param>
public record ConstructorData(ConstructorInfo ConstructorInfo) : InvokableMemberData(ConstructorInfo)
{
    /// <summary>
    /// The default name for constructor method in the XML documentation files.
    /// </summary>
    public const string DefaultName = "#ctor";

    /// <inheritdoc/>
    public override bool OverridesAnotherMember => false;

    /// <inheritdoc/>
    public override string Name => DefaultName;
}

