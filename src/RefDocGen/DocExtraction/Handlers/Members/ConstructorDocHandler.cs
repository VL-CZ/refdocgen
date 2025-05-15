using RefDocGen.CodeElements.Members.Concrete;
using RefDocGen.CodeElements.Types.Concrete;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding constructors.
/// </summary>
internal class ConstructorDocHandler : MethodLikeMemberDocHandler<ConstructorData>
{
    /// <inheritdoc/>
    protected override IReadOnlyDictionary<string, ConstructorData> GetMembers(ObjectTypeData type)
    {
        return type.Constructors;
    }
}
