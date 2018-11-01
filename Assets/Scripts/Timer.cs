﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private bool active;
    private float gameClock;
    Vector3Int gameClockHMS;

    public void StopTimer()
    {
        active = false;
    }
    public void ResumeTimer()
    {
        active = true;
    }
    public Vector3Int GetGameClockXHYMZS()
    {
        return gameClockHMS;
    }
    public string GetGameClockToString()
    {
        return string.Format("{0:D2}:{1:D2}:{2:D2}", gameClockHMS.x, gameClockHMS.y, gameClockHMS.z);
    }
    public void ResetTimer()
    {
        gameClock = 0.0f;
    }

	void Update ()
    {
		if (active)
        {
            gameClock += Time.deltaTime;
            gameClockHMS = new Vector3Int((int) gameClock / 3600, (int) (gameClock / 60) % 60, (int) (gameClock % 60));
        }
	}
}
