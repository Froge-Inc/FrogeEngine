using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace FrogeEngine.Components.Collider;

/// <summary>
/// An incomplete class. Will be used as base for colliders, components used to check collisions between gameObjects..
/// </summary>
public abstract class Collider : Component
{
    //TODO:Revamp collision handling (static list with tuples, avoid duplicate checks, events?
    protected List<Collider> _collisions = new();
    private List<Collider> _prevCollisions = new();
    public bool Active { get; set; } = true;
    
    public sealed override void Update(GameTime gameTime)
    {
        if (!Active) return;
        
        //for each gameobject with collider run
        //CheckCollisionPossibility(g);

        CleanCollisionList();
    }
    
    protected virtual void CheckCollisionPossibility(GameObject g) {}

    private void CleanCollisionList()
    {
        List<Collider> entered = _collisions.Except(_prevCollisions).ToList();
        foreach (Collider c in entered) { CollisionEntered(c.GameObject); }
        
        List<Collider> exists = _collisions.Intersect(_prevCollisions).ToList();
        foreach (Collider c in exists) { CollisionExists(c.GameObject); }
        
        List<Collider> exited = _prevCollisions.Except(_collisions).ToList();
        foreach (Collider c in exited) {CollisionExited(c.GameObject); }
        
        _prevCollisions = _collisions;
        _collisions = new();
    }
    
    private void CollisionEntered(GameObject g) { g.OnCollisionEnter(GameObject); }
    private void CollisionExists(GameObject g) { g.OnCollision(GameObject); }
    private void CollisionExited(GameObject g) { g.OnCollisionExit(GameObject); }
}