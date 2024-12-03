namespace RefDocGen.CodeElements;

/// <summary>
/// Represents the kind of an operator.
/// </summary>
public enum OperatorKind
{
    /// <summary>
    /// Represents the unary plus operator.
    /// </summary>
    UnaryPlus,

    /// <summary>
    /// Represents the unary minus operator.
    /// </summary>
    UnaryMinus,

    /// <summary>
    /// Represents the logical negation operator.
    /// </summary>
    LogicalNegation,

    /// <summary>
    /// Represents the bitwise complement operator.
    /// </summary>
    BitwiseComplement,

    /// <summary>
    /// Represents the increment operator.
    /// </summary>
    Increment,

    /// <summary>
    /// Represents the decrement operator.
    /// </summary>
    Decrement,

    /// <summary>
    /// Represents the true operator for conditional evaluation.
    /// </summary>
    True,

    /// <summary>
    /// Represents the false operator for conditional evaluation.
    /// </summary>
    False,

    /// <summary>
    /// Represents the addition operator.
    /// </summary>
    Addition,

    /// <summary>
    /// Represents the subtraction operator.
    /// </summary>
    Subtraction,

    /// <summary>
    /// Represents the multiplication operator.
    /// </summary>
    Multiplication,

    /// <summary>
    /// Represents the division operator.
    /// </summary>
    Division,

    /// <summary>
    /// Represents the remainder operator.
    /// </summary>
    Remainder,

    /// <summary>
    /// Represents the bitwise AND operator.
    /// </summary>
    BitwiseAnd,

    /// <summary>
    /// Represents the bitwise OR operator.
    /// </summary>
    BitwiseOr,

    /// <summary>
    /// Represents the bitwise XOR operator.
    /// </summary>
    ExclusiveOr,

    /// <summary>
    /// Represents the left shift operator.
    /// </summary>
    LeftShift,

    /// <summary>
    /// Represents the right shift operator.
    /// </summary>
    RightShift,

    /// <summary>
    /// Represents the unsigned right shift operator.
    /// </summary>
    UnsignedRightShift,

    /// <summary>
    /// Represents the equality operator.
    /// </summary>
    Equality,

    /// <summary>
    /// Represents the inequality operator.
    /// </summary>
    Inequality,

    /// <summary>
    /// Represents the less-than operator.
    /// </summary>
    LessThan,

    /// <summary>
    /// Represents the greater-than operator.
    /// </summary>
    GreaterThan,

    /// <summary>
    /// Represents the less-than-or-equal operator.
    /// </summary>
    LessThanOrEqual,

    /// <summary>
    /// Represents the greater-than-or-equal operator.
    /// </summary>
    GreaterThanOrEqual,
}
