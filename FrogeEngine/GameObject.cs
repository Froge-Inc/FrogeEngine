using System;
using System.Collections.Generic;
using System.Linq;
using FrogeEngine.Components;
using Microsoft.Xna.Framework;
using Vector2 = System.Numerics.Vector2;

namespace FrogeEngine;

public class GameObject
{
    public string Name { get => _name; }
    public Transform Transform => _components[0] as Transform; //Components[0] is reserved for Transform and will ALWAYS have a reference in it, hence no checks.
    public bool UpdateRender { get; set; }

    private string _name;
    private List<Component> _components;

    
    
    public GameObject(string name)
    {
        _name = name;
        _components = new List<Component>()
        {
            new Transform()
        };
    }
    
    public GameObject(string name, Transform parent)
    {
        _name = name;
        _components = new List<Component>()
        {
            new Transform()
        };

        Transform.Parent = parent;
    }
    
    public static GameObject CreateGameObject(string name, Transform parent = null)
    {
        var gameObject = new GameObject(name, parent);
        
        FrogeGame.Game.Scene.AddGameObject(gameObject, parent);

        return gameObject;
    }
    
    public T GetComponent<T>() where T: Component
    {
        return _components.OfType<T>().FirstOrDefault();
    }

    public T AddComponent<T>() where T: Component, new()
    {
        if (typeof(T) == typeof(Transform))
        {
            throw new Exception("Tried to add a transform, this is not allowed!");
        }
        
        var newComponent = new T();
        _components.Add(newComponent);
        newComponent.Init(this);
        return newComponent;
    }

    public void DestroyComponent<T>() where T : Component
    {
        Component component = GetComponent<T>();

        DestroyComponent(component);
    }

    public void DestroyComponent(Component component)
    {
        if (component == null) return;

        if (component as Transform != null)
        {
            throw new Exception("Tried to destroy a transform, this is not allowed!");
        }
        
        component.OnDestroy();
        _components.Remove(component);
    }

    public void Start()
    {
        foreach (var component in _components.ToArray())
        {
            component.Start();
        }
    }

    public void Update(GameTime gameTime)
    {
        foreach (var component in _components.ToArray())
        {
            component.Update(gameTime);
        }
    }
    
    public void Draw(GameTime gameTime)
    {
        foreach (var component in _components.ToArray())
        {
            component.Draw(gameTime);
        }
    }
    
    public void OnCollisionEnter(GameObject coll) { }
    public void OnCollision(GameObject coll) { }
    public void OnCollisionExit(GameObject coll) { }
}