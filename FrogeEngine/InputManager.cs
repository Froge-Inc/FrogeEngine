using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FrogeEngine;

/// <summary>
/// InputManager handles input for games. A keymapping functionality has been added, to allow for PlayerN inputs.
/// </summary>
public static class InputManager
{
    /// <summary>
    /// Global keymapping buttons. Will add a JSON parser for this later.
    /// </summary>
    public enum KeyMap
    {
        Up, Down, Left, Right, Shoot, RotateLeft, RotateRight
    }

    /// <summary>
    /// Keymapping for player 1 default keys. Will add a JSON parser for this later.
    /// </summary>
    private static Dictionary<KeyMap, Keys> Player1Keys = new()
    {
        { KeyMap.Up, Keys.W },
        { KeyMap.Down, Keys.S },
        { KeyMap.Left, Keys.A },
        { KeyMap.Right, Keys.D },
        { KeyMap.Shoot, Keys.Space },
        { KeyMap.RotateLeft, Keys.Q },
        { KeyMap.RotateRight, Keys.E }
    };

    /// <summary>
    /// Keymapping for player 2 default keys. Will add a JSON parser for this later.
    /// </summary>
    private static Dictionary<KeyMap, Keys> Player2Keys = new()
    {
        { KeyMap.Up, Keys.Up},
        { KeyMap.Down, Keys.Down},
        { KeyMap.Left, Keys.Left},
        { KeyMap.Right, Keys.Right},
        { KeyMap.Shoot, Keys.Enter},
        { KeyMap.RotateLeft, Keys.OemComma},
        { KeyMap.RotateRight, Keys.OemPeriod}
    };

    /// <summary>
    /// Turns a player integer and a KeyMap value into a usable Keys value.
    /// </summary>
    /// <param name="key">Keymap pressed</param>
    /// <param name="player">player number</param>
    /// <returns></returns>
    private static Keys GetPlayerKey(KeyMap key, int player)
    {
        return (player == 2 ? Player2Keys : Player1Keys)[key];
    }
    
    ///<value>Stores the previous and current state of the mouse, inaccessible.</value>
    private static MouseState _prevMS, MS;
    
    ///<value>Stores the previous and current state of the keyboard, inaccessible.</value>
    private static KeyboardState _prevKS, KS;

    /// <summary>
    /// Updates the input manager (rereads input). Should never be called manually; FrogeGame handles inputManager updating.
    /// </summary>
    public static void Update()
    {
        _prevMS = MS;
        MS = Mouse.GetState();
        
        _prevKS = KS;
        KS = Keyboard.GetState();
    }
    
    
    ///<value>Returns the current mouse position.</value>
    public static Vector2 MousePosition => new Vector2(MS.X, MS.Y);

    ///<value>Returns if the left mouse button is clicked this frame.</value>
    public static bool LeftPressed =>
        _prevMS.LeftButton == ButtonState.Released && MS.LeftButton == ButtonState.Pressed;
    
    ///<value>Returns if the left mouse button is currently held down.</value>
    public static bool LeftDown => MS.LeftButton == ButtonState.Pressed;
    
    ///<value>Returns if the left mouse button is released this frame.</value>
    public static bool LeftReleased =>
        _prevMS.LeftButton == ButtonState.Pressed && MS.LeftButton == ButtonState.Released;
    
    
    ///<value>Returns if the right mouse button is clicked this frame.</value>
    public static bool RightPressed =>
        _prevMS.RightButton == ButtonState.Released && MS.RightButton == ButtonState.Pressed;
    
    ///<value>Returns if the right mouse button is currently held down.</value>
    public static bool RightDown => MS.RightButton == ButtonState.Pressed;
    
    ///<value>Returns if the right mouse button is released this frame.</value>
    public static bool RightReleased =>
        _prevMS.RightButton == ButtonState.Pressed && MS.RightButton == ButtonState.Released;
    
    
    ///<value>Returns if the given player key is pressed this frame.</value>
    public static bool PlayerKeyPressed(KeyMap key, int player) { return KS.IsKeyDown(GetPlayerKey(key, player)) && _prevKS.IsKeyUp(GetPlayerKey(key, player)); }
    
    ///<value>Returns if the given player key is currently held down.</value>
    public static bool PlayerKeyDown(KeyMap key, int player) { return KS.IsKeyDown(GetPlayerKey(key, player)); }
    
    ///<value>Returns if the given player key is released this frame.</value>
    public static bool PlayerKeyReleased(KeyMap key, int player) { return _prevKS.IsKeyDown(GetPlayerKey(key, player)) && KS.IsKeyUp(GetPlayerKey(key, player)); }
    
    
    ///<value>Returns if the given keyboard key is pressed this frame.</value>
    public static bool GlobalKeyPressed(Keys key) { return KS.IsKeyDown(key) && _prevKS.IsKeyUp(key); }
    
    ///<value>Returns if the given keyboard key is pressed this frame.</value>
    public static bool GlobalKeyDown(Keys key) { return KS.IsKeyDown(key); }
    
    ///<value>Returns if the given keyboard key is pressed this frame.</value>
    public static bool GlobalKeyReleased(Keys key) { return KS.IsKeyUp(key) && _prevKS.IsKeyDown(key); }
}