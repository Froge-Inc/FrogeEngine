using System;
using System.Numerics;

namespace FrogeEngine.Mathematics;

public static class Utils
{



    public static Vector2 RotatedVector(Vector2 vec, float rot)
    {
        // Mathematical rotation of a vector by rot radians.
        float x = vec.X * (float)Math.Cos(rot) - vec.Y * (float)Math.Sin(rot);
        float y = vec.X * (float)Math.Sin(rot) - vec.Y * (float)Math.Cos(rot);
        return new(x,y);
    }
}