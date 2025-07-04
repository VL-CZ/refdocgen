using RefDocGen.AssemblyAnalysis.Extensions;
using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Members.Concrete;
using RefDocGen.CodeElements.Types.Concrete;
using RefDocGen.CodeElements.Types.Concrete.Attribute;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RefDocGen.AssemblyAnalysis.MemberCreators;

/// <summary>
/// Class containing helper methods related to creating type and member data objects.
/// </summary>
internal static class MemberCreatorHelper
{
    /// <summary>
    /// Creates a dictionary of method's parameters, indexed by their names.
    /// </summary>
    /// <param name="method">The method containing the paramters.</param>
    /// <param name="availableTypeParameters">Dictionary of type parameters available in the context of the method, indexed by their names.</param>
    /// <returns>
    /// <inheritdoc cref="MethodLikeMemberData.Parameters" path="/summary"/>
    /// </returns>
    internal static Dictionary<string, ParameterData> CreateParametersDictionary(MethodBase method, IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters)
    {
        bool isExtensionMethod = method.IsDefined(typeof(ExtensionAttribute), true);
        return GetParametersDictionary(method.GetParameters(), availableTypeParameters, isExtensionMethod);
    }

    /// <summary>
    /// Creates a dictionary of indexer's parameters; the keys represent parameter names.
    /// </summary>
    /// <param name="indexer">The indexer containing the paramters.</param>
    /// <param name="availableTypeParameters">Dictionary of type parameters available in the context of the indexer, indexed by their names.</param>
    /// <returns>
    /// <inheritdoc cref="IndexerData.Parameters" path="/summary"/>
    /// </returns>
    internal static Dictionary<string, ParameterData> CreateParametersDictionary(PropertyInfo indexer, IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters)
    {
        return GetParametersDictionary(indexer.GetIndexParameters(), availableTypeParameters);
    }

    /// <summary>
    /// Creates a dictionary of type parameters declared in the given type, indexed by their names.
    /// </summary>
    /// <param name="type">The type containing the type parameters.</param>
    /// <param name="includeInherited">Indicates whether the type parameters inherited from the containing type should be included.</param>
    /// <returns>
    /// <inheritdoc cref="TypeDeclaration.TypeParameters" path="/summary"/>
    /// </returns>
    internal static Dictionary<string, TypeParameterData> CreateTypeParametersDictionary(Type type, bool includeInherited = true)
    {
        if (includeInherited)
        {
            return GetTypeParametersDictionary(type.GetGenericArguments(), CodeElementKind.Type);
        }
        else
        {
            var allTypeParams = type.GetGenericArguments();

            var inheritedTypeParams = Array.Empty<Type>();

            if (type.DeclaringType is not null)
            {
                var parentTypeParams = type.DeclaringType.GetGenericArguments().Select(x => x.Name);
                inheritedTypeParams = [.. allTypeParams.Where(g => parentTypeParams.Contains(g.Name))];
            }

            var nonInheritedTypeParams = allTypeParams.Except(inheritedTypeParams).ToArray();

            return GetTypeParametersDictionary(nonInheritedTypeParams, CodeElementKind.Type);
        }
    }

    /// <summary>
    /// Creates a dictionary of type parameters declared in the given method, indexed by their names.
    /// </summary>
    /// <param name="method">The method containing the type parameters.</param>
    /// <returns>
    /// <inheritdoc cref="MethodData.TypeParameters" path="/summary"/>
    /// </returns>
    internal static Dictionary<string, TypeParameterData> CreateTypeParametersDictionary(MethodBase method)
    {
        return GetTypeParametersDictionary(method.GetGenericArguments(), CodeElementKind.Member);
    }

