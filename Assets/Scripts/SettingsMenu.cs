using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mainMixer;

    //Lembrar de Salvar/Dar Load no playerprefs depois de mudar settings
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
}
