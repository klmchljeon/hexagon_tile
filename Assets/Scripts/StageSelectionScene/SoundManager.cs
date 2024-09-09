using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }

    public void PlaySound(AudioClip clip, AudioSource source, bool isSFX = true)
    {
        source.clip = clip;
        source.volume = isSFX ? sfxVolume * masterVolume : musicVolume * masterVolume;
        source.Play();
    }
}