    /// <summary>
    /// Gets an array of attributes applied to the given parameter.
    /// </summary>
    /// <param name="param">The selected parameter.</param>
    /// <param name="availableTypeParameters">Dictionary of type parameters available in the context of the parameter, indexed by their names.</param>
    /// <returns>An array of <see cref="AttributeData"/> instances, representing attributes applied to the given parameter.</returns>
    internal static AttributeData[] GetAttributeData(ParameterInfo param, IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters)
    {
        return GetAttributeData(param.GetCustomAttributesData(), availableTypeParameters);
    }

    /// <summary>
    /// Gets an array of attributes applied to the given member.
    /// </summary>
    /// <param name="member">The selected member.</param>
    /// <param name="availableTypeParameters">Dictionary of type parameters available in the context of the member, indexed by their names.</param>
    /// <returns>An array of <see cref="AttributeData"/> instances, representing attributes applied to the given member.</returns>
    internal static AttributeData[] GetAttributeData(MemberInfo member, IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters)
    {
        return GetAttributeData(member.GetCustomAttributesData(), availableTypeParameters);
    }

    /// <summary>
    /// Gets an array of attributes applied to the given member or parameter.
    /// </summary>
    /// <param name="attributes">An enumerable of <see cref="CustomAttributeData"/> instances representing the attributes applied to the member/parameter.</param>
    /// <param name="availableTypeParameters">Dictionary of type parameters available, indexed by their names.</param>
    /// <returns>An array of <see cref="AttributeData"/> instances, representing attributes applied to the given member/parameter.</returns>
    /// <remarks>Compiler generated attributes are excluded from the result.</remarks>
    private static AttributeData[] GetAttributeData(IEnumerable<CustomAttributeData> attributes, IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters)
    {
        string[] otherCompilerGeneratedAttrs = [
            "Microsoft.FSharp.Core.CompilationArgumentCountsAttribute",
            "Microsoft.FSharp.Core.CompilationMappingAttribute",
            "Microsoft.FSharp.Core.CompilationSourceNameAttribute",
            "Microsoft.FSharp.Core.OptionalArgumentAttribute",
            "Microsoft.VisualBasic.CompilerServices.StandardModuleAttribute"
        ];

        return [.. attributes
            .Where(a => !a.IsCompilerGenerated() && !otherCompilerGeneratedAttrs.Contains(a.AttributeType.FullName))
            .Select(a => new AttributeData(a, availableTypeParameters))];
    }

    /// <summary>
    /// Creates a dictionary of type parameters declared in the given type or member, indexed by their names.
    /// </summary>
    /// <param name="typeParameters">Array of the type parameters declared in the given type/member, represented as <see cref="Type"/> instances.</param>
    /// <param name="codeElementKind">Kind of the code element to which the type paramters belong.</param>
    /// <returns>
    /// A dictionary of type parameters, indexed by their names.
    /// </returns>
    private static Dictionary<string, TypeParameterData> GetTypeParametersDictionary(Type[] typeParameters, CodeElementKind codeElementKind)
    {
        return typeParameters
            .Select((ga, i) => new TypeParameterData(ga, i, codeElementKind))
            .ToDictionary(t => t.Name);
    }

    /// <summary>
    /// Creates a dictionary of type parameters declared in the given type or member, indexed by their names.
    /// </summary>
    /// <param name="parameters">Array of the given member parameters, represented as <see cref="ParameterInfo"/> instances.</param>
    /// <param name="availableTypeParameters">Dictionary of type parameters available in the context, indexed by their names.</param>
    /// <param name="isExtensionMethod">Indicates whether the member is an extension method (in that case, the 1st parameter requires special handling).</param>
    /// <returns>
    /// A dictionary of member parameters, indexed by their names.
    /// </returns>
    private static Dictionary<string, ParameterData> GetParametersDictionary(
        ParameterInfo[] parameters,
        IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters,
        bool isExtensionMethod = false)
    {
        return parameters
            .Select((p, index) => new ParameterData(
                p,
                availableTypeParameters,
                GetAttributeData(p, availableTypeParameters),
                isExtensionMethod && index == 0))
            .ToDictionary(p => p.Name);
    }
}
