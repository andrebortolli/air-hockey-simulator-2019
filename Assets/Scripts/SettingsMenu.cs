using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mainMixer;
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject confirmationPrompt;
    public GameObject errorPrompt;
    public GameController gameController;
    public TMP_Dropdown player1Controls, player2Controls;
    public Toggle player1Inverse, player2Inverse;
    public Slider masterSlider, musicSlider, sfxSlider;

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
            masterSlider.value = playerPrefsMasterVolume;
            float playerPrefsMusicVolume = PlayerPrefs.GetFloat("settings_musicVolume");
            SetVolumeMusic(playerPrefsMusicVolume);
            musicSlider.value = playerPrefsMusicVolume;
            float playerPrefsSFXVolume = PlayerPrefs.GetFloat("settings_sfxVolume");
            SetVolumeSfx(playerPrefsSFXVolume);
            sfxSlider.value = playerPrefsSFXVolume;
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

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();

    }

    private void Start()
    {
        if (mainMixer)
        {
            ReadPlayerPrefs();
        }
        else
        {
            Debug.LogError("No mainMixer!");
        }
    }

    public bool ChangePlayerInput()
    {
        if (player1Controls && player2Controls && player1Controls.value != player2Controls.value)
        {
            bool p1Inverse = player1Inverse.isOn;
            switch(player1Controls.value)
            {
                case 0:
                    gameController.players[0].ChangePlayerMovementAxes("HorizontalP1Mouse", "VerticalP1Mouse", "TriggerP1Mouse", p1Inverse);
                    break;
                case 1:
                    gameController.players[0].ChangePlayerMovementAxes("HorizontalP1", "VerticalP1", "TriggerP1", p1Inverse);
                    break;
                case 2:
                    gameController.players[0].ChangePlayerMovementAxes("HorizontalP2", "VerticalP2", "TriggerP2", p1Inverse);
                    break;
            }
            bool p2Inverse = player2Inverse.isOn;
            switch (player2Controls.value)
            {
                case 0:
                    gameController.players[1].ChangePlayerMovementAxes("HorizontalP1Mouse", "VerticalP1Mouse", "TriggerP1Mouse", p2Inverse);
                    break;
                case 1:
                    gameController.players[1].ChangePlayerMovementAxes("HorizontalP1", "VerticalP1", "TriggerP1", p2Inverse);
                    break;
                case 2:
                    gameController.players[1].ChangePlayerMovementAxes("HorizontalP2", "VerticalP2", "TriggerP2", p2Inverse);
                    break;
            }
            return true;
        }
        else
        {
            return false;
        }
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

    public void Ok()
    {
        if (ChangePlayerInput())
        {
            SavePlayerPrefs();
            if (gameController.GameMode != "menu")
            {
                gameController.EnableMenu(pauseMenu);
                gameController.DisableMenu(this.gameObject);
            }
            else
            {
                gameController.EnableMenu(mainMenu);
                gameController.DisableMenu(this.gameObject);
            }
        }
        else
        {
            gameController.EnableMenu(errorPrompt);
        }
    }
    public void Return()
    {
        if (gameController.GameMode != "menu")
        {
            gameController.EnableMenu(pauseMenu);
            gameController.DisableMenu(this.gameObject);
        }
        else
        {
            gameController.EnableMenu(mainMenu);
            gameController.DisableMenu(this.gameObject);
        }
    }
}
