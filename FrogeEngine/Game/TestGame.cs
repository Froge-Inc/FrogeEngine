using System;
using FrogeEngine;
using FrogeEngine.Components;
using FrogeEngine.Components.Renderer;
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
        
        var cameraObject = new GameObject("camera");
        var camera = cameraObject.AddComponent<Camera>();
        scene.AddGameObject(cameraObject);
        _graphics.ToggleFullScreen();
        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        _graphics.ApplyChanges();
        camera.UpdatePixelSize(_graphics);

        var testObject = new GameObject("testObject");
        var sr1 = testObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr1.Sprite = Content.Load<Texture2D>("bal");
        scene.AddGameObject(testObject);
        testObject.Transform.Position = camera.Center;
        testObject.Transform.Rotation = 0.5f;
        testObject.Start();

        var testChildObject = new GameObject("testChildObject", testObject.GetComponent<Transform>());
        var sr2 = testChildObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr2.Sprite = Content.Load<Texture2D>("bal");
        sr2.ColorModulate = Color.Red;
        scene.AddGameObject(testChildObject);
        testChildObject.Transform.LocalPosition = new Vector2(1f, 1f);
        testChildObject.Transform.LocalRotation = 0.5f;
        
        
        
        
        
        var sceneIndex = AddGameScene(scene);
        LoadScene(sceneIndex);
    }
}