using System;
using System.Collections.Generic;
using System.Linq;
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
    
    
    
    //Maybe move to Renderer component?? Converts transform to coordinates on screen
    public Tuple<Rectangle, float> GameObjectToRect(Vector2 topLeft, float ppu)
    {
        int posX = (int)((-topLeft.X + Transform.Position.X) * ppu);
        int posY = (int)((topLeft.Y - Transform.Position.Y) * ppu);
        int sclX = (int)(Transform.Scaling.X * ppu);
        int sclY = (int)(Transform.Scaling.Y * ppu);
        Rectangle rect = new(posX, posY, sclX, sclY);
        float rot = Transform.Rotation;
        return new Tuple<Rectangle, float>(rect, rot);
    }
}