using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete.Types.Attribute;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RefDocGen.AssemblyAnalysis.MemberCreators;

/// <summary>
/// Class containing helper methods related to creating type and member data objects.
/// </summary>
internal static class MemberCreatorHelper
{
    /// <summary>
    /// Creates a dictionary of method's parameters, indexed by their name.
    /// </summary>
    /// <param name="method">The method containing the paramters.</param>
    /// <param name="availableTypeParameters">Dictionary of type parameters available in the context of the method, indexed by their name.</param>
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
    /// <param name="availableTypeParameters">Dictionary of type parameters available in the context of the indexer, indexed by their name.</param>
    /// <returns>
    /// <inheritdoc cref="IndexerData.Parameters" path="/summary"/>
    /// </returns>
    internal static Dictionary<string, ParameterData> CreateParametersDictionary(PropertyInfo indexer, IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters)
    {
        return GetParametersDictionary(indexer.GetIndexParameters(), availableTypeParameters);
    }

    /// <summary>
    /// Creates a dictionary of type parameters declared in the given type, indexed by their name.
    /// </summary>
    /// <param name="type">The type containing the type parameters.</param>
    /// <returns>
    /// <inheritdoc cref="TypeDeclaration.TypeParameters" path="/summary"/>
    /// </returns>
    internal static Dictionary<string, TypeParameterData> CreateTypeParametersDictionary(Type type)
    {
        return GetTypeParametersDictionary(type.GetGenericArguments(), CodeElementKind.Type);
    }

    /// <summary>
    /// Creates a dictionary of type parameters declared in the given method, indexed by their name.
    /// </summary>
    /// <param name="method">The method containing the type parameters.</param>
    /// <returns>
    /// <inheritdoc cref="MethodData.TypeParameterDeclarations" path="/summary"/>
    /// </returns>
    internal static Dictionary<string, TypeParameterData> CreateTypeParametersDictionary(MethodBase method)
    {
        return GetTypeParametersDictionary(method.GetGenericArguments(), CodeElementKind.Member);
    }

    /// <summary>
    /// Gets an array of attributes applied to the given parameter.
    /// </summary>
    /// <param name="param">The selected parameter.</param>
    /// <param name="availableTypeParameters">Dictionary of type parameters available in the context of the parameter, indexed by their name.</param>
    /// <returns>An array of <see cref="AttributeData"/> instances, representing attributes applied to the given parameter.</returns>
    internal static AttributeData[] GetAttributeData(ParameterInfo param, IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters)
    {
        return GetAttributeData(param.GetCustomAttributesData(), availableTypeParameters);
    }

    /// <summary>
    /// Gets an array of attributes applied to the given member.
    /// </summary>
    /// <param name="member">The selected member.</param>
    /// <param name="availableTypeParameters">Dictionary of type parameters available in the context of the member, indexed by their name.</param>
    /// <returns>An array of <see cref="AttributeData"/> instances, representing attributes applied to the given member.</returns>
    internal static AttributeData[] GetAttributeData(MemberInfo member, IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters)
    {
        return GetAttributeData(member.GetCustomAttributesData(), availableTypeParameters);
    }

    private static AttributeData[] GetAttributeData(IEnumerable<CustomAttributeData> attributes, IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters)
    {
        return attributes
            .Where(a => !a.IsCompilerGenerated())
            .Select(a => new AttributeData(a, availableTypeParameters))
            .ToArray();
    }

    private static Dictionary<string, TypeParameterData> GetTypeParametersDictionary(Type[] typeParameters, CodeElementKind codeElementKind)
    {
        return typeParameters
            .Select((ga, i) => new TypeParameterData(ga, i, codeElementKind))
            .ToDictionary(t => t.Name);
    }

    private static Dictionary<string, ParameterData> GetParametersDictionary(
        ParameterInfo[] parameters,
        IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters,
        bool hasExtensionParameter = false)
    {
        return parameters
            .Select((p, index) => new ParameterData(
                p,
                availableTypeParameters,
                GetAttributeData(p, availableTypeParameters),
                hasExtensionParameter && index == 0))
            .ToDictionary(p => p.Name);
    }
}
