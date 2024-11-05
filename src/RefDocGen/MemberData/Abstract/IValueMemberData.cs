namespace RefDocGen.MemberData.Abstract;

/// <summary>
/// Represents a type member storing a value; i.e. a field or a property.
/// </summary>
public interface IValueMemberData : IMemberData
{
    /// <summary>
    /// Checks if the member is a compile-time constant.
    /// </summary>
    bool IsConstant { get; }

    /// <summary>
    /// Checks if the member is readonly (i.e. can be set just inside a constructor)
    /// </summary>
    bool IsReadonly { get; }

    /// <summary>
    /// Type of the value.
    /// </summary>
    ITypeNameData Type { get; }
}
