using System.Reflection;
using RefDocGen.MemberData.Abstract;

namespace RefDocGen.MemberData.Concrete;

/// <summary>
/// Represents data of a constructor.
/// </summary>
/// <param name="ConstructorInfo"><see cref="System.Reflection.ConstructorInfo"/> object representing the constructor.</param>
internal record ConstructorData(ConstructorInfo ConstructorInfo) : ExecutableMemberData(ConstructorInfo), IConstructorData
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

