using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    Dictionary<string, Sound> SoundDictionary;
    Dictionary<SoundType, Mixer> MixerDictionary;

    [SerializeField] private SoundLibrary soundLibrary;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance is null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Initialise();
    }

    [ContextMenu("Reset")]
    private void Initialise()
    {
        SoundDictionary = new();

        MixerDictionary = new();

        foreach (Sound s in soundLibrary.GetSounds)
            SoundDictionary.Add(s.name, s);

        foreach (Mixer m in soundLibrary.GetMixers)
            MixerDictionary.Add(m.mixerType, m);

        print("Initialised Sound Manager");
    }

    public void PlaySound(string audioName, AudioSource source, bool loop = false, bool randomisePitch = false)
    {
        Sound sound = SoundDictionary[audioName];

        Mixer mixer = MixerDictionary[sound.soundType];

        if (source.outputAudioMixerGroup is null)
            source.outputAudioMixerGroup = MixerDictionary[sound.soundType].mixer;

        source.clip = sound.clip;
        source.loop = loop;
        source.pitch = (randomisePitch) ? Random.Range(1, 1.25F) : 1;

        switch (mixer.mixerType)
        {
            case SoundType.SOUND_EFFECT:
                source.spatialBlend = 1;
                break;

            case SoundType.AMBIENCE_FLAT:
            case SoundType.USER_INTERFACE:
            case SoundType.MUSIC:
                source.spatialBlend = 0;
                break;
        }

        source.Play();
    }

    public void PlaySound(string audioName, Vector3 position, bool randomisePitch = true)
    {
        Sound sound = SoundDictionary[audioName];

        Mixer mixer = MixerDictionary[sound.soundType];

        GameObject tmpGameObject = new("tmp_sound");
        tmpGameObject.transform.position = position;
        AudioSource source = tmpGameObject.AddComponent<AudioSource>();

        if (source.outputAudioMixerGroup is null)
            source.outputAudioMixerGroup = MixerDictionary[sound.soundType].mixer;

        source.clip = sound.clip;
        source.pitch = (randomisePitch) ? Random.Range(1, 1.25F) : 1;

        switch (mixer.mixerType)
        {
            case SoundType.AMBIENCE_SPATIAL:
            case SoundType.SOUND_EFFECT:
                source.spatialBlend = 1;
                break;
        }

        source.Play();

        Destroy(tmpGameObject, sound.SoundLength + 0.1F);
    }
}