using System.Collections.Generic;
using System.Numerics;
using FrogeEngine.Mathematics;

namespace FrogeEngine;

public class Transform : Component
{
    public bool Translated { get; set; } = false;
    public Transform Parent { get; private set; }
    private List<Transform> Children { get; set; }
    
    // Core Properties
    // POSITION ---------------------------------------------
        private Vector2 _position;
        public Vector2 Position
        {
            get => _changedPos ? UpdatePosition() : _position;
            set => UpdateLocalPosition(value);
        }
        private Vector2 _localPosition;
        public Vector2 LocalPosition
        {
            get => _localPosition;
            set
            {
                _localPosition = value;
                _changedPos = true;
                Translated = true;
            }
        }
        private bool _changedPos = true;
    
    // ROTATION ---------------------------------------------
        private Vector2 _scaling;
        public Vector2 Scaling
        {
            get => _changedPos ? UpdateScaling() : _position;
            set => UpdateLocalScaling(value);
        }
        private Vector2 _localScaling;
        public Vector2 LocalScaling
        {
            get => _localScaling;
            set
            {
                _localScaling = value;
                _changedRot = true;
                Translated = true;
            }
        }
        private bool _changedScl = true;
        

    
    // ROTATION ---------------------------------------------
        private float _rotation = 0;
        public float Rotation
        {
            get => _changedRot ? UpdateRotation() : _rotation; 
            set => UpdateLocalRotation(value);
        }
        private float _localRotation = 0;
        public float LocalRotation
        {
            get => _localRotation;
            set
            {
                _localRotation = value;
                _changedRot = true;
                Translated = true;
            }
        }
        private bool _changedRot = true;
    
    
    // Local Properties
        // Points (relative positions)
        public Vector2[] LocalPoints => new[] { LocalBottomLeft, LocalTopLeft, LocalTopRight, LocalBottomRight };

        public Vector2 LocalBottomLeft => Utils.RotatedVector(-Scaling / 2, LocalRotation);
        public Vector2 LocalTopLeft => Utils.RotatedVector(Scaling with {X = -Scaling.X} / 2, LocalRotation);
        public Vector2 LocalTopRight => Utils.RotatedVector(Scaling / 2, LocalRotation);
        public Vector2 LocalBottomRight => Utils.RotatedVector(Scaling with {Y = -Scaling.Y} / 2, LocalRotation);
        
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

    private Vector2 UpdatePosition()
    {
        _changedPos = false;
        Vector2 parentVector = Parent?.Position ?? Vector2.Zero;
        _position = parentVector + _localPosition;
        return _position;
    }
    private void UpdateLocalPosition(Vector2 newPosition)
    {
        var changeLocal = newPosition - _position;
        _localPosition += changeLocal;
        _position = newPosition;
        Translated = true;
    }
    
    private Vector2 UpdateScaling()
    {
        _changedPos = false;
        Vector2 parentVector = Parent?.Position ?? Vector2.Zero;
        _position = parentVector + _localPosition;
        return _position;
    }
    private void UpdateLocalScaling(Vector2 newPosition)
    {
        var changeLocal = newPosition - _position;
        _localPosition += changeLocal;
        _position = newPosition;
        Translated = true;
    }
    
    private float UpdateRotation()
    {
        _changedRot = false;
        float parentRotation = Parent?.Rotation ?? 0;
        _rotation = parentRotation + _localRotation;
        return _rotation;
    }
    private void UpdateLocalRotation(float newRotation)
    {
        var changeLocal = newRotation - _rotation;
        _localRotation += changeLocal;
        _rotation = newRotation;
        Translated = true;
    }

}