using System.Numerics;

namespace RefDocGen.TestingLibrary.Tools;

/// <summary>
/// Struct representing a point.
/// </summary>
internal struct Point
{
    /// <summary>
    /// X coordinate of the point.
    /// </summary>
    internal readonly double X;

    /// <summary>
    /// Y coordinate of the point.
    /// </summary>
    internal readonly double Y;

    /// <summary>
    /// Create new point.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    internal Point(double x, double y)
    {
        this.X = x;
        this.Y = y;
    }

    /// <summary>
    /// Checks if the 2 points are equal.
    /// </summary>
    /// <param name="obj">Another point.</param>
    /// <returns>Are the 2 points are equal?</returns>
    public override bool Equals(object? obj)
    {
        if (obj is Point point)
        {
            return (X, Y) == (point.X, point.Y);
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Compare the 2 points.
    /// </summary>
    /// <param name="point">1st point.</param>
    /// <param name="other">2nd point.</param>
    /// <returns>Are the 2 points equal?</returns>
    public static bool operator ==(Point point, Point other)
    {
        return point.Equals(other);
    }

    /// <inheritdoc cref="operator ==(Point, Point)" />
    public static bool operator !=(Point point, Point other)
    {
        return !point.Equals(other);
    }

    /// <summary>
    /// Negate the point coordinates.
    /// </summary>
    /// <param name="point">The provided point.</param>
    /// <returns>A point with negated coordinates.</returns>
    public static Point operator -(Point point)
    {
        return new Point(-point.X, -point.Y);
    }

    /// <summary>
    /// Converts the point into a vector.
    /// </summary>
    /// <param name="point">Point to convert</param>
    /// <returns>2D vector.</returns>
    public static explicit operator Vector2(Point point)
    {
        return new Vector2((float)point.X, (float)point.Y);
    }

    /// <summary>
    /// Converts the <see cref="Vector2"/> into a <see cref="Point"/>.
    /// </summary>
    /// <param name="vector">Vector to convert.</param>
    /// <returns>A point.</returns>
    public static implicit operator Point(Vector2 vector)
    {
        return new Point(vector.X, vector.Y);
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}
