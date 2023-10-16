using FrogeEngine;
using FrogeEngine.Components;
using Microsoft.Xna.Framework.Graphics;

public class TestGame : FrogeGame
{
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
        
        var cameraObject = new GameObject("camera");
        var camera = cameraObject.AddComponent<Camera>();
        scene.AddGameObject(cameraObject);
        
        var sceneIndex = AddGameScene(scene);
        LoadScene(sceneIndex);
    }
}