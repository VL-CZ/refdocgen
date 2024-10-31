namespace RefDocGen.MemberData.Abstract;

/// <summary>
/// Represents a type member storing a value; i.e. a field or a property.
/// </summary>
public interface IValueMemberData : IMemberData
{
    /// <summary>
    /// Checks if the member is constant.
    /// </summary>
    bool IsConstant { get; }

    /// <summary>
    /// Checks if the member is readonly.
    /// </summary>
    bool IsReadonly { get; }

    /// <summary>
    /// TODO: update
    /// </summary>
    ITypeNameData Type { get; }
}
