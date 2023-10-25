using System;
using System.Collections.Generic;
using System.Linq;
using FrogeEngine.Components;
using Microsoft.Xna.Framework;

namespace FrogeEngine;

/// <summary>
/// The GameObject class is used for anything in the game. Its capabilities can be extended using (custom) components.
/// All GameObjects have a Transform. Everything in a game is a GameObject.
/// </summary>
public class GameObject
{
    ///<value>GameObject.Name returns the name of the gameObject.</value>
    public string Name { get => _name; }
    
    ///<value>GameObject._name stores the name of the gameObject. </value>
    private string _name;
    
    ///<value>GameObject.Transform returns the transform component of the GameObject. _components[0] is reserved for the transform component/</value>
    public Transform Transform => _components[0] as Transform; //Components[0] will ALWAYS have a reference in it, hence no checks.
    
    ///<value>GameObject.UpdateRender stores if the gameObjects Render information should be updated (Culling, planned feature).</value>
    public bool UpdateRender { get; set; }

    ///<value>GameObject._components stores the gameObjects components, inaccesible.</value>
    private List<Component> _components;

    
    /// <summary>
    /// Constructs a gameObject. Should only be used when initializing a scene, instead GameObject.CreateGameObject should be used.
    /// </summary>
    /// <param name="name">Name of the gameObject.</param>
    public GameObject(string name)
    {
        _name = name;
        _components = new List<Component>
        {
            new Transform()
        };
        _components[0].Init(this);
    }
    
    /// <summary>
    /// Constructs a parented gameObject. Should only be used when initializing a scene, instead GameObject.CreateGameObject should be used.
    /// </summary>
    /// <param name="name">Name of the gameObject</param>
    /// <param name="parent">Parent transform</param>
    public GameObject(string name, Transform parent)
    {
        _name = name;
        _components = new List<Component>
        {
            new Transform()
        };
        _components[0].Init(this);

        Transform.Parent = parent;
    }
    
    /// <summary>
    /// Creates a gameObject using the private constructors, and adds it to the current scene.
    /// </summary>
    /// <param name="name">Name of the gameObject</param>
    /// <param name="parent">Optional parent transform</param>
    /// <returns>The created gameObject.</returns>
    public static GameObject CreateGameObject(string name, Transform parent = null)
    {
        var gameObject = new GameObject(name, parent);
        
        FrogeGame.Game.Scene.AddGameObject(gameObject, parent);

        return gameObject;
    }
    
    /// <summary>
    /// Looks for a component of the given type, returns null if absent.
    /// </summary>
    /// <typeparam name="T">Component type to look for</typeparam>
    /// <returns>The requested component, null if absent.</returns>
    public T GetComponent<T>() where T: Component
    {
        return _components.OfType<T>().FirstOrDefault();
    }

    /// <summary>
    /// Adds a component of the given type.
    /// </summary>
    /// <typeparam name="T">Component type to add</typeparam>
    /// <returns>The added component</returns>
    /// <exception cref="Exception">Called when an attempt to add an already existing component is made; this is illegal.</exception>
    public T AddComponent<T>() where T: Component, new()
    {
        if (GetComponent<T>() != null)
        {
            throw new Exception("Tried to add an existing component, this is illegal!");
        }
        
        var newComponent = new T();
        _components.Add(newComponent);
        newComponent.Init(this);
        if (FrogeGame.Game.Scene != null) newComponent.Start();
        return newComponent;
    }

    /// <summary>
    /// Destroys a component of the given type, if present.
    /// </summary>
    /// <typeparam name="T">Component type to destroy</typeparam>
    public void DestroyComponent<T>() where T : Component
    {
        Component component = GetComponent<T>();

        DestroyComponent(component);
    }

    /// <summary>
    /// Destroys a given component instance.
    /// </summary>
    /// <param name="component">The component to destroy</param>
    /// <exception cref="Exception">Called when an attempt to delete a transform component is made; this is illegal.</exception>
    public void DestroyComponent(Component component)
    {
        if (component == null) return;

        if (component is Transform)
        {
            throw new Exception("Tried to destroy a transform, this is illegal!");
        }
        
        component.OnDestroy();
        _components.Remove(component);
    }
    
    /// <summary>
    /// Force destroys a component. This is dangerous to use; it will also perform any illegal deletions.
    /// </summary>
    /// <param name="component"></param>
    private void ForceDestroyComponent(Component component)
    {
        if (component == null) return;
        
        component.OnDestroy();
        _components.Remove(component);
    }

    /// <summary>
    /// Calls the start phase of all components. Generally handled by the scene, unless the gameobject is instantiated later.
    /// </summary>
    public void Start()
    {
        foreach (var component in _components.ToArray())
        {
            component.Start();
        }
    }

    /// <summary>
    /// Calls the update phase of all components. While the method is public, it should never be run manually.
    /// GameScene handles gameObject updating.
    /// </summary>
    /// <param name="gameTime"></param>
    public void Update(GameTime gameTime)
    {
        foreach (var component in _components.ToArray())
        {
            component.Update(gameTime);
        }
    }
    
    /// <summary>
    /// Calls the draw phase of all components. While the method is public, it should never be run manually.
    /// GameScene handles gameObject drawing.
    /// </summary>
    /// <param name="gameTime"></param>
    public void Draw(GameTime gameTime)
    {
        foreach (var component in _components.ToArray())
        {
            component.Draw(gameTime);
        }
    }
    
    // events to be called for collision handling. To be added; not functional at the moment.
    public void OnCollisionEnter(GameObject coll) { }
    public void OnCollision(GameObject coll) { }
    public void OnCollisionExit(GameObject coll) { }

    /// <summary>
    /// Destroys this gameObject in the scene.
    /// </summary>
    public void Destroy()
    {
        FrogeGame.Game.Scene.DestroyGameObject(this);
    }

    /// <summary>
    /// Calls the destroy phase of all components. While the method is public, it should never be run manually.
    /// GameScene handles gameObject deletion.
    /// </summary>
    public void OnDestroy()
    {
        foreach (var component in _components)
        {
            component.OnDestroy();
        }
    }

    public GameObject DeepCopy()
    {
        var newGameObject = new GameObject(_name);
        newGameObject._components = new List<Component>();
        foreach (var component in _components)
        {
            var newComponent = component.DeepCopy();
            newComponent.Init(newGameObject);
            newGameObject._components.Add(newComponent);
        }
        return newGameObject;
    }
}