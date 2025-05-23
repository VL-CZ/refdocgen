using RefDocGen.CodeElements.Types.Abstract.TypeName;

namespace RefDocGen.CodeElements.Members.Abstract;

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
    /// Checks if the member is required (i.e. must be initialized by an object initializer).
    /// </summary>
    /// <remarks>
    /// For further info, see <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/required"/>.
    /// </remarks>
    bool IsRequired { get; }

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
