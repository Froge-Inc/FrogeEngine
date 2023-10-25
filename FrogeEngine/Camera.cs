using System.Collections.Generic;
using FrogeEngine.Components;
using FrogeEngine.Components.Renderer;
using FrogeEngine.Mathematics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrogeEngine;

/// <summary>
/// The Camera component is a crucial component. Every scene has at least one camera; It is the component responsible\
/// for making the objects in the game space (which is an arbitrary plane) onto your screen.
/// The camera provides various properties of itself, and a variety of methods to allow proper rendering.
/// </summary>
public class Camera : Component
{
    ///<value>Camera.Main returns the currently active camera.</value>
    public static Camera Main;
    
    ///<value>Camera.PixelSize returns the current size of the camera view in screen pixels.</value>
    public Vector2 PixelSize { get; private set; }
    
    ///<value>Camera.Origin returns the bottom left game space position of the camera.</value>
    public Vector2 Origin { get; private set; }
    ///<value>Camera.TopLeft returns the top left game space position of the camera.</value>
    public Vector2 TopLeft => Origin + FOV with { X = 0 };
    
    ///<value>Camera.FOV returns the field of visible objects in the game space.</value>
    public Vector2 FOV { get; private set; }

    ///<value>Camera.Center returns the current center of the camera in the game space.</value>
    public Vector2 Center => Origin + 0.5f * FOV;
    
    ///<value>Camera._subscribedRenderers stores the list of Renderer components that should be drawn to this camera. Inaccessible.</value>
    private static List<Renderer> _subscribedRenderers = new();

    ///<value>Camera._gameSpriteBatch stores a SpriteBatch (MonoGame) of all gameObjects that will be drawn this frame. Inaccessible.</value>
    private SpriteBatch _gameSpriteBatch;

    /// <summary>
    /// Subscribes the given Renderer to the camera, allowing it to be drawn.
    /// </summary>
    /// <param name="renderer">The Renderer component to subscribe</param>
    public static void SubscribeRenderer(Renderer renderer) { _subscribedRenderers.Add(renderer); }
    
    /// <summary>
    /// Unsubscribes the given Renderer to the camera, disallowing it to be drawn.
    /// </summary>
    /// <param name="renderer">The Renderer component to unsubscribe</param>
    public static void UnSubscribeRenderer(Renderer renderer) { _subscribedRenderers.Remove(renderer); }

    /// <summary>
    /// Calls the Initialize phase of the camera. Should never be run manually.
    /// </summary>
    public override void Initialize()
    {
        _gameSpriteBatch = new SpriteBatch(FrogeGame.Game.GraphicsDevice);
    }

    /// <summary>
    /// Calls the draw phase of the camera. Should never be run manually.
    /// </summary>
    /// <param name="gameTime"></param>
    public override void Draw(GameTime gameTime)
    {
        DrawGame(gameTime);
    }
    
    /// <summary>
    /// Calls all draw functions of the subscribed renderers, drawing them to the screen. Inaccessible.
    /// </summary>
    /// <param name="gameTime">A parameter provided by MonoGame</param>
    private void DrawGame(GameTime gameTime)
    {
        _gameSpriteBatch.Begin();
        foreach (var renderer in _subscribedRenderers)
        {
            renderer.Draw(_gameSpriteBatch, this);
        }
        _gameSpriteBatch.End();
    }

    /// <summary>
    /// Updates the Camera.PixelSize and FOV to match the current backbuffer size.
    /// </summary>
    /// <param name="g"></param>
    public void UpdatePixelSize(GraphicsDeviceManager g)
    {
        PixelSize = new(g.PreferredBackBufferWidth, g.PreferredBackBufferHeight);
        UpdateFOV(Vector2.Zero);
    }

