using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types.Attribute;
using RefDocGen.CodeElements.Abstract.Types.Exception;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Tools;
using System.Reflection;

namespace RefDocGen.CodeElements.Concrete.Members;

/// <summary>
/// Class representing data of an executable member (i.e. method or a constructor).
/// Note that properties are excluded from this definition.
/// </summary>
internal abstract class ExecutableMemberData : MemberData, IParametricMemberData
{
    /// <summary>
    /// <see cref="MethodBase"/> object representing the member.
    /// </summary>
    private readonly MethodBase methodBase;

    /// <summary>
    /// Create new <see cref="ExecutableMemberData"/> instance.
    /// </summary>
    /// <param name="methodBase"><see cref="MethodBase"/> object representing the member.</param>
    /// <param name="containingType">Type that contains the member.</param>
    /// <param name="attributes">Collection of attributes applied to the member.</param>
    /// <param name="parameters">Dictionary of member parameters, the keys represent parameter names.</param>
    protected ExecutableMemberData(
        MethodBase methodBase,
        TypeDeclaration containingType,
        IReadOnlyDictionary<string, ParameterData> parameters,
        IReadOnlyList<IAttributeData> attributes)
        : base(methodBase, containingType, attributes)
    {
        this.methodBase = methodBase;
        Parameters = parameters;
    }

    /// <inheritdoc/>
    public override AccessModifier AccessModifier => AccessModifierHelper.GetAccessModifier(methodBase.IsPrivate, methodBase.IsFamily,
        methodBase.IsAssembly, methodBase.IsPublic, methodBase.IsFamilyAndAssembly, methodBase.IsFamilyOrAssembly);

    /// <inheritdoc/>
    public override bool IsStatic => methodBase.IsStatic;

    /// <summary>
    /// Dictionary of method parameters, the keys represents parameter names.
    /// </summary>
    internal IReadOnlyDictionary<string, ParameterData> Parameters { get; }

    /// <inheritdoc/>
    IReadOnlyList<IParameterData> IParametricMemberData.Parameters => Parameters.Values
        .OrderBy(p => p.Position)
        .ToList();

    /// <inheritdoc/>
    public IEnumerable<IExceptionDocumentation> DocumentedExceptions { get; internal set; } = [];

    /// <inheritdoc/>
    internal override string MemberKindId => "M";
}
