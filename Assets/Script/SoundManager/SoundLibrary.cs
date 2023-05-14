using System;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Sound Library", menuName = "Game/Sound/Sound Library")]
public class SoundLibrary : ScriptableObject
{
    [SerializeField] private Sound[] sounds;

    [SerializeField] private Mixer[] mixers;

    public Mixer[] GetMixers { get { return mixers; } }
    
    public Sound[] GetSounds
    {
        get
        {
            return sounds;
        }
    }
}

[Serializable]
public struct Sound
{
    public string name;
    public AudioClip clip;
    public SoundType soundType;

    public float SoundLength
    {
        get
        {
            if (clip is null)
            {
                Debug.LogError("No clip is attached to " + name);
                return 0;
            }

            return clip.length;
        }
    }
}

[Serializable]
public struct Mixer
{
    public string name;
    public AudioMixerGroup mixer;
    public SoundType mixerType;
}

public enum SoundType
{
    SOUND_EFFECT,
    USER_INTERFACE,
    MUSIC,
    AMBIENCE_FLAT,
    AMBIENCE_SPATIAL
}
