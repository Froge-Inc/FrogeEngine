using System;
using System.Collections.Generic;
using FrogeEngine.Mathematics;
using Microsoft.Xna.Framework;

namespace FrogeEngine;

/// <summary>
/// The Transform component is a crucial component all gameObjects have. They define the position, rotation, scaling
/// and other physical properties of a gameObject, and their Parent/Child hierarchy.
/// Each transform component has a global (root-relative) and local (parent-relative) position, rotation and scaling.
/// These are updated accordingly, when either of the two is read out.
/// The position, rotation and scaling can be interpolated by setting the TargetPosition and -Rotation.
/// The gameObject will automatically interpolate transform to this position.
/// The component also provides information on the physical properties, like references to the global and local sides and corner points.
/// </summary>
public class Transform : Component
{
    ///<value>Transform.Translated returns if any information in the transform changed, indicating other classes should update their stored values.</value>
    public bool Translated { get; set; } = true;
    ///<value>Transform.Parent returns the parent transform of this transform, or null if absent.</value>
    public Transform Parent { get =>_parent; set => SetParent(value); }
    ///<value>Transform._parent stores the parent transform, if present. Inaccessible.</value>
    private Transform _parent;
    ///<value>Transform._easingFrames stores the amount of update loops the object should take to lerp to its targetPosition and -Rotation.</value>
    private const int _easingFrames = 5;
    ///<value>Transform._targetPosition stores the target position to lerp to, or null if absent. Nullable, inaccessible.</value>
    private Vector2? _targetPosition = null;
    ///<value>Transform._targetRotation stores the target rotation to lerp to, or null if absent. Nullable, inaccessible.</value>
    private float? _targetRotation = null;
    ///<value>Transform._targetScale stores the target scaling to lerp to, or null if absent. Nullable, inaccessible.</value>
    private Vector2? _targetScale = null;

    ///<value>Transform.TargetPosition returns the target position, or the current position if absent.</value>
    public Vector2 TargetPosition
    {
        get => _targetPosition ?? Position;
        set => _targetPosition = value;
    }
    ///<value>Transform.TargetRotation returns the target rotation, or the current rotation if absent.</value>
    public float TargetRotation
    {
        get => _targetRotation ?? Rotation;
        set => _targetRotation = value;
    }

    ///<summary>Cancels the target transform, clearing (nulling) it, cancelling all lerping in the process..</summary>
    public void CancelTarget()
    {
        _targetPosition = null;
        _targetRotation = null;
    }
    
    ///<value>Transform.Children returns all child transforms.</value>
    private List<Transform> Children { get; set; } = new();
    
    // Core Properties
    // POSITION ---------------------------------------------
        ///<value>Transform._position stores the position of the gameObject. Inaccessible.</value>
        private Vector2 _position;
        ///<value>Transform.Position returns the position of the gameObject, and updates the localPosition when set.</value>
        public Vector2 Position
        {
            get => _translated ? UpdatePosition() : _position;
            set => UpdateLocalPosition(value);
        }
        ///<value>Transform._localPosition stores the local position of the gameObject. Inaccessible.</value>
        private Vector2 _localPosition= new(0, 0);
        ///<value>Transform.LocalPosition returns the local position of the gameObject, and prompts the Position to be updated when set.</value>
        public Vector2 LocalPosition
        {
            get => _localPosition;
            set
            {
                _localPosition = value;
                _translated = true;
                UpdateAllChildren();
            }
        }
    
    // SCALING ---------------------------------------------
        ///<value>Transform._scaling stores the scaling of the gameObject. Inaccessible.</value>
        private Vector2 _scaling = Vector2.One;
        ///<value>Transform.Scaling returns the scaling of the gameObject, and updates the localScaling when set.</value>
        public Vector2 Scaling
        {
            get => _translated ? UpdateScaling() : _scaling;
            set => UpdateLocalScaling(value);
        }
        ///<value>Transform._localScaling stores the local scaling of the gameObject. Inaccessible.</value>
        private Vector2 _localScaling = Vector2.One;
        ///<value>Transform.LocalScaling returns the local scaling of the gameObject, and prompts the Scaling to be updated when set.</value>
        public Vector2 LocalScaling
        {
            get => _localScaling;
            set
            {
                _localScaling = value;
                _translated = true;
                UpdateAllChildren();
            }
        }
    
