using System;
using FrogeEngine;
using FrogeEngine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = System.Numerics.Vector2;

public class TestGame : FrogeGame
{
    
    [STAThread]
    static void Main()
    {
        var game = new TestGame();
        game.Run();
    }
    
    protected override void LoadContent()
    {
        Content.RootDirectory = "Content";
        var scene = new GameScene();

        var testObject = new GameObject("testObject");
        var sr = testObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.Sprite = Content.Load<Texture2D>("bal");
        scene.AddGameObject(testObject);
        testObject.Transform.Position = new Vector2(9.5f, 5);
        
        
        var cameraObject = new GameObject("camera");
        var camera = cameraObject.AddComponent<Camera>();
        scene.AddGameObject(cameraObject);
        _graphics.ToggleFullScreen();
        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        _graphics.ApplyChanges();
        camera.UpdatePixelSize(_graphics);
        
        
        var sceneIndex = AddGameScene(scene);
        LoadScene(sceneIndex);
    }
}