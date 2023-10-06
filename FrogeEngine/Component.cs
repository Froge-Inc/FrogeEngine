using Microsoft.Xna.Framework;

namespace FrogeEngine;

public class Component
{
    public GameObject Parent { get; private set; }

    public Component(GameObject caller)
    {
        Parent = caller;
    }
    
    public virtual void Update(GameTime gameTime) { }
}