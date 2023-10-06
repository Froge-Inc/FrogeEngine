using System.Collections.Generic;

namespace FrogeEngine;

public class GameObject
{
    public Transform Transform => _components[0] as Transform; //Components[0] is reserved for Transform and will ALWAYS have a reference in it, hence no checks.
    
    
    private List<Component> _components;
    
    
}