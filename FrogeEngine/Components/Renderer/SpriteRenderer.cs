using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrogeEngine.Components;

/// <summary>
/// The SpriteRenderer component is an extended Renderer component that is used to draw sprites (images) to the screen.
/// </summary>
public class SpriteRenderer : Renderer
{
    ///<value>SpriteRenderer.Sprite returns the image to be drawn to the screen.</value>
    public Texture2D Sprite { get; set; }
    ///<value>SpriteRenderer.SpriteSize returns the pixel size of the Sprite.</value>
    public Vector2 SpriteSize => new(Sprite.Width, Sprite.Height);

    /// <summary>
    /// Calls the start phase of the component. Should never be run manually.
    /// </summary>
    public override void Start()
    {
        Camera.SubscribeRenderer(this);
    }

    /// <summary>
    /// Calls the draw phase of the component, drawing the sprite to the screen. Should never be run manually.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to add the sprite to</param>
    /// <param name="camera">The camera to draw the sprite to</param>
    public override void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        //if (!camera.IsRendered(Transform)) return;
        var rect = TransformToRect(camera.TopLeft, camera.PixelsPerUnit, SpriteSize);
        spriteBatch.Draw(Sprite,
            rect.Item1,
            null,
            ColorModulate,
            Transform.Rotation,
            SpriteSize / 2,
            rect.Item2,
            SpriteEffects.None,
            Layer);
    }
    
    /// <summary>
    /// Calls the destroy phase of the gameObject. Should never be run manually.
    /// </summary>
    public override void OnDestroy()
    {
        Camera.UnSubscribeRenderer(this);
    }
}