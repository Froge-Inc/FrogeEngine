using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace FrogeEngine;

public class GameScene
{
    private List<GameObject> _gameObjects = new();
    private List<Transform> _rootTransforms = new();

    public void Start()
    {
        foreach (var gameObject in _gameObjects.ToArray())
        {
            gameObject.Start();
        }
    }

    public void Update(GameTime gameTime)
    {
        foreach (var gameObject in _gameObjects.ToArray())
        {
            gameObject.Update(gameTime);
        }
    }

    public void Draw(GameTime gameTime)
    {
        foreach (var gameObject in _gameObjects.ToArray())
        {
            gameObject.Draw(gameTime);
        }
    }

    /// <summary>
    /// Add a GameObject to the scene. If parent is null also add it to the root transforms.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="parent"></param>
    public void AddGameObject( GameObject gameObject, Transform parent = null)
    {
        _gameObjects.Add( gameObject);
        if (parent != null) return;
        _rootTransforms.Add(gameObject.Transform);
    }
    
    /// <summary>
    /// Recursively destroys GameObject and it's children.
    /// </summary>
    /// <param name="gameObject"></param>
    public void DestroyGameObject(GameObject gameObject)
    {
        _gameObjects.Remove(gameObject);
        gameObject.Transform.Parent.UpdateChildren();
        var children = gameObject.Transform.GetChildren();
        foreach (var child in children)
        {
            DestroyGameObject(child.GameObject);
        }
    }
    
    /// <summary>
    /// In case a transform's parent is changed, make sure that the root transforms are updated.
    /// </summary>
    /// <param name="transform"></param>
    public void UpdateParent(Transform transform)
    {
        if (transform.Parent != null)
        {
            if (!_rootTransforms.Contains(transform)) return;

            _rootTransforms.Remove(transform);
        }

        _rootTransforms.Add(transform);
    }
}