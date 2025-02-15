using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete.Types.Attribute;
using RefDocGen.Tools;
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
    /// <inheritdoc cref="ExecutableMemberData.Parameters" path="/summary"/>
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
    /// <returns>
    /// <inheritdoc cref="TypeDeclaration.AllTypeParameters" path="/summary"/>
    /// </returns>
    internal static Dictionary<string, TypeParameterData> CreateTypeParametersDictionary(Type type)
    {
        var allGeneric = type.GetGenericArguments();

        var inherited = Array.Empty<Type>();

        if (type.DeclaringType is not null)
        {
            var parentArgs = type.DeclaringType.GetGenericArguments().Select(x => x.Name);
            inherited = allGeneric.Where(g => parentArgs.Contains(g.Name)).ToArray();
        }

        var nonInherited = allGeneric.Except(inherited).ToArray();

        var inheritedDict = GetTypeParametersDictionary(inherited, CodeElementKind.Type, true);
        var dict = GetTypeParametersDictionary(nonInherited, CodeElementKind.Type, false, inheritedDict.Count);

        return inheritedDict.Merge(dict);
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
        return attributes
            .Where(a => !a.IsCompilerGenerated())
            .Select(a => new AttributeData(a, availableTypeParameters))
            .ToArray();
    }

    /// <summary>
    /// Creates a dictionary of type parameters declared in the given type or member, indexed by their names.
    /// </summary>
    /// <param name="typeParameters">Array of the type parameters declared in the given type/member, represented as <see cref="Type"/> instances.</param>
    /// <param name="codeElementKind">Kind of the code element to which the type paramters belong.</param>
    /// <returns>
    /// A dictionary of type parameters, indexed by their names.
    /// </returns>
    private static Dictionary<string, TypeParameterData> GetTypeParametersDictionary(Type[] typeParameters,
        CodeElementKind codeElementKind, bool areInherited = false, int startIndex = 0)
    {
        return typeParameters
            .Select((ga, i) => new TypeParameterData(ga, i + startIndex, codeElementKind, areInherited))
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
