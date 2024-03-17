using System.Numerics;

namespace Kintsugi.Core;

/// <summary>
/// Representation of an int-based vector.
/// </summary>
public readonly struct Vec2Int
{
    /// <summary>
    /// X-coordinate
    /// </summary>
    public int x { get; }
    /// <summary>
    /// Y-coorinate
    /// </summary>
    public int y { get; }

    #region Static Properties

    public static readonly Vec2Int Up = new(0, 1);
    public static readonly Vec2Int Down = new(0, -1);
    public static readonly Vec2Int Left = new(-1, 0);
    public static readonly Vec2Int Right = new(1, 0);
    public static readonly Vec2Int One = new(1, 1);
    public static readonly Vec2Int Zero = new(0, 0);

    public Vec2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    #endregion

    #region Algebras and Methods
    
    /// <summary>
    /// Square root of the sum of x and y squared.
    /// </summary>
    /// <returns>Magnitude (Length)</returns>
    public double Magnitude()
        => Math.Sqrt(x ^ 2 + y ^ 2);
    
    /// <summary>Returns the inverse of this vector.</summary>
    /// <returns>The vector inverted on both axis.</returns>
    public Vec2Int Inverse()
        => this * -1;

    /// <summary>Calculates the distance between one vector and the other.</summary>
    /// <param name="start">Start vector.</param>
    /// <param name="end">End vector.</param>
    /// <returns>Distance between one vector and another expressed as a vector.</returns>
    public static Vec2Int Distance(Vec2Int start, Vec2Int end)
        => end - start;

    /// <summary>Calculate the distance between this vector and another.</summary>
    /// <param name="other">Target vector to measure to.</param>
    /// <returns>Difference between this vector and another.</returns>
    public Vec2Int Distance(Vec2Int other)
        => other - this;

    /// <summary>
    /// Sum of the multiplications between x and y of two vectors.
    /// </summary>
    /// <returns>Dot product between both vectors</returns>
    public static int DotProduct(Vec2Int a, Vec2Int b)
        => a.x * b.x + a.y * b.y;
    
    /// <summary>
    /// Sum of the multiplications between this and a target vector.
    /// </summary>
    /// <param name="other">Target vector.</param>
    /// <returns>Dot product between both vectors</returns>
    public int DotProduct(Vec2Int other)
        => other.x * x + other.y + y;
    
    /// <summary>
    /// Gets the maximum value on each axis between two vectors.
    /// </summary>
    /// <param name="a">First vector</param>
    /// <param name="b">Second vector</param>
    /// <returns>A new vector with the maximum x and maximum y between both values.</returns>
    public static Vec2Int Max(Vec2Int a, Vec2Int b)
        => new(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
    
    /// <summary>
    /// Gets the maximum value on each axis between two vectors.
    /// </summary>
    /// <param name="other">The vector being compared</param>
    /// <returns>A new vector with the maximum x and maximum y between both values.</returns>
    public Vec2Int Max(Vec2Int other)
        => new(Math.Max(other.x, x), Math.Max(other.y, y));
    
    /// <summary>
    /// Gets the minimum value on each axis between two vectors.
    /// </summary>
    /// <param name="a">First vector</param>
    /// <param name="b">Second vector</param>
    /// <returns>A new vector with the minimum x and minimum y between both values.</returns>
    public static Vec2Int Min(Vec2Int a, Vec2Int b)
        => new Vec2Int(Math.Min(a.x, b.x), Math.Min(a.y, b.y));
    
    /// <summary>
    /// Gets the minimum value on each axis between two vectors.
    /// </summary>
    /// <param name="other">The vector being compared</param>
    /// <returns>A new vector with the minimum x and minimum y between both values.</returns>
    public Vec2Int Min(Vec2Int other)
        => new Vec2Int(Math.Min(other.x, x), Math.Min(other.y, y));
    
    /// <summary>
    /// Assesses if the two vectors are equal to one another based on their axis.
    /// </summary>
    /// <param name="other">The other vector to assess.</param>
    /// <returns><c>true</c> if both vectors are equal on each axis.</returns>
    public bool Equals(Vec2Int other)
        => x == other.x && y == other.y;

    /// <summary>
    /// Assesses if the this vector and an object are equal to one another based on their axis.
    /// </summary>
    /// <param name="obj">The object to assess.</param>
    /// <returns><c>true</c> if <paramref name="obj"/> can be casted to <see cref="Vec2Int"/>
    /// and both vectors are equal on each axis.</returns>
    public override bool Equals(object? obj)
        => obj is Vec2Int other && Equals(other);

    public override int GetHashCode() 
        => HashCode.Combine(x, y);
    
    public override string ToString() => $"[{x}, {y}]";
    
    #endregion

    #region Operators

    public static Vec2Int operator -(Vec2Int a, Vec2Int b)
        => new(a.x - b.x, a.y - b.y);

    public static Vec2Int operator +(Vec2Int a, Vec2Int b)
        => new(a.x + b.x, a.y + b.y);

    public static Vec2Int operator *(Vec2Int a, Vec2Int b)
        => new(a.x * b.x, a.y * b.y);
    
    public static Vec2Int operator *(Vec2Int v, int m)
        => new(v.x * m, v.y * m);
    
    public static Vector2 operator *(Vec2Int v, float m)
        => new(v.x * m, v.y * m);

    public static Vec2Int operator ^(Vec2Int v, int m)
        => new(v.x ^ m, v.y ^ m);

    public static bool operator ==(Vec2Int a, Vec2Int b)
        => (a.x == b.x) & (a.y == b.y);

    public static bool operator !=(Vec2Int a, Vec2Int b)
        => (a.x != b.x) | (a.y != b.y);

    public static implicit operator Vector2(Vec2Int v)
        => new(v.x, v.y);

    #endregion
}