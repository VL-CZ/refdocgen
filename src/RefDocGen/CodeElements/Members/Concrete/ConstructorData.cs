using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Members.Tools;
using RefDocGen.CodeElements.Types.Abstract.Attribute;
using RefDocGen.CodeElements.Types.Concrete;
using System.Reflection;

namespace RefDocGen.CodeElements.Members.Concrete;

/// <summary>
/// Class representing data of a constructor.
/// </summary>
internal class ConstructorData : MethodLikeMemberData, IConstructorData
{
    /// <summary>
    /// The default name for constructor method in the XML documentation files.
    /// </summary>
    internal const string DefaultName = "#ctor";

    /// <summary>
    /// The default name for a static constructor method in the XML documentation files.
    /// </summary>
    internal const string DefaultStaticName = "#cctor";

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
    public override string Id => MemberId.Of(this);

    /// <inheritdoc/>
    public ConstructorInfo ConstructorInfo { get; }

    /// <inheritdoc/>
    public override string Name => IsStatic
        ? DefaultStaticName
        : DefaultName;
}
