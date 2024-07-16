using System.Reflection;
using System.Runtime.CompilerServices;

namespace RefDocGen.MemberData;

public record MethodData(MethodInfo MethodInfo)
{
    public string Name => MethodInfo.Name;

    public string ReturnType => MethodInfo.ReturnType.Name;

    public AccessModifier AccessModifier => AccessModifierExtensions.GetAccessModifier(MethodInfo.IsPrivate, MethodInfo.IsFamily,
        MethodInfo.IsAssembly, MethodInfo.IsPublic, MethodInfo.IsFamilyAndAssembly, MethodInfo.IsFamilyOrAssembly);

    public bool IsStatic => MethodInfo.IsStatic;

    public bool IsOverridable => MethodInfo.IsVirtual && !MethodInfo.IsFinal;

    public bool OverridesAnotherMethod => !MethodInfo.Equals(MethodInfo.GetBaseDefinition());

    public bool IsAbstract => MethodInfo.IsAbstract;

    public bool IsFinal => MethodInfo.IsFinal;

    public bool IsAsync => MethodInfo.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null;

    public bool IsSealed => OverridesAnotherMethod && IsFinal;

    public bool HasVirtualKeyword => IsOverridable && !IsAbstract && !OverridesAnotherMethod;

    public IEnumerable<MethodParameterData> GetParameters()
    {
        return MethodInfo.GetParameters().Select(p => new MethodParameterData(p));
    }
}
