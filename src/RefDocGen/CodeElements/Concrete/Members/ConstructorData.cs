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
    /// The default name for constructor method in the XML documentation files.
    /// </summary>
    internal const string DefaultName = "#ctor";

    /// <summary>
    /// Initializes a new instance of the <see cref="ConstructorData"/> class.
    /// </summary>
    /// <param name="constructorInfo"><see cref="System.Reflection.ConstructorInfo"/> object representing the constructor.</param>
    /// <param name="declaredTypeParameters">Collection of type parameters declared in the containing type; the keys represent type parameter names.</param>
    internal ConstructorData(ConstructorInfo constructorInfo, IReadOnlyDictionary<string, TypeParameterData> declaredTypeParameters)
        : base(constructorInfo, declaredTypeParameters)
    {
        ConstructorInfo = constructorInfo;
    }

    /// <inheritdoc/>
    public ConstructorInfo ConstructorInfo { get; }

    /// <inheritdoc/>
    public override bool OverridesAnotherMember => false;

    /// <inheritdoc/>
    public override string Name => DefaultName;

    public override bool IsExplicitImplementation => false;

    /// <inheritdoc/>
    protected override bool IsConstructor()
    {
        return true;
    }
}

