using System;
using System.Numerics;

namespace FrogeEngine.Mathematics;

public class Line //todo:rewrite to allow vertical and horizontal shit
{
    // A Line is a geometrical concept defining a straight connection between two points.
    // This class is a code-equivalent, with methods and properties similar to a geometrical line in vector representation.
    // Used in classes like Transform to define the sides of an object, or raycasts.
    // The line is defined by two absolute points; From these points an algebraic vector-equation is calculated, with a min and max range.
    
    public Vector2 Origin;
    public Vector2 LineVector;
    
    // A function of a straight line can always be written as (x,y) = (Xo,Yo) + a*(Xv,Yv)
    // Xo (X Origin) and Yo (Y Origin) is point 1;
    // Xv (X Vector) and Yv (Y Vector) is the vector from point 1 to point 2.
    // a is a float between 0 and 1.
    
    
    public Line(Vector2 point1, Vector2 point2)
    {
        Initialize(point1.X, point1.Y, point2.X, point2.Y);
    }
    public Line(float x1, float y1, float x2, float y2)
    {
        Initialize(x1, y1, x2, y2);
    }

    private void Initialize(float x1, float y1, float x2, float y2)
    {
        Origin = new(x1, y1);
        Vector2 endpoint = new(x2, y2);
        LineVector = endpoint - Origin;
    }

    public Vector2 PointFromX(float x)
    {
        if(LineVector.X == 0) { return new(Int32.MinValue); } // Vertical line has multiple values on Origin.X, so impossible return
        var localX = x - Origin.X;
        var a = x / LineVector.X;
        if(a < 0 || a > 1) { return new(Int32.MinValue); } // x is outside bounds, so impossible return

        return Origin + a * LineVector;
    }

    public Vector2 PointFromY(float y)
    {
        if(LineVector.Y == 0) { return new(Int32.MinValue); } // Vertical line has multiple values on Origin.Y, so impossible return
        var localX = y - Origin.Y;
        var a = y / LineVector.Y;
        if(a < 0 || a > 1) { return new(Int32.MinValue); } // x is outside bounds, so impossible return

        return Origin + a * LineVector;
    }

    public bool Intersects(Line line)
    {
        // Vectors intersect if V1V2⊥ != 0 and a1 and a2 are both between 0 and 1, with a1/a2 the magnitudes of each vector where they intersect.
        // a1 = (O2-O1)V2⊥ / V1V2⊥ = DotProd(V2⊥, (O2-O1)) / DotProd(V2⊥, V1)
        // a2 = (O1-O2)V1⊥ / V2V1⊥ = DotProd(V1⊥, (O1-O2)) / DotProd(V1⊥, V2)
        // This gives:
        
        // Calculating V1V2⊥ (if this is 0, V1 and V2 are parallel)
        Vector2 V2p = new(-line.LineVector.Y, line.LineVector.X);
        float V1V2p = Vector2.Dot(LineVector, V2p);
        if (V1V2p == 0) { return false; }
        
        // Calculating a1
        Vector2 O2mO1 = line.Origin - Origin;
        float a1 = Vector2.Dot(V2p, O2mO1) / V1V2p;
        if(a1 < 0 || a1 > 1) { return false; }
        
        // Calculating a2
        Vector2 V1p = new(-LineVector.Y, LineVector.X);
        Vector2 O1mO2 = Origin - line.Origin;
        float V2V1p = Vector2.Dot(line.LineVector, V1p);
        float a2 =  Vector2.Dot(V1p, O1mO2) / V2V1p;
        if(a2 < 0 || a2 > 1) { return false; }
        
        // If all calculations and conditions passed, the vectors intersect.
        return true;
    }
    
    
}