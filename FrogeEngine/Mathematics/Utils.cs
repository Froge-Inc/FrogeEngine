using System;
using Microsoft.Xna.Framework;


namespace FrogeEngine.Mathematics;

/// <summary>
/// The FrogeEngine.Mathematics package provides various classes containing mathematical components and operations,
/// simplifying code.
/// The Mathematics.Utils class has static functions providing the possibility of various mathematical operations,
/// taking them out of the main codebase simplifying use.
/// </summary>
public static class Utils
{

    /// <summary>
    /// Rotates a vector around it's origin by the given rotation, counterclockwise.
    /// </summary>
    /// <param name="vector">The vector to rotate</param>
    /// <param name="rotation">The rotation to be performed (in radians)</param>
    /// <returns>The rotated vector.</returns>
    public static Vector2 RotatedVector(Vector2 vector, float rotation)
    {
        // Mathematical rotation of a vector by rot radians.
        float x = vector.X * (float)Math.Cos(rotation) - vector.Y * (float)Math.Sin(rotation);
        float y = vector.X * (float)Math.Sin(rotation) + vector.Y * (float)Math.Cos(rotation);
        return new(x,y);
    }

    /// <summary>
    /// Converts a change in degrees to an equivalent change in radians.
    /// </summary>
    /// <param name="degrees">The rotation in degrees</param>
    /// <returns>The rotation in radians.</returns>
    public static float DegToRadians(int degrees)
    {
        return -((float)Math.PI / 180 * degrees);
    }

    /// <summary>
    /// Converts a change in radians to an equivalent change in degrees.
    /// </summary>
    /// <param name="radians">The rotation in radians</param>
    /// <returns>the rotation in degrees.</returns>
    public static int RadiansToDegs(float radians)
    {
        return (int)(radians * 180 / (float)Math.PI);
    }
}