using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour
{
    public AudioClip playing;
    public AudioClip[] music;
    private AudioSource audioSource;
    int number = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
        audioSource.clip = playing;
        audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (audioSource.clip != playing)
        {
            audioSource.clip = playing;
            audioSource.Play();
        }
        else
        {
            if(audioSource.isPlaying == false)
            {
                if (number >= music.Length)
                {
                    number = 0;
                }
                else
                {
                    playing = music[number];
                    audioSource.clip = playing;
                    audioSource.Play();
                    number++;
                }  
            }
        }
	}
}
