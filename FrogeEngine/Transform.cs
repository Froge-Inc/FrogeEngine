using System.Numerics;

namespace FrogeEngine;

public class Transform : Component
{
    public Transform(GameObject parent) : base(parent) { }

    // Public Properties
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Scale { get; set; } = Vector2.One;
    public float Rotation { get; set; } = 0;
    
    // Private Properties
    
    
    // Public Methods
    
    
    // Private Methods
    
    
}