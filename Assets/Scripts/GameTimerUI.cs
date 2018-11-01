using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimerUI : MonoBehaviour
{
    private TMP_Text timerText;
    private Timer gameTime;

    void Awake()
    {
        timerText = GetComponent<TMP_Text>();
        gameTime = FindObjectOfType<Timer>();
    }

    // Update is called once per frame
    void Update ()
    {
        timerText.text = gameTime.GetGameClockToString();
	}
}
