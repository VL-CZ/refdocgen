using System.Reflection;
using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Concrete.Types;

namespace RefDocGen.CodeElements.Concrete.Members;

/// <summary>
/// Class representing data of a constructor.
/// </summary>
internal class ConstructorData : ExecutableMemberData, IConstructorData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstructorData"/> class.
    /// </summary>
    /// <param name="constructorInfo"><see cref="System.Reflection.ConstructorInfo"/> object representing the constructor.</param>
    public ConstructorData(ConstructorInfo constructorInfo, IReadOnlyDictionary<string, TypeParameterDeclaration> declaredTypeParameters)
        : base(constructorInfo, declaredTypeParameters)
    {
        ConstructorInfo = constructorInfo;
    }

    /// <inheritdoc/>
    public ConstructorInfo ConstructorInfo { get; }

    /// <summary>
    /// The default name for constructor method in the XML documentation files.
    /// </summary>
    public const string DefaultName = "#ctor";

    /// <inheritdoc/>
    public override bool OverridesAnotherMember => false;

    /// <inheritdoc/>
    public override string Name => DefaultName;
}

