using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private bool active;
    public bool decrement = false;
    private float gameClock;
    Vector3Int gameClockHMS;
    
    public void SetClockState(bool enabled)
    {
        active = enabled;
    }

    public float GetGameClock()
    {
        return gameClock;
    }

    public Vector3Int GetGameClockXHYMZS()
    {
        return gameClockHMS;
    }
    public string GetGameClockToString()
    {
        if(gameClock >= 3600)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", gameClockHMS.x, gameClockHMS.y, gameClockHMS.z);
        }
        else
        {
            return string.Format("{0:D2}:{1:D2}", gameClockHMS.y, gameClockHMS.z);
        }
    }
    public void ResetTimer(bool decrement, float timeToSet)
    {
        this.decrement = decrement;
        if (decrement == true)
        {
            gameClock = timeToSet;
        }
        else
        {
            gameClock = 0.0f;
        }
    }

	void Update ()
    {
		if (active)
        {
            if (decrement)
            {
                gameClock -= Time.deltaTime;
            }
            else
            {
                gameClock += Time.deltaTime;
            }
            gameClockHMS = new Vector3Int((int) gameClock / 3600, (int) (gameClock / 60) % 60, (int) (gameClock % 60));
        }
	}
}
