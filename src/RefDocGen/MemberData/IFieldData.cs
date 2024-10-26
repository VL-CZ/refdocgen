using RefDocGen.MemberData.Abstract;
using System.Reflection;

namespace RefDocGen.MemberData;

public interface IFieldData : IValueMemberData
{
    /// <summary>
    /// <see cref="System.Reflection.FieldInfo"/> object representing the field.
    /// </summary>
    FieldInfo FieldInfo { get; }
}
