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
        Initialize();
    }

    public virtual void Initialize()
    {
        
    }
    
    public virtual void Start(){ }
    
    public virtual void Update(GameTime gameTime) { }

    public virtual void Draw(GameTime gameTime) { }


    public virtual void OnDestroy() { }

    public void Destroy()
    {
        GameObject.DestroyComponent(this);
    }
}