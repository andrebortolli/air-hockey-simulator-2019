using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReplayController : MonoBehaviour
{
    private GameController gameController;
    public bool cleanRecording = true;
    public int framesToStore = 1000;
    private bool isReplaying = false;
    private bool record = false;
    private int replayFrame = 0;
    private bool replayCameraToggled = false;
    public float secondsToWaitBeforeReset = 1f;
    public GameObject[] gameObjectsToRecord;
    List<Vector3>[] gameObjectsPositionXZAndRotation;
    public TMP_Text[] textToRecord;
    List<string>[] textToRecordTexts;
    public GameObject replayText;

    // Use this for initialization
    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void Start()
    {
        ResetReplayState();
    }

    public bool IsReplaying()
    {
        return isReplaying;
    }

    public bool IsRecording()
    {
        return record;
    }
    public int CurrentFrame()
    {
        return replayFrame;
    }
    public int StoredFrame()
    {
        return gameObjectsPositionXZAndRotation[0].Count;
    }

    void RecordGameObjects()
    {
        for (int i = 0; i < gameObjectsPositionXZAndRotation.Length; i++)
        {
            Vector3 aux = gameObjectsToRecord[i].transform.position;
            aux.y = gameObjectsToRecord[i].transform.eulerAngles.y;
            gameObjectsPositionXZAndRotation[i].Add(aux);
        }
        for (int i = 0; i < textToRecordTexts.Length; i++)
        {
            string aux = textToRecord[i].text;
            textToRecordTexts[i].Add(aux);
        }
    }

    public void ResetReplayState()
    {
        gameObjectsPositionXZAndRotation = new List<Vector3>[gameObjectsToRecord.Length];
        for (int i = 0; i < gameObjectsPositionXZAndRotation.Length; i++)
        {
            gameObjectsPositionXZAndRotation[i] = new List<Vector3>();
        }
        textToRecordTexts = new List<string>[textToRecord.Length];
        for (int i = 0; i < textToRecordTexts.Length; i++)
        {
            textToRecordTexts[i] = new List<string>();
        }
        isReplaying = false;
        record = false;
        replayFrame = 0;
        gameController.disablePausing = false;
        replayText.SetActive(false);
        gameController.PauseGame(false, false, false);  
        gameController.ToggleCurrentGameModeCamera();
        replayCameraToggled = false;
    }

    void PlayReplayFrames()
    {
        for (int i = 0; i < gameObjectsPositionXZAndRotation.Length; i++)
        {
            Vector3 aux = gameObjectsPositionXZAndRotation[i][replayFrame];
            gameObjectsToRecord[i].transform.position = new Vector3(aux.x, 0.35f, aux.z);
            gameObjectsToRecord[i].transform.eulerAngles = Vector3.up * aux.y;
        }
        for (int i = 0; i < textToRecordTexts.Length; i++)
        {
            string aux = textToRecordTexts[i][replayFrame];
            textToRecord[i].text = aux;
        }
    }

    public void SetRecordState(bool recording)
    {
        record = recording;
    }
    
    public void SetPlayState(bool play)
    {
        if (StoredFrame() != 0)
        {
            isReplaying = play;
        }
    }

    public void PlayReplay(int frames)
    {
        if (StoredFrame() != 0)
        {
            isReplaying = true;
            gameController.SetReplayCamera();
            gameController.PauseGame(true, false, false);
            gameController.disablePausing = true;
            replayText.SetActive(true);
            replayFrame = gameObjectsPositionXZAndRotation[0].Count - frames;
            replayFrame = Mathf.Clamp(replayFrame, 0, gameObjectsPositionXZAndRotation[0].Count - 1);
        }
    }

    IEnumerator WaitNSecondsAndResetReplayState(float n)
    {
        isReplaying = false;
        yield return new WaitForSeconds(n);
        ResetReplayState();
    }

    // Update is called once per frame
    void Update()
    {
        int frames = gameObjectsPositionXZAndRotation[0].Count;
        if (frames > framesToStore - 1 && cleanRecording)
        {
            for (int i = 0; i < gameObjectsToRecord.Length; i++)
            {
                gameObjectsPositionXZAndRotation[i].RemoveAt(0);
            }
            for (int i = 0; i < textToRecord.Length; i++)
            {
                textToRecordTexts[i].RemoveAt(0);
            }
        }
        if (isReplaying)
        {
            if(replayCameraToggled == false)
            {
                gameController.ToggleCamera("Replay Camera");
                replayCameraToggled = true;
            }
            PlayReplayFrames();
            replayFrame++;
            if (replayFrame >= gameObjectsPositionXZAndRotation[0].Count)
            {
                StartCoroutine(WaitNSecondsAndResetReplayState(secondsToWaitBeforeReset));
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
