using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayController : MonoBehaviour
{
    private GameController gameController;
    private bool isReplay = false;
    private bool record = false;
    private int replayFrame = 0;
    public GameObject[] gameObjectsToRecord;
    List<Vector3>[] gameObjectsPositionXZAndRotation;

	// Use this for initialization
	void Awake ()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void Start()
    {
        ResetReplayState();
    }

    public bool isReplaying()
    {
        return isReplay;
    }

    void RecordGameObjects()
    {
        for (int i = 0; i < gameObjectsPositionXZAndRotation.Length; i++)
        {
            Vector3 aux = gameObjectsToRecord[i].transform.position;
            aux.y = gameObjectsToRecord[i].transform.eulerAngles.y;
            gameObjectsPositionXZAndRotation[i].Add(aux);
        }
    }

    public void ResetReplayState()
    {
        gameObjectsPositionXZAndRotation = new List<Vector3>[3];
        for (int i = 0; i < gameObjectsPositionXZAndRotation.Length; i++)
        {
            gameObjectsPositionXZAndRotation[i] = new List<Vector3>();
        }
        isReplay = false;
        record = false;
        replayFrame = 0;
        gameController.PauseGame(false, false);
    }

    void PlayReplayFrames()
    {
        for (int i = 0; i < gameObjectsPositionXZAndRotation.Length; i++)
        {
            Vector3 aux = gameObjectsPositionXZAndRotation[i][replayFrame];
            gameObjectsToRecord[i].transform.position = new Vector3(aux.x, 0.35f, aux.z);
            gameObjectsToRecord[i].transform.eulerAngles = Vector3.up * aux.y;
        }
    }

    public void SetRecordState(bool recording)
    {
        record = recording;
    }

    public void PlayReplay(int frames)
    {
        isReplay = true;
        replayFrame = gameObjectsPositionXZAndRotation[0].Count - frames;
        replayFrame = Mathf.Clamp(replayFrame, 0, gameObjectsPositionXZAndRotation[0].Count - 1);
    }

    // Update is called once per frame
        void Update()
    {
        if (isReplay)
        {
            if (replayFrame == 0)
            {
                gameController.PauseGame(true, false);
            }
            PlayReplayFrames();
            replayFrame++;
            if (replayFrame >= gameObjectsPositionXZAndRotation[0].Count)
            {
                ResetReplayState();
            }
        }
        else
        {
            if (record)
            {
                RecordGameObjects();
            }
        }
    }
}
