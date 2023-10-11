using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = System.Numerics.Vector2;

namespace FrogeEngine.Components;

public class SpriteRenderer : Component
{
    public Texture2D Sprite { get; set; }
    public Color ColorModulate { get; set; } = Color.White;
    public int Layer { get; set; } = 0;
    
    
    public void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        if (!camera.IsRendered(Transform)) return;
        var rect = GameObjectToRect(camera.TopLeft, camera.PixelsPerUnit);
        spriteBatch.Draw(Sprite,
            rect.Item1,
            null,
            ColorModulate,
            Transform.Rotation,
            rect.Item2 / 2,
            rect.Item2,
            SpriteEffects.None,
            Layer);
    }
    
    public (Vector2, Vector2) GameObjectToRect(Vector2 topLeft, float ppu)
    {
        Vector2 pTopLeft = Transform.Position - Transform.LocalTopLeft;
        Vector2 pos = (pTopLeft - topLeft) * ppu;
        Vector2 scl = Transform.Scaling * ppu;
        return (pos, scl);
    }
}