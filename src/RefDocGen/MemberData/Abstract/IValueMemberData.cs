namespace RefDocGen.MemberData.Abstract;

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
    /// Type of the field.
    /// </summary>
    string Type { get; }
}
