using System.Reflection;
using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types.Attribute;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete.Types;

namespace RefDocGen.CodeElements.Concrete.Members;

/// <summary>
/// Class representing data of a constructor.
/// </summary>
internal class ConstructorData : ExecutableMemberData, IConstructorData
{
    /// <summary>
    /// The default name for constructor method in the XML documentation files.
    /// </summary>
    internal const string DefaultName = "#ctor";

    /// <summary>
    /// Initializes a new instance of the <see cref="ConstructorData"/> class.
    /// </summary>
    /// <param name="constructorInfo"><see cref="System.Reflection.ConstructorInfo"/> object representing the constructor.</param>
    /// <param name="parameters">Dictionary of constructor parameters, the keys represent parameter names.</param>
    /// <param name="containingType">Type that contains the member.</param>
    /// <param name="attributes">Collection of attributes applied to the constructor.</param>
    internal ConstructorData(
        ConstructorInfo constructorInfo,
        TypeDeclaration containingType,
        IReadOnlyDictionary<string, ParameterData> parameters,
        IReadOnlyList<IAttributeData> attributes) : base(constructorInfo, containingType, parameters, attributes)
    {
        ConstructorInfo = constructorInfo;
    }

    /// <inheritdoc/>
    public ConstructorInfo ConstructorInfo { get; }

    /// <inheritdoc/>
    public override bool OverridesAnotherMember => false;

    /// <inheritdoc/>
    public override string Name => DefaultName;

    /// <inheritdoc/>
    public override ITypeNameData? ExplicitInterfaceType => null; // the constructors can't be explicitly declared
}
