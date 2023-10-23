using Microsoft.Xna.Framework;

namespace FrogeEngine;

/// <summary>
/// The Component class is used to give gameObjects functionality. All components are provided with a few base methods
/// like Initialize (called on creation), Start (called when the GameObject starts), Update, Draw and more.
/// </summary>
public abstract class Component
{
    ///<value>Component.GameObject returns the gameObject containing this component.</value>
    public GameObject GameObject { get; private set; }
    
    ///<value>Component.Transform returns the Transform component of the gameObject containing this component.</value>
    public Transform Transform => GameObject.Transform;

    /// <summary>
    /// The constructor of the component should always be empty. A component having a constructor is considered illegal.
    /// </summary>
    public Component() { }
    
    /// <summary>
    /// Calls the initialization phase of the component.
    /// </summary>
    /// <param name="g">The gameObject containing the component</param>
    public void Init (GameObject g)
    {
        GameObject ??= g;
        Initialize();
    }

    /// <summary>
    /// Initializes the component. Can be overridden to add parameters, but should be called manually if this is done.
    /// </summary>
    public virtual void Initialize()
    {
        
    }
    
    /// <summary>
    /// Calls the start phase of the component. Should be overridden if necessary, but should not be called manually.
    /// Component handling is done by the containing gameObject.
    /// </summary>
    public virtual void Start(){ }
    
    /// <summary>
    /// Calls the update phase of the component. Should be overridden if necessary, but should not be called manually.
    /// Component handling is done by the containing gameObject.
    /// </summary>
    /// <param name="gameTime">A parameter given by MonoGame.</param>
    public virtual void Update(GameTime gameTime) { }

    /// <summary>
    /// Calls the draw phase of the component. Should not be overridden; Use a Renderer component to draw gameObjects.
    /// Should not be called manually; Component handling is done by the containing gameObject.
    /// </summary>
    /// <param name="gameTime"></param>
    public virtual void Draw(GameTime gameTime) { }

    /// <summary>
    /// Calls the destroy phase of the component. Should be overridden if necessary, but should not be called manually.
    /// Component handling is done by the containing gameObject.
    /// </summary>
    public virtual void OnDestroy() { }

    /// <summary>
    /// Self destructs the current component, while keeping the gameObject intact.
    /// </summary>
    public void Destroy()
    {
        GameObject.DestroyComponent(this);
    }

    public Component DeepCopy()
    {
        var newComponent = (Component) MemberwiseClone();
        newComponent.GameObject = null;
        return newComponent;
    }
}