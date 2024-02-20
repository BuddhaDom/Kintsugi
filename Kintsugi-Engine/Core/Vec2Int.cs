namespace Kintsugi.Core;

public struct Vec2Int(int x, int y)
{
    public int x { get; } = x;
    public int y { get; } = y;

    #region Static Properties

    public static readonly Vec2Int Up = new(0, 1);
    public static readonly Vec2Int Down = new(0, -1);
    public static readonly Vec2Int Left = new(-1, 0);
    public static readonly Vec2Int Right = new(1, 0);
    public static readonly Vec2Int One = new(1, 1);
    public static readonly Vec2Int Zero = new(0, 0);
    
    #endregion

    #region Algebras and Methods
    
    /// <summary>
    /// Square root of the sum of x and y squared.
    /// </summary>
    /// <returns>Magnitude (Length)</returns>
    public double Magnitude()
        => Math.Sqrt(x ^ 2 + y ^ 2);
    
    /// <returns>The vector inverted on both axis.</returns>
    public Vec2Int Inverse()
        => new Vec2Int(-x, -y);
    
    /// <returns>Offset between one vector and another.</returns>
    /// <param name="start">Start vector.</param>
    /// <param name="end">End vector.</param>
    public static Vec2Int Distance(Vec2Int start, Vec2Int end)
        => new Vec2Int(end.x - start.x, end.y - start.y);

    /// <returns>Offset between this vector and another.</returns>
    /// <param name="other">Target vector to measure to.</param>
    public Vec2Int Distance(Vec2Int other)
        => new Vec2Int(other.x - x, other.y - y);

    /// <summary>
    /// Sum of the multiplications between x and y of two vectors.
    /// </summary>
    /// <returns>Dot product between both vectors</returns>
    public static int DotProduct(Vec2Int a, Vec2Int b)
        => a.x * b.x + a.y + b.y;
    
    /// <summary>
    /// Sum of the multiplications between this and a target vector.
    /// </summary>
    /// <param name="other">Target vector.</param>
    /// <returns>Dot product between both vectors</returns>
    public int DotProduct(Vec2Int other)
        => other.x * x + other.y + y;
    
    /// <returns>Maximum X and Y between two vectors.</returns>
    public static Vec2Int Max(Vec2Int a, Vec2Int b)
        => new Vec2Int(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
    
    /// <returns>Maximum X and Y between this and another vector..</returns>
    public Vec2Int Max(Vec2Int other)
        => new Vec2Int(Math.Max(other.x, x), Math.Max(other.y, y));
    
    /// <returns>Minimum X and Y between two vectors.</returns>
    public static Vec2Int Min(Vec2Int a, Vec2Int b)
        => new Vec2Int(Math.Min(a.x, b.x), Math.Min(a.y, b.y));
    
    /// <returns>Minimum X and Y between this and another vector.</returns>
    public Vec2Int Min(Vec2Int other)
        => new Vec2Int(Math.Min(other.x, x), Math.Min(other.y, y));
    
    public bool Equals(Vec2Int other)
        => x == other.x && y == other.y;

    public override bool Equals(object? obj)
        => obj is Vec2Int other && Equals(other);

    public override int GetHashCode() 
        => HashCode.Combine(x, y);
    
    public override string ToString() => $"[{x}, {y}]";
    
    #endregion

    #region Operators

    public static Vec2Int operator -(Vec2Int a, Vec2Int b)
        => new Vec2Int(a.x - b.x, a.y - b.y);

    public static Vec2Int operator +(Vec2Int a, Vec2Int b)
        => new Vec2Int(a.x + b.x, a.y + b.y);

    public static Vec2Int operator *(Vec2Int a, Vec2Int b)
        => new Vec2Int(a.x * b.x, a.y * b.y);
    
    public static Vec2Int operator *(Vec2Int v, int m)
        => new Vec2Int(v.x * m, v.y * m);

    public static bool operator ==(Vec2Int a, Vec2Int b)
        => (a.x == b.x) & (a.y == b.y);

    public static bool operator !=(Vec2Int a, Vec2Int b)
        => (a.x != b.x) | (a.y != b.y);
    
    #endregion
}