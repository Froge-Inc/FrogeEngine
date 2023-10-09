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
    public Vector2 TopLeft => Origin + FOV with { X = 0 };
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
            // foreach gameobject
            //Renderer re = g.GetComponent<Renderer>();
            //Tuple<Rectangle, float> r = GameObjectToRect(g);
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
    

}