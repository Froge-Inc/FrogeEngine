using System.Collections.Generic;
using System.Linq;

namespace FrogeEngine;

public class GameObject
{
    public string Name { get => _name; }
    public Transform Transform => _components[0] as Transform; //Components[0] is reserved for Transform and will ALWAYS have a reference in it, hence no checks.

    private string _name;
    private List<Component> _components;
    

    public GameObject(string name)
    {
        _name = name;
        _components = new List<Component>()
        {
            new Transform(null)
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
        return newComponent;
    }
}