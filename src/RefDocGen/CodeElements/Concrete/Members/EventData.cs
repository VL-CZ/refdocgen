using RefDocGen.CodeElements.Abstract.Members;
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
    /// Initializes a new instance of the <see cref="EventData"/> class.
    /// </summary>
    /// <param name="eventInfo"><see cref="System.Reflection.EventInfo"/> object representing the event.</param>
    /// <param name="availableTypeParameters">Collection of type parameters declared in the containing type; the keys represent type parameter names.</param>
    /// <param name="containingType">Type that contains the member.</param>
    internal EventData(EventInfo eventInfo, TypeDeclaration containingType, IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters)
        : base(eventInfo, containingType)
    {
        EventInfo = eventInfo;
        Type = eventInfo.EventHandlerType?.GetTypeNameData(availableTypeParameters) ?? typeof(void).GetTypeNameData();
    }

    public EventInfo EventInfo { get; }

    public bool IsOverridable => throw new NotImplementedException();

    public bool OverridesAnotherMember => throw new NotImplementedException();

    public bool IsAbstract => throw new NotImplementedException();

    public bool IsFinal => throw new NotImplementedException();

    public bool IsSealed => throw new NotImplementedException();

    public bool IsAsync => throw new NotImplementedException();

    public bool IsVirtual => throw new NotImplementedException();

    public IEnumerable<IExceptionDocumentation> DocumentedExceptions => throw new NotImplementedException();

    public bool IsExplicitImplementation => throw new NotImplementedException();

    public ITypeNameData? ExplicitInterfaceType => throw new NotImplementedException();

    public ITypeNameData Type { get; }

    public override AccessModifier AccessModifier => AccessModifier.Public; // TODO: update

    public override bool IsStatic => false; // TODO
}
