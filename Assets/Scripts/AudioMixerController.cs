using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    public AudioMixer mainMixer;
    public SettingsMenu settingsMenu;


    public void ReadPlayerPrefs()
    {
        if (PlayerPrefs.GetInt("settings_audio") == 0)
        {
            SavePlayerPrefs();
        }
        else
        {
            float playerPrefsMasterVolume = PlayerPrefs.GetFloat("settings_masterVolume");
            SetVolumeMaster(playerPrefsMasterVolume);
            settingsMenu. masterSlider.value = playerPrefsMasterVolume;
            float playerPrefsMusicVolume = PlayerPrefs.GetFloat("settings_musicVolume");
            SetVolumeMusic(playerPrefsMusicVolume);
            settingsMenu.musicSlider.value = playerPrefsMusicVolume;
            float playerPrefsSFXVolume = PlayerPrefs.GetFloat("settings_sfxVolume");
            SetVolumeSfx(playerPrefsSFXVolume);
            settingsMenu.sfxSlider.value = playerPrefsSFXVolume;
        }
    }

    public void SavePlayerPrefs()
    {
        float gameDefaultMasterVolume;
        mainMixer.GetFloat("masterVolume", out gameDefaultMasterVolume);
        PlayerPrefs.SetFloat("settings_masterVolume", gameDefaultMasterVolume);

        float gameDefaultMusicVolume;
        mainMixer.GetFloat("musicVolume", out gameDefaultMusicVolume);
        PlayerPrefs.SetFloat("settings_musicVolume", gameDefaultMusicVolume);

        float gameDefaultSFXVolume;
        mainMixer.GetFloat("sfxVolume", out gameDefaultSFXVolume);
        PlayerPrefs.SetFloat("settings_sfxVolume", gameDefaultSFXVolume);

        PlayerPrefs.SetInt("settings_audio", 1);
        ReadPlayerPrefs();
    }

    public void SetVolumeMaster(float volume)
    {
        mainMixer.SetFloat("masterVolume", volume);
    }

    public void SetVolumeMusic(float volume)
    {
        mainMixer.SetFloat("musicVolume", volume);
    }

    public void SetVolumeSfx(float volume)
    {
        mainMixer.SetFloat("sfxVolume", volume);
    }

    void Start ()
    {
        ReadPlayerPrefs();
	}
}