    /// <summary>
    /// Updates the Camera.FOV to the given size. Automatically matches the PixelSize with 100ppu if zero is given,
    /// automatically matches the X/Y component to the other if either is zero.
    /// </summary>
    /// <param name="newFOV"></param>
    public void UpdateFOV(Vector2 newFOV)
    {
        if (newFOV == Vector2.Zero)
        {
            FOV = PixelSize / 100;
            return;
        }
        if (newFOV.X == 0)
        {
            FOV = newFOV with { X = newFOV.Y / PixelSize.Y * PixelSize.X };
            return;
        }
        if (newFOV.Y == 0)
        {
            FOV = newFOV with { Y = newFOV.X / PixelSize.X * PixelSize.Y };
            return;
        }

        FOV = newFOV with { Y = PixelSize.X / newFOV.X * PixelSize.Y };
    }
    
    ///<value>Camera.PixelsPerUnit (ppu) returns the current amount of pixels on screen a unit of 1 has in the game space.</value>
    public float PixelsPerUnit => PixelSize.X / FOV.X; // Returns a value of pixels per unit

    ///<value>Camera.Bounds returns the current values of the sides of the camera in the game space.</value>
    public float[] Bounds => new[] {Origin.X, Origin.Y + FOV.Y, Origin.X + FOV.X, Origin.Y};
    
    ///<value>Camera.InBounds returns if the given point is in the camera view in the game space.</value>
    public bool InBounds(Vector2 p) { return InBoundsX(p) && InBoundsY(p); }
    ///<value>Camera.InBoundsX returns if the given point is in the horizontal bounds of the camera view in the game space.</value>
    public bool InBoundsX(Vector2 p) { return p.X >= Bounds[0] && p.X <= Bounds[2]; }
    ///<value>Camera.InBoundsY returns if the given point is in the vertical bounds of the camera view in the game space.</value>
    public bool InBoundsY(Vector2 p) { return p.Y >= Bounds[3] && p.X <= Bounds[1]; }
    
    ///<value>Camera.Lines returns the FrogeEngine.Mathematics.Line representation of the bounds of the camera.</value>
    public Line[] Lines => new[] { Left, Top, Right, Bottom };
    ///<value>Camera.Left returns the FrogeEngine.Mathematics.Line representation of the left side of the camera.</value>
    public Line Left => new (Origin, FOV with { X = 0 });
    ///<value>Camera.Top returns the FrogeEngine.Mathematics.Line representation of the top side of the camera.</value>
    public Line Top => new (Origin + FOV with { X = 0 }, FOV with { Y = 0 } );
    ///<value>Camera.Right returns the FrogeEngine.Mathematics.Line representation of the right side of the camera.</value>
    public Line Right => new (Origin + FOV, -FOV with { X = 0 });
    ///<value>Camera.Bottom returns the FrogeEngine.Mathematics.Line representation of the bottom side of the camera.</value>
    public Line Bottom => new (Origin + FOV with { Y = 0 }, -FOV with { Y = 0 });

    ///<value>Calculates if the given gameObject should be rendered. (Culling, to be added - currently incomplete.)</value>
    public bool IsRendered(GameObject gameObject)
    {
        return IsRendered(gameObject.Transform);
    }
    
    ///<value>Calculates if the given Transform should be rendered. (Culling, to be added - currently incomplete.)</value>
    public bool IsRendered(Transform transform)
    {
        // Two rectangles, 1 and 2, do not intersect if either A or B is true;
        // A: All points of rectangle 1 are below the bottom or above the top of rectangle 2
        // B: All points of rectangle 1 are to the left of the left or to the right of the right side of rectangle 2
        Vector2[] points = transform.Points;
        bool A = false;
        bool B = false;
        foreach (Vector2 p in points)
        {
            if (InBoundsY(p)) { A = true; }
            if (InBoundsX(p)) { B = true; }
        }
        return A && B;
    }
    
    public Vector2 Screen2WorldPos(Vector2 screenPos)
    {
        var pos = screenPos / PixelsPerUnit;
        pos -= TopLeft;
        pos.Y *= -1;
        return pos;
    }
    
    public Vector2 World2ScreenPos(Vector2 worldPos)
    {
        var pos = worldPos;
        pos.Y *= -1;
        pos += TopLeft;
        pos *= PixelsPerUnit;
        return pos;
    }
}