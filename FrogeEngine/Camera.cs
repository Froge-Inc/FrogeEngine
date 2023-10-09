using System;
using System.Collections.Generic;
using FrogeEngine.Mathematics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = System.Numerics.Vector2;

namespace FrogeEngine;

public class Camera
{
    public Vector2 PixelSize { get; private set; }
    
    public Vector2 Origin { get; private set; }
    public Vector2 FOV { get; private set; }
    
    private List<GameObject> _renderedObjects;
    //private List<UIObject> _uiObjects;

    private SpriteBatch _spriteBatch;

    public Camera(SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;
        _renderedObjects = new();
    }

    public void RenderObject(GameObject go) { _renderedObjects.Add(go); }
    public void UnRenderObject(GameObject go) { _renderedObjects.Remove(go); }
    
    

    public virtual void Update(GameTime gameTime) { }

    public virtual void Draw(GameTime gameTime)
    {
        DrawGame(gameTime);
        DrawUI(gameTime);
    }
    private void DrawGame(GameTime gameTime)
    {
        foreach (GameObject g in _renderedObjects)
        {
            //Tuple<Rectangle, float> r = GameObjectToRect(g);
            //Renderer re = g.GetComponent<Renderer>();
            //_spriteBatch.Draw(re.Sprite, r.Rectangle, r.float, re.color);
        }
    }
    private void DrawUI(GameTime gameTime)
    {
        //todo: Convert floatrange coords to rectangle
        /* foreach(UIObject u in _uiObjects)
         * {
         *      _spriteBatch.Draw(u.Sprite, calculatedRectangle, u.Rotation, u.Color)
         * }
         */
    }
    
    
    
    public float PixelsPerUnit => PixelSize.X / FOV.X; // Returns a value of pixels per unit

    public float[] Bounds => new[] {Origin.X, Origin.Y + FOV.Y, Origin.X + FOV.X, Origin.Y};
    public bool InBounds(Vector2 p) { return InBoundsX(p) && InBoundsY(p); }
    public bool InBoundsX(Vector2 p) { return p.X >= Bounds[0] && p.X <= Bounds[2]; }
    public bool InBoundsY(Vector2 p) { return p.Y >= Bounds[3] && p.X <= Bounds[1]; }


    public Line[] Lines => new[] { Left, Top, Right, Bottom };
    public Line Left => new (Origin, FOV with { X = 0 });
    public Line Top => new (Origin + FOV with { X = 0 }, FOV with { Y = 0 } );
    public Line Right => new (Origin + FOV, -FOV with { X = 0 });
    public Line Bottom => new (Origin + FOV with { Y = 0 }, -FOV with { Y = 0 });

    public bool IsRendered(GameObject gameObject)
    {
        return IsRendered(gameObject.Transform);
    }
    public bool IsRendered(Transform transform)
    {
        foreach (Vector2 p in transform.Points) // If any corner points of the object fall inbounds, it should be rendered
        {
            if(InBounds(p)) { return true; }
        }

        foreach (Line l in transform.Sides) // Else if any bounds of the object intersect with the camera bounds, it should be rendered
        {
            foreach (Line s in Lines)
            {
                if(l.Intersects(s)) { return true; }
            }
        }
        return false; // Else it shouldn't
    }
    

}