using RefDocGen.CodeElements.Types.Abstract.TypeName;
using System.Reflection;

namespace RefDocGen.CodeElements.Members.Abstract;

/// <summary>
/// Represents data of an event.
/// </summary>
public interface IEventData : ICallableMemberData
{
    /// <summary>
    /// <see cref="System.Reflection.EventInfo"/> object representing the event.
    /// </summary>
    EventInfo EventInfo { get; }

    /// <summary>
    /// Type of the event.
    /// </summary>
    ITypeNameData Type { get; }
}
