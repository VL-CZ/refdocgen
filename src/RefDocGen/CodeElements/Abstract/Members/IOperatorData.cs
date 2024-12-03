namespace RefDocGen.CodeElements.Abstract.Members;

/// <summary>
/// Represents data of an operator.
/// </summary>
public interface IOperatorData : IMethodData
{
    /// <summary>
    /// Kind of the operator.
    /// </summary>
    public OperatorKind Kind { get; }
}
