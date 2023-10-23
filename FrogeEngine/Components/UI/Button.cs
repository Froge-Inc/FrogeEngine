using Microsoft.Xna.Framework;

namespace FrogeEngine.Components;

public class Button : Component
{
    public delegate void OnClickDelegate();
    public event OnClickDelegate OnClick;
    
    public override void Update(GameTime gameTime)
    {
        if (InputManager.LeftPressed)
        {
            if (!IsMouseOver()) return;
            OnClick?.Invoke();
        }
    }

    private bool IsMouseOver()
    {
        var mousePos = InputManager.MousePosition;
        mousePos = Camera.Main.Screen2WorldPos(mousePos);
        if (mousePos.X < Transform.BottomLeft.X || mousePos.Y < Transform.BottomLeft.Y) return false;
        return !(mousePos.X > Transform.TopRight.X) && !(mousePos.Y > Transform.TopRight.Y);
    }
    
}