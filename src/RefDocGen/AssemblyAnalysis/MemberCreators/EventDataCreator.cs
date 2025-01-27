using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using System.Reflection;

namespace RefDocGen.AssemblyAnalysis.MemberCreators;

/// <summary>
/// Class responsible for creating <see cref="EventData"/> instances.
/// </summary>
internal static class EventDataCreator
{
    /// <summary>
    /// Creates a <see cref="EventData"/> instance from the corresponding <paramref name="eventInfo"/>.
    /// </summary>
    /// <param name="eventInfo"><see cref="EventInfo"/> object representing the event.</param>
    /// <param name="containingType">Type containing the event.</param>
    /// <param name="availableTypeParameters">A dictionary of available type parameters; the keys represent type parameter names.</param>
    /// <returns>A <see cref="EventData"/> instance representing the event.</returns>
    internal static EventData CreateFrom(EventInfo eventInfo, TypeDeclaration containingType, Dictionary<string, TypeParameterData> availableTypeParameters)
    {
        MethodData? addMethod = null;
        MethodData? removeMethod = null;

        if (eventInfo.GetAddMethod(nonPublic: true) is MethodInfo addMethodInfo)
        {
            addMethod = MethodDataCreator.CreateFrom(addMethodInfo, containingType, availableTypeParameters);
        }

        if (eventInfo.GetRemoveMethod(nonPublic: true) is MethodInfo removeMethodInfo)
        {
            removeMethod = MethodDataCreator.CreateFrom(removeMethodInfo, containingType, availableTypeParameters);
        }

        return new EventData(
            eventInfo,
            addMethod,
            removeMethod,
            containingType,
            availableTypeParameters,
            Helper.GetAttributeData(eventInfo, availableTypeParameters));
    }
}
