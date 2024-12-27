using RefDocGen.CodeElements.Abstract.Types.TypeName;

namespace RefDocGen.CodeElements.Abstract.Members;

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
    /// Type of the member.
    /// </summary>
    ITypeNameData Type { get; }

    /// <summary>
    /// Constant value of the member.
    /// <para>
    /// Returns <see cref="DBNull.Value"/> if the member has no constant value.
    /// (note that this is done, because <see langword="null"/> can be declared as a constant value of the member.
    /// </para>
    /// </summary>
    object? ConstantValue { get; }
}
