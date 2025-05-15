using RefDocGen.CodeElements.Members.Concrete;
using RefDocGen.CodeElements.Types.Concrete;

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
