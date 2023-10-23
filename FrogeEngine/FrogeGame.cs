using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FrogeEngine;

/// <summary>
/// The FrogeGame class extends the MonoGame Game class, and handles the initialization of the engine.
/// To use FrogeEngine, this class should be extended by an ExampleGame class.
/// </summary>
public class FrogeGame : Game
{
    /// <value> FrogeGame.Game returns a static instance of the current game, to allow all objects root access. </value>
    public static FrogeGame Game => _game;
    
    /// <value> FrogeGame.Game.Scene returns an instance of the current scene in the given game, to allow all objects scene access. </value>
    public GameScene Scene => _scene;
    
    /// <value> FrogeGame._game is a private instance of the current game, inaccessible. </value>
    private static FrogeGame _game;
    
    /// <value> FrogeGame._scene is a private instance of the current scene, inaccessible. </value>
    private GameScene _scene;
    
    /// <value> FrogeGame.Game._gameScenes manages the current existing scenes, to allow for scene switching. </value>
    private List<GameScene> _gameScenes;

    /// <value> FrogeGame.Game._graphics holds an instance of MonoGame's GraphicsDeviceManager. </value>
    protected GraphicsDeviceManager _graphics;
    
    
    public static Dictionary<string, int> SceneIndices = new Dictionary<string, int>();
    
    protected FrogeGame()
    {
        _game = this;
        _gameScenes = new List<GameScene>();
        _graphics = new GraphicsDeviceManager(this);
        _graphics.ApplyChanges();
    }

    /// <summary>
    /// Creates a new scene in the current game.
    /// </summary>
    /// <param name="scene">Name of the new scene</param>
    /// <returns>The index of the newly created scene</returns>
    public int AddGameScene(GameScene scene)
    {
        _gameScenes.Add(scene);
        return _gameScenes.Count - 1;
    }

    /// <summary>
    /// Calls an existing scene in the current game.
    /// </summary>
    /// <param name="sceneIndex">Index of the requested scene</param>
    /// <returns>The requested GameScene.</returns>
    public GameScene GetScene(int sceneIndex)
    {
        return _gameScenes[sceneIndex];
    }

    /// <summary>
    /// Loads a scene to be the current scene, disabling and storing earlier scenes.
    /// </summary>
    /// <param name="index">Index of the scene to be loaded</param>
    public void LoadScene(int index)
    {
        UnloadScene();
        var scene = GetScene(index).RecursiveDeepCopy();
        if (scene == null) return;
        _scene = scene;
        StartScene();
    }
    
    public void LoadScene(string name)
    {
        LoadScene(SceneIndices[name]);
    }

    public void UnloadScene()
    {
        _scene?.Unload();
    }
    
    /// <summary>
    /// Calls the LoadContent phase of game initialization. Should be overridden by ExampleGame class.
    /// </summary>
    protected override void LoadContent()
    {
        
    }
    
    /// <summary>
    /// Calls the Start phase of scene initialization of the current scene.
    /// </summary>
    private void StartScene()
    {
        _scene?.Start();
    }

    /// <summary>
    /// Calls the update phase of the InputManager and the current scene; cannot be overridden, the update phase of an ExampleGame should happen in the ExampleScene.
    /// </summary>
    /// <param name="gameTime">Parameter provided by MonoGame.</param>
    protected sealed override void Update(GameTime gameTime)
    {
        InputManager.Update();
        _scene?.Update(gameTime);
    }

    /// <summary>
    /// Calls the Draw phase of the current scene; cannot be overridden, the draw phase of an ExampleGame should happen in the ExampleScene.
    /// </summary>
    /// <param name="gameTime"></param>
    protected sealed override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _scene?.Draw(gameTime);
    }
}
