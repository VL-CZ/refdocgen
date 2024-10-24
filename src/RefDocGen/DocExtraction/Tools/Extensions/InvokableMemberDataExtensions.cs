using RefDocGen.MemberData.Abstract;

namespace RefDocGen.DocExtraction.Tools.Extensions;

/// <summary>
/// Static class containing extension methods for <see cref="InvokableMemberDataExtensions"/> class.
/// </summary>
internal static class InvokableMemberDataExtensions
{
    /// <summary>
    /// Get member signature in the same format as in the XML documentation comments file.
    /// <para>
    /// The format is described here: <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/#id-strings"/>
    /// </para>
    /// </summary>
    /// <param name="memberData">The member <see cref="InvokableMemberData"/>, whose signature we want.</param>
    /// <returns>Member signature in the same format as in the XML documentation comments file.</returns>
    internal static string GetXmlDocSignature(this InvokableMemberData memberData)
    {
        if (memberData.Parameters.Length == 0) // no params
        {
            return memberData.Name;
        }
        else
        {
            // Get the parameters in the format: System.String, System.Int32, etc.
            string parameterList = string.Join(",",
                    memberData.Parameters.Select(
                        p => p.ParameterInfo.ParameterType.FullName
                            ?.Replace('&', '@') // denotes params passed by reference
                    )
                );

            return memberData.Name + "(" + parameterList + ")";
        }
    }
}
