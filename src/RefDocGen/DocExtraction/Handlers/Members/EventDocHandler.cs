using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding events.
/// </summary>
internal class EventDocHandler : MemberDocHandler<ObjectTypeData, EventData>
{
    /// <inheritdoc/>
    protected override IReadOnlyDictionary<string, EventData> GetMembers(ObjectTypeData type)
    {
        return type.Events;
    }
}
