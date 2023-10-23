using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrogeEngine.Components;

/// <summary>
/// The Renderer component is the foundation of rendering the game; translating the arbitrary positions of the Transform
/// component to the screen. The renderer component gets extended by types of renderers like a SpriteRenderer and TextRenderer;
/// it should not be used individually.
/// </summary>
public abstract class Renderer : Component
{
    ///<value>Renderer.ColorModulate returns the color modulate of the renderer.</value>
    public Color ColorModulate { get; set; } = Color.White;
    ///<value>Renderer.Layer returns the draw layer of the renderer.</value>
    public int Layer { get; set; } = 0;

    /// <summary>
    /// Calls the start phase of the renderer. Should never be run manually.
    /// </summary>
    public override void Start()
    {
        Camera.SubscribeRenderer(this);
    }

    /// <summary>
    /// Calls the drawing phase of the renderer. Extended by Renderer components to draw the object.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to add the renderer to</param>
    /// <param name="camera">The camera to draw to</param>
    public abstract void Draw(SpriteBatch spriteBatch, Camera camera);

    /// <summary>
    /// Calculates the pixel rectangle (position and scaling) on the camera of a transform.
    /// </summary>
    /// <param name="topLeft">The top left of the camera to draw to in the game space.</param>
    /// <param name="ppu">The ppu to use.</param>
    /// <param name="size">The pixel size of the object to render.</param>
    /// <returns></returns>
    protected (Vector2, Vector2) TransformToRect(Vector2 topLeft, float ppu, Vector2 size)
    {
        var pos = (Transform.Position - topLeft) * ppu;
        pos = pos with { Y = -pos.Y };
        var scl = Transform.Scaling * ppu / size;
        return (pos, scl);
    }

    /// <summary>
    /// Calls the destroy phase of the component. Should never be run manually.
    /// </summary>
    public override void OnDestroy()
    {
        Camera.UnSubscribeRenderer(this);
    }

}