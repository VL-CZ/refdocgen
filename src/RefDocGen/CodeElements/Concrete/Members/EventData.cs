using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types.Attribute;
using RefDocGen.CodeElements.Abstract.Types.Exception;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Tools;
using System.Reflection;

namespace RefDocGen.CodeElements.Concrete.Members;

/// <inheritdoc cref="IEventData"/>
internal class EventData : MemberData, IEventData
{
    /// <summary>
    /// Add method of the delegate.
    /// </summary>
    private readonly IMethodData? addMethod;

    /// <summary>
    /// Remove method of the delegate.
    /// </summary>
    private readonly IMethodData? removeMethod;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventData"/> class.
    /// </summary>
    /// <param name="eventInfo"><see cref="System.Reflection.EventInfo"/> object representing the event.</param>
    /// <param name="availableTypeParameters">Collection of type parameters declared in the containing type; the keys represent type parameter names.</param>
    /// <param name="containingType">Type that contains the member.</param>
    /// <param name="attributes">Collection of attributes applied to the event.</param>
    /// <param name="addMethod"><inheritdoc cref="addMethod"/></param>
    /// <param name="removeMethod"><inheritdoc cref="removeMethod"/></param>
    internal EventData(
        EventInfo eventInfo,
        MethodData? addMethod,
        MethodData? removeMethod,
        TypeDeclaration containingType,
        IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters,
        IReadOnlyList<IAttributeData> attributes) : base(eventInfo, containingType, attributes)
    {
        this.addMethod = addMethod;
        this.removeMethod = removeMethod;

        EventInfo = eventInfo;
        Type = eventInfo.EventHandlerType?.GetTypeNameData(availableTypeParameters)
            ?? throw new ArgumentException("Cannot obtain event handler type.");

        BaseDeclaringType = Methods.Select(m => m.BaseDeclaringType).Distinct().FirstOrDefault();
    }

    /// <inheritdoc/>
    public override string Id => MemberId.Of(this);

    /// <inheritdoc/>
    public override string Name => MemberName.Of(this);

    /// <inheritdoc/>
    public ITypeNameData Type { get; }

    /// <summary>
    /// Gets the collection of <c>Add</c> and <c>Remove</c> methods (if not null).
    /// </summary>
    private IEnumerable<IMethodData> Methods
    {
        get
        {
            if (addMethod is not null)
            {
                yield return addMethod;
            }
            if (removeMethod is not null)
            {
                yield return removeMethod;
            }
        }
    }

    /// <inheritdoc/>
    public EventInfo EventInfo { get; }

    /// <inheritdoc/>
    public bool IsOverridable => Methods.All(m => m.IsOverridable);

    /// <inheritdoc/>
    public bool OverridesAnotherMember => Methods.All(m => m.OverridesAnotherMember);

    /// <inheritdoc/>
    public bool IsAbstract => Methods.All(m => m.IsAbstract);

    /// <inheritdoc/>
    public bool IsFinal => Methods.All(m => m.IsFinal);

    /// <inheritdoc/>
    public bool IsSealed => Methods.All(m => m.IsSealed);

    /// <inheritdoc/>
    public bool IsAsync => false;

    /// <inheritdoc/>
    public bool IsVirtual => Methods.All(m => m.IsVirtual);

    /// <inheritdoc/>
    public IEnumerable<IExceptionDocumentation> DocumentedExceptions => [];

    /// <inheritdoc/>
    public bool IsExplicitImplementation => Methods.All(m => m.IsExplicitImplementation);

    /// <inheritdoc/>
    public ITypeNameData? ExplicitInterfaceType => Methods
        .Select(m => m.ExplicitInterfaceType)
        .Distinct()
        .SingleOrDefault();

    /// <inheritdoc/>
    public override AccessModifier AccessModifier
    {
        get
        {
            var accessModifiers = Methods.Select(m => m.AccessModifier);
            return AccessModifierHelper.GetTheLeastRestrictive(accessModifiers);
        }
    }

    /// <inheritdoc/>
    public override bool IsStatic => Methods.All(m => m.IsStatic);

    /// <inheritdoc/>
    public ITypeNameData? BaseDeclaringType { get; }

    /// <inheritdoc/>
    internal override string MemberKindId => "E";
}
