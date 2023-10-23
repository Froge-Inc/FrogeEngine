using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FrogeEngine;

/// <summary>
/// The GameScene class contains the logic for a game to be run. It contains a list of GameObjects, and manages them as long as the scene is active.
/// The GameScene class should never be overridden or extended; the game should be managed using GameObjects.
/// </summary>
public class GameScene
{
    ///<value>GameScene._gameObjects holds a list of all gameObjects in the scene, inaccessible.</value>
    private List<GameObject> _gameObjects = new();
    ///<value>GameScene._gameObjects holds a list of all root transforms in the scene, inaccessible.</value>
    private List<Transform> _rootTransforms = new();
    
    
    public GameScene()
    {
    }
    
    public GameScene RecursiveDeepCopy()
    {
        var newScene = new GameScene();
        
        foreach (var gameObject in _gameObjects)
        {
            var newGameObject = gameObject.DeepCopy();
            newScene._gameObjects.Add( newGameObject);
            newScene._rootTransforms.Add(newGameObject.Transform);
        }
        
        return newScene;
    }
    
    /// <summary>
    /// Calls the start phase of all gameObjects. While the method is public, this method should never be run manually.
    /// FrogeGame handles scene initialization.
    /// </summary>
    public void Start()
    {
        foreach (var gameObject in _gameObjects.ToArray())
        {
            gameObject.Start();
        }
    }

    public void Unload()
    {
        foreach (var rootTransforms in _rootTransforms)
        {
            var gameObject = rootTransforms.GameObject;
            gameObject.Destroy();
        }
    }

    /// <summary>
    /// Calls the update phase of all gameObjects. While the method is public, this method should never be run manually.
    /// FrogeGame handles scene updating.
    /// </summary>
    public void Update(GameTime gameTime)
    {
        foreach (var gameObject in _gameObjects.ToArray())
        {
            gameObject.Update(gameTime);
        }
    }

    /// <summary>
    /// Calls the start phase of all gameObjects. While the method is public, this method should never be run manually.
    /// FrogeGame handles scene drawing.
    /// </summary>
    public void Draw(GameTime gameTime)
    {
        foreach (var gameObject in _gameObjects.ToArray())
        {
            gameObject.Draw(gameTime);
        }
    }

    /// <summary>
    /// Adds a GameObject to the scene. If parent is null also add it to the root transforms.
    /// </summary>
    /// <param name="gameObject">GameObject to add</param>
    /// <param name="parent">Optional parent transform</param>
    public void AddGameObject( GameObject gameObject, Transform parent = null)
    {
        _gameObjects.Add( gameObject);
        if (parent != null) return;
        _rootTransforms.Add(gameObject.Transform);
    }
    
    /// <summary>
    /// Recursively destroys a GameObject and it's components and children.
    /// </summary>
    /// <param name="gameObject">The GameObject to destroy</param>
    public void DestroyGameObject(GameObject gameObject)
    {
        var children = gameObject.Transform.GetChildren();
        foreach (var child in children)
        {
            DestroyGameObject(child.GameObject);
        }
        gameObject.OnDestroy();
        _gameObjects.Remove(gameObject);
        gameObject.Transform.Parent?.UpdateChildren();
    }
    
    /// <summary>
    /// In case a transform's parent is changed, make sure that the root transforms are updated if necessary.
    /// </summary>
    /// <param name="transform">The transform who's parent transform changed.</param>
    public void UpdateParent(Transform transform)
    {
        if (transform.Parent != null)
        {
            if (!_rootTransforms.Contains(transform)) return;

            _rootTransforms.Remove(transform);
            return;
        }

        _rootTransforms.Add(transform);
    }
}