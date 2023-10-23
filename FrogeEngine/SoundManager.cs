using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace FrogeEngine;

/// <summary>
/// SoundManager handles input for games. A separation has been made between songs and sound effects.
/// This class will likely be improved and expanded on later.
/// </summary>
public static class SoundManager
{
    private static readonly float _songVolume = 0.3f;
    private static readonly float _sfxVolume = 0.5f;
    
    
    ///<value>Stores the song files</value>
    private static Dictionary<string, Song> _songs = new();
    ///<value>Stores the sound effect files</value>
    private static Dictionary<string, SoundEffect> _sfx = new();

    /// <summary>
    /// Adds a song to the song library.
    /// </summary>
    /// <param name="name">Name of the song</param>
    /// <param name="song">Song file</param>
    public static void AddSong(string name, Song song)
    {
        _songs.Add(name, song);
    }
    
    /// <summary>
    /// Plays a song from the song library, if available.
    /// </summary>
    /// <param name="name">Name of the song</param>
    /// <param name="loop">Loop or not</param>
    public static void PlaySong(string name, bool loop = false)
    {
        if (!_songs.ContainsKey(name)) return;
        MediaPlayer.Play(_songs[name]);
        MediaPlayer.IsRepeating = loop;
        MediaPlayer.Volume = _songVolume;
    }

    /// <summary>
    /// Add sound effect to the sound effect library.
    /// </summary>
    /// <param name="name">Name of the sound effect</param>
    /// <param name="sfx">Sound effect file</param>
    public static void AddSFX(string name, SoundEffect sfx)
    {
        _sfx.Add(name, sfx);
    }

    /// <summary>
    /// Play sound effect from the sound effect library, if available.
    /// </summary>
    /// <param name="name">Name of the sound effect</param>
    public static void PlaySFX(string name)
    {
        if (!_sfx.ContainsKey(name)) return;
        _sfx[name].Play();
        SoundEffect.MasterVolume = _sfxVolume;
    }
}