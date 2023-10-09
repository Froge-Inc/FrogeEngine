using System.Collections;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace FrogeEngine;

public abstract class Component
{
    public GameObject GameObject { get; private set; }
    public Transform Transform => GameObject.Transform;

    public Component() { }
    
    public void Init (GameObject g)
    {
        GameObject ??= g;
    }
    
    public virtual void Update(GameTime gameTime) { }
}