    // ROTATION ---------------------------------------------
        ///<value>Transform._rotation stores the rotation of the gameObject. Inaccessible.</value>
        private float _rotation = 0;
        ///<value>Transform.Rotation returns the rotation of the gameObject, and updates the localRotation when set.</value>
        public float Rotation
        {
            get => _translated ? UpdateRotation() : _rotation; 
            set => UpdateLocalRotation(value);
        }
        ///<value>Transform._localRotation stores the local rotation of the gameObject. Inaccessible.</value>
        private float _localRotation = 0;
        ///<value>Transform.LocalRotation returns the local rotation of the gameObject, and prompts the Rotation to be updated when set.</value>
        public float LocalRotation
        {
            get => _localRotation;
            set
            {
                _localRotation = value;
                _translated = true;
                UpdateAllChildren();
            }
        }
        
        ///<value>Transform._translated stores if the transform was changed in any way, prompting values to be updated when read.</value>
        private bool _translated = true;

        ///<summary>Updates all transforms, and updates all children recursively.</summary>
        public void UpdateAllTransforms()
        {
            _translated = true;
            UpdateAllChildren();
        }

        /// <summary>
        /// Updates all children transforms recursively.
        /// </summary>
        private void UpdateAllChildren()
        {
            foreach (Transform child in GetChildren())
            {
                child.UpdateAllTransforms();
            }
        }
    
    // Local Properties
        // Points (relative positions)
        ///<value>Transform.LocalPoints returns the local position of all corner points.</value>
        public Vector2[] LocalPoints => new[] { LocalBottomLeft, LocalTopLeft, LocalTopRight, LocalBottomRight };

        ///<value>Transform.LocalBottomLeft returns the local position of the bottom left corner.</value>
        public Vector2 LocalBottomLeft => Utils.RotatedVector(-Scaling / 2, LocalRotation);
        ///<value>Transform.LocalTopLeft returns the local position of the top left corner.</value>
        public Vector2 LocalTopLeft => Utils.RotatedVector(new Vector2(-Scaling.X, Scaling.Y) / 2, LocalRotation);
        ///<value>Transform.LocalTopRight returns the local position of the top right corner.</value>
        public Vector2 LocalTopRight => Utils.RotatedVector(Scaling / 2, LocalRotation);
        ///<value>Transform.LocalBottomRight returns the local position of the bottom right corner.</value>
        public Vector2 LocalBottomRight => Utils.RotatedVector(Scaling with {Y = -Scaling.Y} / 2, LocalRotation);
        
        // Sides (Relative representations)
        ///<value>Transform.LocalSides returns the local FrogeEngine.Mathematics.Line representation of all sides of the gameObject.</value>
        public Line[] LocalSides => new[] { LocalLeft, LocalTop, LocalRight, LocalBottom };
        
        ///<value>Transform.LocalLeft returns the local FrogeEngine.Mathematics.Line representation of the left side of the gameObject.</value>
        public Line LocalLeft => new(LocalBottomLeft, LocalTopLeft);
        ///<value>Transform.LocalTop returns the local FrogeEngine.Mathematics.Line representation of the top side of the gameObject.</value>
        public Line LocalTop => new(LocalTopLeft, LocalTopRight);
        ///<value>Transform.LocalRight returns the local FrogeEngine.Mathematics.Line representation of the right side of the gameObject.</value>
        public Line LocalRight => new(LocalTopRight, LocalBottomRight);
        ///<value>Transform.LocalBottom returns the local FrogeEngine.Mathematics.Line representation of the bottom side of the gameObject.</value>
        public Line LocalBottom => new(LocalBottomRight, LocalBottomLeft);
        
    
    // Global Properties
        // Points (global positions). Names refer to the respective points when rotation = 0.
        ///<value>Transform.Points returns the global position of all corner points of the gameObject.</value>
        public Vector2[] Points => new []{ BottomLeft, TopLeft, TopRight, BottomRight };
        
        ///<value>Transform.BottomLeft returns the global position of the bottom left corner point.</value>
        public Vector2 BottomLeft => Position + Utils.RotatedVector(LocalBottomLeft, Rotation);
        ///<value>Transform.TopLeft returns the global position of the top left corner point.</value>
        public Vector2 TopLeft => Position + Utils.RotatedVector(LocalTopLeft, Rotation);
        ///<value>Transform.TopRight returns the global position of the top right corner point.</value>
        public Vector2 TopRight => Position + Utils.RotatedVector(LocalTopRight, Rotation);
        ///<value>Transform.BottomRight returns the global position of the bottom right corner point.</value>
        public Vector2 BottomRight => Position + Utils.RotatedVector(LocalBottomRight, Rotation);
    
