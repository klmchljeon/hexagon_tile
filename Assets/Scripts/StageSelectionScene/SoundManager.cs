using System.IO;
using UnityEngine;

[System.Serializable]
public class AudioSettings
{
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    AudioSource bgm;

    private string filePath;

    public static SoundManager _instance;

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType(typeof(SoundManager)) as SoundManager;

                if (_instance == null)
                {
                    if (applicationIsQuitting)
                    {
                        return null;
                    }

                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<SoundManager>();
                    singletonObject.name = typeof(SoundManager).ToString();
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return _instance;
        }
    }

    private static bool applicationIsQuitting = false;
    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }

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

        filePath = Path.Combine(Application.persistentDataPath, "audioSettings.json");
        LoadData();
    }

    void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            AudioSettings settings = JsonUtility.FromJson<AudioSettings>(json);

            masterVolume = settings.masterVolume;
            musicVolume = settings.musicVolume;
            sfxVolume = settings.sfxVolume;
        }
        else
        {
            masterVolume = 1f;
            musicVolume = 1f;
            sfxVolume = 1f;
        }
    }

    void SaveData()
    {
        AudioSettings settings = new AudioSettings
        {
            masterVolume = masterVolume,
            musicVolume = musicVolume,
            sfxVolume = sfxVolume
        };

        string json = JsonUtility.ToJson(settings);
        File.WriteAllText(filePath, json);
    }

    private void Start()
    {
        bgm = GetComponent<AudioSource>();
    }

    private void Update()
    {
        bgm.volume = musicVolume * masterVolume;
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        SaveData();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        SaveData();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        SaveData();
    }

    public void PlaySound(AudioClip clip, AudioSource source, bool isSFX = true)
    {
        source.clip = clip;
        source.volume = isSFX ? sfxVolume * masterVolume : musicVolume * masterVolume;
        source.Play();
    }
}
