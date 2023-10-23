using System;
using Microsoft.Xna.Framework;

namespace FrogeEngine.Components;

/// <summary>
/// An incomplete class. Will be used to handle collisions between non-rotated quad colliders and other colliders.
/// </summary>
public class AlignedQuadCollider : Collider
{
    protected override void CheckCollisionPossibility(GameObject g)
    {
        Vector2 distance = Transform.Position - g.Transform.Position;
        float colDisX = (Transform.Scaling.X + g.Transform.Scaling.X) / 2;
        float colDisY = (Transform.Scaling.Y + g.Transform.Scaling.Y) / 2;
        if (Math.Abs(distance.X) >= colDisX || Math.Abs(distance.Y) >= colDisY) return;
        _collisions.Add(g.GetComponent<Collider>());
    }
}