        // Sides (global representations). Names refer to the respective side when rotation = 0.
        ///<value>Transform.Sides returns the global FrogeEngine.Mathematics.Line representation of all sides of the gameObject.</value>
        public Line[] Sides => new[] { Left, Top, Right, Bottom };
        
        ///<value>Transform.Left returns the global FrogeEngine.Mathematics.Line representation of the left side of the gameObject.</value>
        public Line Left => new(BottomLeft, TopLeft);
        ///<value>Transform.Top returns the global FrogeEngine.Mathematics.Line representation of the top side of the gameObject.</value>
        public Line Top => new(TopLeft, TopRight);
        ///<value>Transform.Right returns the global FrogeEngine.Mathematics.Line representation of the right side of the gameObject.</value>
        public Line Right => new(TopRight, BottomRight);
        ///<value>Transform.Bottom returns the global FrogeEngine.Mathematics.Line representation of the bottom side of the gameObject.</value>
        public Line Bottom => new(BottomRight, BottomLeft);


    // Public Methods
    
    /// <summary>
    /// Calls the update phase of the component. Should never be run manually; The gameObject does component handling.
    /// </summary>
    /// <param name="gameTime">A parameter provided by MonoGame</param>
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        TranslateToTarget();
    }
    
    /// <summary>
    /// Moves the gameObject with the given Vector2 (globally).
    /// </summary>
    /// <param name="translation">The movement to be carried out on the gameObject.</param>
    public void Translate(Vector2 translation) { Position += translation; }
    
    /// <summary>
    /// Resets the global Position to (0,0).
    /// </summary>
    public void Reposition() { Position = Vector2.Zero; }

    /// <summary>
    /// Scales the gameObject with the given Vector2 (globally).
    /// </summary>
    /// <param name="scale">The scale to be carried out on the gameObject.</param>
    public void Scale(Vector2 scale) { Scaling *= scale; }
    
    /// <summary>
    /// Resets the global Scaling to (1,1).
    /// </summary>
    public void Rescale() { Scaling = Vector2.One; }
    
    /// <summary>
    /// Rotates the gameObject with the given float (globally).
    /// </summary>
    /// <param name="rotation">The rotation to be carried out on the gameObject in radians.</param>
    public void Rotate(float rotation) { Rotation += rotation; }
    
    /// <summary>
    /// Resets the global Rotation to 0.
    /// </summary>
    public void Orient() { Rotation = 0; }

    /// <summary>
    /// Resets the global transforms to their neutral values (Position (0,0), Scaling (1,1), Rotation 0).
    /// </summary>
    public void Reset() { Reposition(); Rescale(); Orient(); }

    /// <summary>
    /// Copies the transform properties of the given transform to the current transform.
    /// </summary>
    /// <param name="transform">The transform object to copy the properties from</param>
    /// <param name="affectPosition">Determine if the position should be affected -  Defaults to True</param>
    /// <param name="affectScaling">Determine if the scaling should be affected -  Defaults to True</param>
    /// <param name="affectRotation">Determine if the rotation should be affected -  Defaults to True</param>
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

    /// <summary>
    /// Updates the Position if the localPosition was changed. Inaccessible.
    /// </summary>
    /// <returns>The newly calculated position.</returns>
    private Vector2 UpdatePosition()
    {
        Vector2 parentVector = Parent?.Position ?? Vector2.Zero;
        Vector2 rotatedLocalPos = Utils.RotatedVector(_localPosition, -Rotation);
        _position = parentVector + rotatedLocalPos;
        _translated = false;
        return _position;
    }
    
    /// <summary>
    /// Updates the localPosition if the Position was changed. Inaccessible.
    /// </summary>
    /// <param name="newPosition">New global position</param>
    private void UpdateLocalPosition(Vector2 newPosition)
    {
        var changeLocal = newPosition - _position;
        _localPosition += changeLocal;
        _position = newPosition;
        UpdateAllChildren();
    }
    
    /// <summary>
    /// Updates the Scaling if the localScaling was changed. Inaccessible.
    /// </summary>
    /// <returns>The newly calculated scaling.</returns>
    private Vector2 UpdateScaling()
    {
        Vector2 parentVector = Parent?.Scaling ?? Vector2.One;
        _scaling = parentVector * _localScaling;
        _translated = false;
        return _scaling;
    }
    
    /// <summary>
    /// Updates the localScaling if the Scaling was changed. Inaccessible.
    /// </summary>
    /// <param name="newScaling">New global scaling</param>
    private void UpdateLocalScaling(Vector2 newScaling)
    {
        var changeLocal = newScaling / _scaling;
        _localScaling *= changeLocal;
        _scaling = newScaling;
        UpdateAllChildren();
    }
    
    /// <summary>
    /// Updates the Rotation if the localRotation was changed. Inaccessible.
    /// </summary>
    /// <returns>The newly calculated rotation.</returns>
    private float UpdateRotation()
    {
        float parentRotation = Parent?.Rotation ?? 0;
        _rotation = parentRotation + _localRotation;
        _translated = false;
        return _rotation;
    }
    
    /// <summary>
    /// Updates the localRotation if the Rotation was changed. Inaccessible.
    /// </summary>
    /// <param name="newRotation">New global rotation.</param>
    private void UpdateLocalRotation(float newRotation)
    {
        var changeLocal = newRotation - Parent?.Rotation ?? _rotation;
        _localRotation += changeLocal;
        _rotation = newRotation;
        UpdateAllChildren();
    }

    /// <summary>
    /// Set the parent of the current transform.
    /// </summary>
    /// <param name="parent">The new parent</param>
    public void SetParent(Transform parent)
    {
        _parent = parent;
        _parent?.Children.Add(this);
        if (parent != null) return;
        if (FrogeGame.Game.Scene == null) return;
        FrogeGame.Game.Scene.UpdateParent(this);
    }
    
    /// <summary>
    /// Update the list of children if any of their parent transforms were changed.
    /// </summary>
    public void UpdateChildren()
    {
        foreach (var child in Children.ToArray())
        {
            if (child.Parent == this) continue;
            Children.Remove((child));
        }
    }
    
    /// <summary>
    /// Interpolated translate to the targetPosition and targetRotation, if available.
    /// </summary>
    private void TranslateToTarget()
    {
        if (Vector2.Distance(TargetPosition, Position) != 0)
        {
            Vector2 dir = TargetPosition - Position;
            Translate(dir / _easingFrames);
        }

        float rotationOffset = (TargetRotation - Rotation + (float)Math.PI) % ((float)Math.PI * 2) - (float)Math.PI;
        rotationOffset += rotationOffset < -(float)Math.PI ? (float)Math.PI * 2 : 0;
        if (rotationOffset != 0)
        {
            Rotate(rotationOffset / _easingFrames);
        }
    }

    /// <summary>
    /// Calculate the relative position of the given position to the current position.
    /// </summary>
    /// <param name="position">The global position to turn into a relative position</param>
    /// <returns>The relative position from the given global position.</returns>
    public Vector2 GetRelativePosition(Vector2 position)
    {
        var distance = position - Position;

        var rotatedPosition = GetRotatedPosition(distance);

        return rotatedPosition;
    }

    /// <summary>
    /// Calculate the world position of the given position relative to the current position/
    /// </summary>
    /// <param name="relativePosition">The position to turn into a global position</param>
    /// <returns>The global position from the given relative position.</returns>
    public Vector2 GetWorldPosition(Vector2 relativePosition)
    {
        var rotatedPos = GetRotatedPosition(-relativePosition);

        var worldPos = rotatedPos + Position;
        return worldPos * _scaling;
    }

    /// <summary>
    /// Calculate the position 
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector2 GetRotatedPosition(Vector2 position)
    {
        var cosAngle = MathF.Cos(Rotation);
        var sinAngle = MathF.Sin(Rotation);

        var rotatedPosition = new Vector2(
            position.X * cosAngle - position.Y * sinAngle,
            position.X * sinAngle - position.Y * cosAngle
        );

        return rotatedPosition;
    }
    
    /// <summary>
    /// Get all children of the current transform.
    /// </summary>
    /// <returns>All children of the current transform.</returns>
    public Transform[] GetChildren()
    {
        return Children.ToArray();
    }

    
}