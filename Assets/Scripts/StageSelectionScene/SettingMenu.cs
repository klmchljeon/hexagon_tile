using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private void Start()
    {
        // 슬라이더 초기값을 현재 사운드 매니저의 값으로 설정
        masterVolumeSlider.value = SoundManager.instance.masterVolume;
        musicVolumeSlider.value = SoundManager.instance.musicVolume;
        sfxVolumeSlider.value = SoundManager.instance.sfxVolume;

        // 슬라이더 값 변경 이벤트 연결
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float value)
    {
        SoundManager.instance.SetMasterVolume(value);
    }

    public void SetMusicVolume(float value)
    {
        SoundManager.instance.SetMusicVolume(value);
    }

    public void SetSFXVolume(float value)
    {
        SoundManager.instance.SetSFXVolume(value);
    }
}
