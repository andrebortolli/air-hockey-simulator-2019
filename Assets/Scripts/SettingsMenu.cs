using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public GameController gameController;
    public AudioMixerController audioMixerController;
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject confirmationPrompt;
    public GameObject errorPrompt;
    public TMP_Dropdown player1Controls, player2Controls;
    public Toggle player1Inverse, player2Inverse;
    public Slider masterSlider, musicSlider, sfxSlider;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void OnEnable()
    {
        gameController.disablePausing = true;
    }
    private void OnDisable()
    {
        if (gameController.GameMode != "menu")
        {
            gameController.disablePausing = false;
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
        audioMixerController.SetVolumeMaster(volume);
    }

    public void SetVolumeMusic(float volume)
    {
        audioMixerController.SetVolumeMusic(volume);
    }

    public void SetVolumeSfx(float volume)
    {
        audioMixerController.SetVolumeSfx(volume);
    }

    public void Ok()
    {
        if (ChangePlayerInput())
        {
            audioMixerController.SavePlayerPrefs();
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
        audioMixerController.ReadPlayerPrefs();
    }
}
