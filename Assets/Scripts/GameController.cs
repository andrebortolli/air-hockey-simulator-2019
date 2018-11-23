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
    public GameObject settingsMenu;
    private ReplayController replayController;
    private Timer gameTimer;
    public float matchTime = 3600f;
    private ReplayCameraController replayCameraController;
    public GameObject[] cameras;
    private bool inGame = false;
    private bool isVSAI;
    public bool IsVSAI
    {
        get
        {
            return isVSAI;
        }
    }
    public bool isHighscoreEditMode = false;
    public bool disablePausing = false;

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
    public TMP_Dropdown gameTypeDropdown;
    public TMP_Dropdown matchTimeDropdown;
    public HighscorePrompt highscorePrompt;
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
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                gameMode = "demo";
                inGame = true;
                isVSAI = true;
                isHighscoreEditMode = false;
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].aI = true;
                    players[i].UpdatePlayerType();
                }
                disc.GetComponent<Disc>().ResetDisc(false);
                player1ScoreUI.gameObject.SetActive(true);
                player2ScoreUI.gameObject.SetActive(true);
                timerUI.gameObject.SetActive(true);
                disablePausing = false;
                ToggleCamera("Demo Camera");
                gameTimer.ResetTimer(true, matchTime);
                gameTimer.SetClockState(true);
                break;

            case "sp": //Single Player Code
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                gameMode = "sp";
                inGame = true;
                isVSAI = true;
                isHighscoreEditMode = true;
                players[0].aI = false;
                players[0].UpdatePlayerType();
                players[1].aI = true;
                players[1].UpdatePlayerType();
                disc.GetComponent<Disc>().ResetDisc(true);
                player1ScoreUI.gameObject.SetActive(true);
                player2ScoreUI.gameObject.SetActive(true);
                timerUI.gameObject.SetActive(true);
                disablePausing = false;
                ToggleCamera("SP Camera");
                gameTimer.ResetTimer(true, matchTime);
                gameTimer.SetClockState(true);
                break;

            case "mp": //Multiplayer Code
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                gameMode = "mp";
                inGame = true;
                isVSAI = false;
                isHighscoreEditMode = true;
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].aI = false;
                    players[i].UpdatePlayerType();
                }
                disc.GetComponent<Disc>().ResetDisc(true);
                player1ScoreUI.gameObject.SetActive(true);
                player2ScoreUI.gameObject.SetActive(true);
                timerUI.gameObject.SetActive(true);
                disablePausing = false;
                ToggleCamera("MP Camera");
                gameTimer.ResetTimer(true, matchTime);
                gameTimer.SetClockState(true);
                break;
            case "menu":
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                gameMode = "menu";
                inGame = false;
                isVSAI = false;
                isHighscoreEditMode = false;
                gameTimer.ResetTimer(false, 0.0f);
                replayController.ResetReplayState();
                disc.GetComponent<Disc>().ResetDisc(false);
                player1ScoreUI.gameObject.SetActive(false);
                player2ScoreUI.gameObject.SetActive(false);
                timerUI.gameObject.SetActive(false);
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].aI = true;
                    players[i].aiResponse = Random.Range(0.50f, 0.85f);
                    players[i].UpdatePlayerType();
                }
                disablePausing = true;
                ToggleCamera("Demo Camera");
                gameTimer.SetClockState(true);
                mainMenu.SetActive(true);

                break;
            case "highscorePrompt":
                if (gameMode != "demo")
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    gameMode = "menu";
                    inGame = false;
                    replayController.ResetReplayState();
                    player1ScoreUI.gameObject.SetActive(false);
                    player2ScoreUI.gameObject.SetActive(false);
                    timerUI.gameObject.SetActive(false);
                    PauseGame(true, false, false);
                    disablePausing = true;
                    EnableMenu(highscorePrompt.gameObject);
                }
                else
                {
                    SetGameState("menu");
                }
                break;
            default:
                Debug.LogError("Incorrect usage! Use \"demo\" for demo mode; \"sp\" for single player mode;  \"mp\" for multiplayer mode; and \"menu\" for Menu Mode.");
                break;
        }
    }

    public void StartGame()
    {
        switch (matchTimeDropdown.value)
        {
            case 0:
                matchTime = 180f;
                break;
            case 1:
                matchTime = 300f;
                break;
            case 2:
                matchTime = 600f;
                break;
            case 3:
                matchTime = 900f;
                break;
            case 4:
                matchTime = 1800f;
                break;
            case 5:
                matchTime = 2700f;
                break;
            case 6:
                matchTime = 3600f;
                break;
            case 7:
                matchTime = 5400f;
                break;
            default:
                matchTime = 360000f;
                break;
        }
        switch (gameTypeDropdown.value)
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
        else if (GameMode == "menu")
        {
            ToggleCamera("Demo Camera");
        }
    }

    public void SetReplayCamera(bool random = true, int replayCameraNumber = 0)
    {
        int amountOfReplayCameras = 0;
        int replayCameraArrayIndex = 0;
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i].tag == "Replay Camera")
            {
                replayCameraArrayIndex = i;
                amountOfReplayCameras++;
            }
        }
        if (amountOfReplayCameras > 1)
        {
            Debug.LogWarning("There is more than one Replay Camera active in the Array!");
        }
        else
        {
            if (random)
            {
                cameras[replayCameraArrayIndex] = replayCameraController.GetRandomCamera();
            }
            else
            {
                cameras[replayCameraArrayIndex] = replayCameraController.GetCamera(replayCameraNumber);
            }
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
        SetGameState("menu");
    }

    void ToggleFreezeGameObjects(bool toggle, bool pauseAudio)
    {
        if (gameObjectsToFreezeOnPause != null)
        {
            if (toggle)
            {
                for (int i = 0; i < gameObjectsToFreezeOnPause.Count; i++)
                {
                    if (pauseAudio == false && gameObjectsToFreezeOnPause[i].gameObject.tag == "BGM Controller")
                    {
                        gameObjectsToFreezeOnPause[i].GetComponent<MonoBehaviour>().enabled = true;
                    }
                    else
                    {
                        gameObjectsToFreezeOnPause[i].GetComponent<MonoBehaviour>().enabled = false;
                    }
                    Rigidbody rb = gameObjectsToFreezeOnPause[i].GetComponent<Rigidbody>();
                    AudioSource audioSource = gameObjectsToFreezeOnPause[i].GetComponent<AudioSource>();
                    if (rb)
                    {
                        frozenGameObjectsVelocities[i] = rb.velocity;
                        rb.isKinematic = true;
                        rb.detectCollisions = false;
                    }
                    if (pauseAudio)
                    {
                        if (audioSource)
                        {
                            audioSource.Pause();
                        }
                    }
                }
            }
            else
            {
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
                    if (pauseAudio)
                    {
                        if (audioSource)
                        {
                            audioSource.UnPause();
                        }
                    }
                }
            }
        }
    }

    public void PauseGame(bool pause, bool showCanvas = true, bool pauseAudio = true)
    {
        if (mainMenu.activeSelf == false && disablePausing == false)
        {
            isPaused = pause;
            if (!isPaused) //Unpause
            {
                ToggleFreezeGameObjects(false, pauseAudio);
                if (gameMode != "menu")
                {
                    replayController.SetRecordState(true);
                }
                if (showCanvas)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
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
                ToggleFreezeGameObjects(true, pauseAudio);
                replayController.SetRecordState(false);
                if (showCanvas)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
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

    void CheckIfGameOver()
    {
        if (gameTimer.GetGameClock() <= 0.0f && inGame == true)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        SetGameState("highscorePrompt");
    }

    public void SetPause(bool pause)
    {
        PauseGame(pause);
    }

    private void Awake()
    {
        gameTimer = FindObjectOfType<Timer>();
        replayController = FindObjectOfType<ReplayController>();
        replayCameraController = FindObjectOfType<ReplayCameraController>();
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
        CheckIfGameOver();
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!disablePausing)
            {
                PauseGame(!IsPaused);
            }
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetGameState("menu");
        }
    }
}
