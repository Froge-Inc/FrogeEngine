using System.Numerics;
using FrogeEngine.Mathematics;

namespace FrogeEngine;

public class Transform : Component
{
    public Transform(GameObject parent) : base(parent) { }

    // Core Properties
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Scaling { get; set; } = Vector2.One;
    public float Rotation { get; set; } = 0;
    
    
    // Local Properties
        // Points (relative positions)
        public Vector2[] LocalPoints => new[] { LocalBottomLeft, LocalTopLeft, LocalTopRight, LocalBottomRight };
        
        public Vector2 LocalBottomLeft => new(-Scaling.X, -Scaling.Y);
        public Vector2 LocalTopLeft => new(-Scaling.X, Scaling.Y);
        public Vector2 LocalTopRight => new(Scaling.X, Scaling.Y);
        public Vector2 LocalBottomRight => new(Scaling.X, -Scaling.Y);
        
        // Sides (Relative representations)
        public Line[] LocalSides => new[] { LocalLeft, LocalTop, LocalRight, LocalBottom };
        
        public Line LocalLeft => new(LocalBottomLeft, LocalTopLeft);
        public Line LocalTop => new(LocalTopLeft, LocalTopRight);
        public Line LocalRight => new(LocalTopRight, LocalBottomRight);
        public Line LocalBottom => new(LocalBottomRight, LocalBottomLeft);
        
    
    // Global Properties
        // Points (absolute positions). Names refer to the respective points when rotation = 0.
        public Vector2[] Points => new []{ BottomLeft, TopLeft, TopRight, BottomRight };
        
        public Vector2 BottomLeft => Position + Utils.RotatedVector(LocalBottomLeft, Rotation);
        public Vector2 TopLeft => Position + Utils.RotatedVector(LocalTopLeft, Rotation);
        public Vector2 TopRight => Position + Utils.RotatedVector(LocalTopRight, Rotation);
        public Vector2 BottomRight => Position + Utils.RotatedVector(LocalBottomRight, Rotation);
    
        // Sides (absolute representations). Names refer to the respective side when rotation = 0.
        public Line[] Sides => new[] { Left, Top, Right, Bottom };
        
        public Line Left => new(BottomLeft, TopLeft);
        public Line Top => new(TopLeft, TopRight);
        public Line Right => new(TopRight, BottomRight);
        public Line Bottom => new(BottomRight, BottomLeft);


    // Public Methods
    public void Translate(Vector2 translation) { Position += translation; }
    public void Reposition() { Position = Vector2.Zero; }

    public void Scale(Vector2 scale) { Scaling *= scale; }
    public void Rescale() { Scaling = Vector2.One; }
    
    public void Rotate(float rotation) { Rotation += rotation; }
    public void Orient() { Rotation = 0; }

    public void Reset() { Reposition(); Rescale(); Orient(); }

    public void Align(Transform transform,
        bool affectPosition = true,
        bool affectScaling = true,
        bool affectRotation = true)
    {
        if(affectPosition) { Position = transform.Position; }
        if(affectScaling)  { Scaling  = transform.Scaling;  }
        if(affectRotation) { Rotation = transform.Rotation; }
    }
    
    // Private Methods

}