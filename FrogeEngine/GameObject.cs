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
    
    public T GetComponent<T>() where T: Component
    {
        return _components.OfType<T>().FirstOrDefault();
    }

    public T AddComponent<T>() where T: Component, new()
    {
        var newComponent = new T();
        _components.Add(newComponent);
        newComponent.Init(this);
        return newComponent;
    }
    
    
    
    public virtual void OnCollisionEnter(GameObject coll) { }
    public virtual void OnCollision(GameObject coll) { }
    public virtual void OnCollisionExit(GameObject coll) { }
}