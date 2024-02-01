namespace Kintsugi.Core;

public struct Vec2Int(int x, int y)
{
    public int x { get; } = x;
    public int y { get; } = y;

    #region Static Properties

    public static Vec2Int Up => new Vec2Int(0, 1);
    public static Vec2Int Down => new Vec2Int(0, -1);
    public static Vec2Int Left => new Vec2Int(-1, 0);
    public static Vec2Int Right => new Vec2Int(1, 0);
    public static Vec2Int One => new Vec2Int(1, 1);
    public static Vec2Int Zero => new Vec2Int(0, 0);
    
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
    /// <param name="vector">Target vector to measure to.</param>
    public Vec2Int Distance(Vec2Int vector)
        => new Vec2Int(vector.x - x, vector.y - y);

    /// <summary>
    /// Sum of the multiplications between x and y of two vectors.
    /// </summary>
    /// <returns>Dot product between both vectors</returns>
    public static int DotProduct(Vec2Int a, Vec2Int b)
        => a.x * b.x + a.y + b.y;
    
    /// <summary>
    /// Sum of the multiplications between this and a target vector.
    /// </summary>
    /// <param name="vector">Target vector.</param>
    /// <returns>Dot product between both vectors</returns>
    public int DotProduct(Vec2Int vector)
        => vector.x * x + vector.y + y;
    
    /// <returns>Maximum X and Y between two vectors.</returns>
    public static Vec2Int Max(Vec2Int a, Vec2Int b)
        => new Vec2Int(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
    
    /// <returns>Maximum X and Y between this and another vector..</returns>
    public Vec2Int Max(Vec2Int vector)
        => new Vec2Int(Math.Max(vector.x, x), Math.Max(vector.y, y));
    
    /// <returns>Minimum X and Y between two vectors.</returns>
    public static Vec2Int Min(Vec2Int a, Vec2Int b)
        => new Vec2Int(Math.Min(a.x, b.x), Math.Min(a.y, b.y));
    
    /// <returns>Minimum X and Y between this and another vector.</returns>
    public Vec2Int Min(Vec2Int vector)
        => new Vec2Int(Math.Min(vector.x, x), Math.Min(vector.y, y));
    
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
    
    public override string ToString() => $"[{x}, {y}]";
}