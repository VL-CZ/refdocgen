using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.TypeName;

namespace RefDocGen.TemplateProcessors.Shared.Tools;

/// <summary>
/// Class reponsible for returning type and member IDs used in templates.
/// </summary>
/// <remarks>
/// Note: we cannot use the <see cref="ITypeNameBaseData.Id"/> and <see cref="IMemberData.Id"/> values, as they may contain special URL characters.
/// </remarks>
internal class TemplateId
{
    /// <summary>
    /// Get ID of the given type used in templates.
    /// </summary>
    /// <param name="type">The provided type.</param>
    /// <returns>The ID of the given type used in templates.</returns>
    /// <remarks>
    /// The ID is intended to be used in the URL.
    /// </remarks>
    internal static string Of(ITypeDeclaration type)
    {
        return Escape(type.Id);
    }

    /// <summary>
    /// Get ID of the given member used in templates.
    /// </summary>
    /// <param name="member">The provided member.</param>
    /// <returns>The ID of the given member used in templates.</returns>
    /// <remarks>
    /// The ID is intended to be used in the URL.
    /// </remarks>
    internal static string Of(IMemberData member)
    {
        return Escape(member.Id);
    }

    /// <summary>
    /// Escapes the ID of a type or member.
    /// </summary>
    /// <param name="id">An ID of a type or a member to escape.</param>
    /// <returns>The escaped ID, which can be used as a part of URL.</returns>
    internal static string Escape(string id)
    {
        return id
            .Replace("`", "-") // generic param symbol
            .Replace("@", "-") // parameter passed by ref symbol
            .Replace("{", "(") // generic paramter brackets symbol
            .Replace("}", ")")
            .Replace("[", "(") // array brackets symbol
            .Replace("]", ")")
            .Replace("#", "."); // explicit interface namespace delimiter + special member name start symbol
    }
}
