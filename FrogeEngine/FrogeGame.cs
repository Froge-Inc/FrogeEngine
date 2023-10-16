

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrogeEngine;

public class FrogeGame : Game
{
    public static FrogeGame Game { get => _game; }
    public GameScene Scene { get => _scene; }
    
    private static FrogeGame _game;
    
    private List<GameScene> _gameScenes;

    private GraphicsDeviceManager _graphics;
    private GameScene _scene;

    protected FrogeGame()
    {
        _game = this;
        _gameScenes = new List<GameScene>();
        _graphics = new GraphicsDeviceManager(this);
        _graphics.ApplyChanges();
    }

    public int AddGameScene(GameScene scene)
    {
        _gameScenes.Add(scene);

        return _gameScenes.Count - 1;
    }

    public GameScene GetScene(int sceneIndex)
    {
        return _gameScenes[sceneIndex];
    }

    public void LoadScene(int index)
    {
        var scene = GetScene(index);
        if (scene == null) return;
        _scene = scene;
        StartScene();
    }

    protected override void LoadContent()
    {
        
    }
    
    private void StartScene()
    {
        _scene?.Start();
    }

    protected sealed override void Update(GameTime gameTime)
    {
        _scene?.Update(gameTime);
    }

    protected sealed override void Draw(GameTime gameTime)
    {
        _scene?.Draw(gameTime);
    }
}
