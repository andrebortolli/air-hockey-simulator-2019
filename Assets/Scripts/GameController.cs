//Programmed by: André Bortolli (RA: 16236796) and Gabriel Solano (RA: 16554685)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Scripts.DebugInfo;
using ScriptTools.ObjectState;

public class GameController : MonoBehaviour
{
    private SaveGameObjectState saveGameObjectState = new SaveGameObjectState();
    private bool isMainMenuVisible;
    public bool IsMainMenuVisible
    {
        get
        {
            return isMainMenuVisible;
        }
        set
        {
            isMainMenuVisible = value;
        }
    }
    private bool isPaused;
    public bool IsPaused
    {
        get
        {
            return isPaused;
        }
    }
    public List<GameObject> gameObjectsToFreezeOnPause;
    public List<GameObject> gameObjectsToDisplayOnPause;
    public Vector3[] frozenGameObjectsVelocities;
    private int[] playerScore = new int[2];
    public bool enableDebug;
    private DebugInformation dbgInfo;
    public TMP_Text fpsCounter;
    public TMP_Text frameCounter;
    public List<PlayerController> players;

    public void DemoMode(bool value)
    {
        //Demo Mode code here.
    }

    //public void PauseGame(bool pause)
    //{
    //    isPaused = pause;
    //    if (gameObjectsToFreezeOnPause != null)
    //    {
    //        if (!isPaused)
    //        {
    //            for (int i = 0; i < gameObjectsToFreezeOnPause.Count; i++)
    //            {
    //                gameObjectsToFreezeOnPause[i].GetComponent<MonoBehaviour>().enabled = true;
    //                Rigidbody rb = gameObjectsToFreezeOnPause[i].GetComponent<Rigidbody>();
    //                if (rb && frozenGameObjectsVelocities != null)
    //                {
    //                    rb.isKinematic = false;
    //                    rb.detectCollisions = true;
    //                    rb.velocity = frozenGameObjectsVelocities[i];
    //                }
    //            }
    //            foreach (GameObject go in gameObjectsToDisplayOnPause)
    //            {
    //                go.SetActive(true);
    //            }
    //        }
    //        else
    //        {
    //            frozenGameObjectsVelocities = new Vector3[gameObjectsToFreezeOnPause.Count];
    //            for (int i = 0; i < gameObjectsToFreezeOnPause.Count; i++)
    //            {
    //                gameObjectsToFreezeOnPause[i].GetComponent<MonoBehaviour>().enabled = false;
    //                Rigidbody rb = gameObjectsToFreezeOnPause[i].GetComponent<Rigidbody>();
    //                if (rb)
    //                {
    //                    frozenGameObjectsVelocities[i] = rb.velocity;
    //                    rb.isKinematic = true;
    //                    rb.detectCollisions = false;
    //                }
    //            }
    //            foreach (GameObject go in gameObjectsToDisplayOnPause)
    //            {
    //                go.SetActive(false);
    //            }
    //        }
    //    }
    //}

    public void PauseGame(bool pause)
    {
        isPaused = pause;
        if (gameObjectsToFreezeOnPause != null)
        {
            if (!isPaused)
            {
                for (int i = 0; i < gameObjectsToFreezeOnPause.Count; i++)
                {
                    gameObjectsToFreezeOnPause[i].GetComponent<MonoBehaviour>().enabled = true;
                    Rigidbody rb = gameObjectsToFreezeOnPause[i].GetComponent<Rigidbody>();
                    if (rb && frozenGameObjectsVelocities != null)
                    {
                        rb.isKinematic = false;
                        rb.detectCollisions = true;
                        rb.velocity = frozenGameObjectsVelocities[i];
                    }
                }
                foreach (GameObject go in gameObjectsToDisplayOnPause)
                {
                    go.SetActive(true);
                }
            }
            else
            {
                frozenGameObjectsVelocities = new Vector3[gameObjectsToFreezeOnPause.Count];
                for (int i = 0; i < gameObjectsToFreezeOnPause.Count; i++)
                {
                    gameObjectsToFreezeOnPause[i].GetComponent<MonoBehaviour>().enabled = false;
                    Rigidbody rb = gameObjectsToFreezeOnPause[i].GetComponent<Rigidbody>();
                    if (rb)
                    {
                        frozenGameObjectsVelocities[i] = rb.velocity;
                        rb.isKinematic = true;
                        rb.detectCollisions = false;
                    }
                }
                foreach (GameObject go in gameObjectsToDisplayOnPause)
                {
                    go.SetActive(false);
                }
            }
        }
    }
    private void UpdateDebugInformationUI()
    {
        fpsCounter.text = string.Format("Framerate: {0:00.0} FPS", dbgInfo.GetFramerateSec());
        frameCounter.text = string.Format("Frametime: {0:00.0} ms / {1:00.0} ms", dbgInfo.GetFrametime(), dbgInfo.GetPhysicsFrametime()); //Not optimal, since Update time is slower than FixedUpdate. For now it works, but it would be best to separate Update and Physics time.
    }

    public void FindPlayers()
    {
        players = new List<PlayerController>(FindObjectsOfType<PlayerController>());
    }

    private void Awake()
    {
        if (enableDebug)
        {
            dbgInfo = gameObject.AddComponent<DebugInformation>(); //Only adds the component if the checkbox is true on Inspector.
        }
    }
    // Use this for initialization
    void Start ()
    {
        FindPlayers();
    }
	// Update is called once per frame
	void Update ()
    {
        if (enableDebug)
        {
            UpdateDebugInformationUI();
        } 
	}
}
