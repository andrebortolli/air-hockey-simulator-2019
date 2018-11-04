using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Scripts.DebugInfo;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private string gameMode;
    public string GameMode
    {
        get
        {
            return gameMode;
        }
    }
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
    public GameObject mainMenu;
    private ReplayController replayController;
    private Timer gameTimer;
    public GameObject[] cameras;

    #region Pause
    public List<GameObject> gameObjectsToFreezeOnPause;
    public List<GameObject> gameObjectsToEnableOnPause;
    public List<GameObject> gameObjectsToEnableOnUnpause;
    public Vector3[] frozenGameObjectsVelocities;
    #endregion

    #region Debug
    public bool enableDebug;
    private DebugInformation dbgInfo;
    public TMP_Text fpsCounter;
    public TMP_Text frameCounter;
    public TMP_Text discInfo;
    public TMP_Text recordingInfo;
    public TMP_Text scoreInfo;
    #endregion

    #region UI
    public TMP_Text player1ScoreUI;
    public TMP_Text player2ScoreUI;
    public TMP_Text timerUI;
    #endregion

    public List<PlayerController> players;
    public Slider aiDifficultySlider;
    public GameObject disc;

    public void EnableMenu(GameObject menu)
    {
        menu.SetActive(true);
    }
    public void DisableMenu(GameObject menu)
    {
        menu.SetActive(false);
    }

    public void SetGameState(string state)
    {
        switch (state)
        {
            case "demo": //Demo Code
                gameMode = "demo";
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].aI = true;
                    players[i].aiResponse = Random.Range(0.50f, 0.85f);
                }
                disc.GetComponent<Disc>().ResetDisc(false);
                ToggleCamera("Demo Camera");
                gameTimer.ResetTimer();
                gameTimer.SetClockState(true);
                break;

            case "sp": //Single Player Code
                gameMode = "sp";
                players[0].aI = false;
                players[1].aI = true;
                disc.GetComponent<Disc>().ResetDisc(true);
                ToggleCamera("SP Camera");
                gameTimer.ResetTimer();
                gameTimer.SetClockState(true);
                break;

            case "mp": //Multiplayer Code
                gameMode = "mp";
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].aI = false;
                }
                disc.GetComponent<Disc>().ResetDisc(true);
                ToggleCamera("MP Camera");
                gameTimer.ResetTimer();
                gameTimer.SetClockState(true);
                break;

            default:
                Debug.LogError("Incorrect usage! Use \"demo\" for demo mode; \"sp\" for single player mode; and \"mp\" for multiplayer mode.");
                break;
        }
    }

    public void StartGame(TMP_Dropdown dropdown)
    {
        switch (dropdown.value)
        {
            case 0:
                SetGameState("sp");
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].aI == true)
                    {
                        players[i].aiResponse = aiDifficultySlider.value;
                    }
                    players[i].ResetPlayerScore();
                }
                break;
            case 1:
                SetGameState("mp");
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].aI == true)
                    {
                        players[i].aiResponse = aiDifficultySlider.value;
                    }
                    players[i].ResetPlayerScore();
                }
                break;
            case 2:
                SetGameState("demo");
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].aI == true)
                    {
                        players[i].aiResponse = aiDifficultySlider.value;
                    }
                    players[i].ResetPlayerScore();
                }
                break;
            default:
                Debug.LogError("Error!");
                break;
        }
    }

    public void ToggleCamera(string cameraTag)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i].gameObject.tag == cameraTag)
            {
                cameras[i].SetActive(true);
            }
            else
            {
                cameras[i].SetActive(false);
            }
        }
    }

    public void ToggleCurrentGameModeCamera()
    {
        if (GameMode == "demo")
        {
            ToggleCamera("Demo Camera");
        }
        else if (GameMode == "sp")
        {
            ToggleCamera("SP Camera");
        }
        else if (GameMode == "mp")
        {
            ToggleCamera("MP Camera");
        }
    }

    public GameObject ActiveCamera()
    {
        int activeCamerasNumber = 0;
        GameObject activeCamera = null;
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i].activeSelf)
            {
                activeCamera = cameras[i];
                activeCamerasNumber++;
            }
        }
        if (activeCamerasNumber > 1)
        {
            Debug.LogWarning("More than one camera is active. Returning the last camera found active in the array.");
        }
        return activeCamera;
    }

    private void MainMenu()
    {
        SetGameState("demo");
        mainMenu.SetActive(true);
    }

    public void PauseGame(bool pause, bool showCanvas = true)
    {
        if (!replayController.IsReplaying() && mainMenu.activeSelf == false)
        {
            isPaused = pause;
            if (gameObjectsToFreezeOnPause != null)
            {
                if (!isPaused) //Unpause
                {
                    replayController.SetRecordState(true);
                    for (int i = 0; i < gameObjectsToFreezeOnPause.Count; i++)
                    {
                        gameObjectsToFreezeOnPause[i].GetComponent<MonoBehaviour>().enabled = true;
                        Rigidbody rb = gameObjectsToFreezeOnPause[i].GetComponent<Rigidbody>();
                        AudioSource audioSource = gameObjectsToFreezeOnPause[i].GetComponent<AudioSource>();
                        if (rb && frozenGameObjectsVelocities != null)
                        {
                            rb.isKinematic = false;
                            rb.detectCollisions = true;
                            rb.velocity = frozenGameObjectsVelocities[i];
                        }
                        if (audioSource)
                        {
                            audioSource.UnPause();
                        }
                    }
                    if (showCanvas)
                    {
                        ToggleCurrentGameModeCamera();
                        if (gameObjectsToEnableOnPause != null)
                        {
                            foreach (GameObject go in gameObjectsToEnableOnPause)
                            {
                                if (go)
                                {
                                    go.SetActive(false);
                                }
                            }
                        }
                        if (gameObjectsToEnableOnUnpause != null)
                        {
                            foreach (GameObject go in gameObjectsToEnableOnUnpause)
                            {
                                if (go)
                                {
                                    go.SetActive(true);
                                }
                            }
                        }
                    }
                    gameTimer.SetClockState(true);
                }
                else //Pause
                {
                    replayController.SetRecordState(false);
                    frozenGameObjectsVelocities = new Vector3[gameObjectsToFreezeOnPause.Count];
                    for (int i = 0; i < gameObjectsToFreezeOnPause.Count; i++)
                    {
                        gameObjectsToFreezeOnPause[i].GetComponent<MonoBehaviour>().enabled = false;
                        Rigidbody rb = gameObjectsToFreezeOnPause[i].GetComponent<Rigidbody>();
                        AudioSource audioSource = gameObjectsToFreezeOnPause[i].GetComponent<AudioSource>();
                        if (rb)
                        {
                            frozenGameObjectsVelocities[i] = rb.velocity;
                            rb.isKinematic = true;
                            rb.detectCollisions = false;
                        }
                        if (audioSource)
                        {
                            audioSource.Pause();
                        }
                    }
                    if (showCanvas)
                    {
                        ToggleCamera("Demo Camera");
                        if (gameObjectsToEnableOnPause != null)
                        {
                            foreach (GameObject go in gameObjectsToEnableOnPause)
                            {
                                if (go)
                                {
                                    go.SetActive(true);
                                }
                            }
                        }
                        if (gameObjectsToEnableOnUnpause != null)
                        {
                            foreach (GameObject go in gameObjectsToEnableOnUnpause)
                            {
                                if (go)
                                {
                                    go.SetActive(false);
                                }
                            }
                        }
                    }
                    gameTimer.SetClockState(false);
                }
            }
        }
    }

    private void UpdateDebugInformationUI()
    {
        fpsCounter.text = string.Format("Framerate: {0:00.0} FPS", dbgInfo.GetFramerateSec());
        frameCounter.text = string.Format("Frametime: {0:00.0} ms / {1:00.0} ms", dbgInfo.GetFrametime(), dbgInfo.GetPhysicsFrametime()); //Not optimal, since Update time is slower than FixedUpdate. For now it works, but it would be best to separate Update and Physics time.
        discInfo.text = string.Format("Current Pos: {0}\nPredict Pos: {1}\nVelocity: {2}\nHeading: {3}\n", disc.transform.position, disc.GetComponentInChildren<Transform>().position, disc.GetComponent<Rigidbody>().velocity, disc.transform.eulerAngles.y);
        recordingInfo.text = string.Format("Recording:\t{0}\nIsReplaying:\t\t{1}\nCurrent Frame:\t{2}\nRecorded Frames:\t{3}", replayController.IsRecording(), replayController.IsReplaying(), replayController.CurrentFrame(), replayController.StoredFrame());
        scoreInfo.text = string.Format("Game Mode:\t{0}\nScore:\t[{1}|{2}]\nIsAI:\t[{3}:{4}]\nAI Proficiency:\t[{5}|{6}]\nGame Paused:\t{7}", gameMode, players[0].GetPlayerScore(), players[1].GetPlayerScore(), players[0].aI, players[1].aI, players[0].aiResponse, players[1].aiResponse, isPaused);
    }

    public void FindPlayers()
    {
        players = new List<PlayerController>(FindObjectsOfType<PlayerController>());
    }

    private void Awake()
    {
        gameTimer = FindObjectOfType<Timer>();
        replayController = FindObjectOfType<ReplayController>();
        frozenGameObjectsVelocities = new Vector3[gameObjectsToFreezeOnPause.Count];
        if (enableDebug)
        {
            dbgInfo = gameObject.AddComponent<DebugInformation>(); //Only adds the component if the checkbox is true on Inspector.
        }
    }
    // Use this for initialization
    void Start()
    {
        FindPlayers();
        MainMenu();
    }
    // Update is called once per frame
    void Update()
    {
        if (enableDebug)
        {
            UpdateDebugInformationUI();
        }
        if (!replayController.IsReplaying())
        {
            player1ScoreUI.text = string.Format("{0}", players[0].GetPlayerScore());
            player2ScoreUI.text = string.Format("{0}", players[1].GetPlayerScore());
            timerUI.text = gameTimer.GetGameClockToString();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame(!IsPaused);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SetGameState("demo");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SetGameState("sp");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SetGameState("mp");
        }
    }
}
