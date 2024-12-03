using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Concrete.Types;
using System.Reflection;

namespace RefDocGen.CodeElements.Concrete.Members;

internal class OperatorData : MethodData, IOperatorData
{
    internal OperatorData(MethodInfo methodInfo, IReadOnlyDictionary<string, TypeParameterDeclaration> declaredTypeParameters)
        : base(methodInfo, declaredTypeParameters)
    {
        Kind = OperatorKind.UnaryMinus;
    }

    /// <inheritdoc/>
    public OperatorKind Kind { get; }
}
