using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrogeEngine.Components;

/// <summary>
/// The TextRenderer component is an extended Renderer component that is used to draw text to the screen.
/// </summary>
public class TextRenderer : Renderer
{
    ///<value>TextRenderer.Text returns the text to be rendered to the screen</value>
    public string Text = "";
    ///<value>TextRenderer.Text returns the font to draw the text with.</value>
    public SpriteFont SpriteFont { get; set; }
    ///<value>SpriteRenderer.TextSize returns the pixel size of the current stored Text.</value>
    public Vector2 TextSize => SpriteFont.MeasureString(Text);
    
    
    /// <summary>
    /// Calls the draw phase of the component, drawing the text to the screen. Should never be run manually.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to add the text to</param>
    /// <param name="camera">The camera to draw the text to</param>
    public override void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        var pos = Camera.Main.World2ScreenPos(Transform.Position);
        
        spriteBatch.DrawString(SpriteFont, Text, pos, ColorModulate, Transform.Rotation, TextSize/2 , Vector2.One, SpriteEffects.None, Layer);
    }
    
    
}