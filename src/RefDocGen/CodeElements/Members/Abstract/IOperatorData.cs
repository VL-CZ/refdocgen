namespace RefDocGen.CodeElements.Members.Abstract;

/// <summary>
/// Represents data of an operator.
/// </summary>
public interface IOperatorData : IMethodData
{
    /// <summary>
    /// Kind of the operator.
    /// </summary>
    OperatorKind Kind { get; }

    /// <summary>
    /// Checks if the operator represents a conversion operator (explicit or implicit).
    /// </summary>
    bool IsConversionOperator { get; }
}
