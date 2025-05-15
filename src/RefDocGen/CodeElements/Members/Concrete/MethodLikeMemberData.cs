using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Shared;
using RefDocGen.CodeElements.Types.Abstract.Attribute;
using RefDocGen.CodeElements.Types.Abstract.Exception;
using RefDocGen.CodeElements.Types.Concrete;
using System.Reflection;

namespace RefDocGen.CodeElements.Members.Concrete;

/// <summary>
/// Represents data of a method-like type member (such as a method or a constructor).
/// </summary>
internal abstract class MethodLikeMemberData : MemberData, IParameterizedMemberData
{
    /// <summary>
    /// <see cref="MethodBase"/> object representing the member.
    /// </summary>
    private readonly MethodBase methodBase;

    /// <summary>
    /// Create new <see cref="MethodLikeMemberData"/> instance.
    /// </summary>
    /// <param name="methodBase"><see cref="MethodBase"/> object representing the member.</param>
    /// <param name="containingType">Type that contains the member.</param>
    /// <param name="attributes">Collection of attributes applied to the member.</param>
    /// <param name="parameters">Dictionary of member parameters, the keys represent parameter names.</param>
    protected MethodLikeMemberData(
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
    IReadOnlyList<IParameterData> IParameterizedMemberData.Parameters => Parameters.Values
        .OrderBy(p => p.Position)
        .ToList();

    /// <inheritdoc/>
    public IEnumerable<IExceptionDocumentation> DocumentedExceptions { get; internal set; } = [];

    /// <inheritdoc/>
    internal override string MemberKindId => "M";
}
