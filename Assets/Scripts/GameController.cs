//Programmed by: André Bortolli (RA: 16236796) and Gabriel Solano (RA: 16554685)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Scripts.DebugInfo;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private string gameMode;
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
    public GameObject demoModeCameraPivot;
    public GameObject singlePlayerCamera;
    public GameObject multiPlayerCamera;
    public GameObject mainMenu;
    public List<GameObject> gameObjectsToFreezeOnPause;
    public List<GameObject> gameObjectsToEnableOnPause;
    public List<GameObject> gameObjectsToEnableOnUnpause;
    public Vector3[] frozenGameObjectsVelocities;
    public bool enableDebug;
    private DebugInformation dbgInfo;
    public TMP_Text fpsCounter;
    public TMP_Text frameCounter;
    public TMP_Text discInfo;
    public TMP_Text scoreInfo;
    public List<PlayerController> players;
    public GameObject disc;
    public Slider aiDifficultySlider;

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
                    players[i].aiResponse = Random.Range(0.50f, 0.75f);
                }
                demoModeCameraPivot.SetActive(true);
                singlePlayerCamera.SetActive(false);
                multiPlayerCamera.SetActive(false);
                break;

            case "sp": //Single Player Code
                gameMode = "sp";
                players[0].aI = false;
                players[1].aI = true;
                demoModeCameraPivot.SetActive(false);
                singlePlayerCamera.SetActive(true);
                multiPlayerCamera.SetActive(false);
                break;

            case "mp": //Multiplayer Code
                gameMode = "mp";
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].aI = false;
                }
                demoModeCameraPivot.SetActive(false);
                singlePlayerCamera.SetActive(false);
                multiPlayerCamera.SetActive(true);
                break;
            default:
                Debug.LogError("Incorrect usage! Use \"demo\" for demo mode; \"sp\" for single player mode; and \"mp\" for multiplayer mode.");
                break;
        }
    }

    public void StartGame(TMP_Dropdown dropdown)
    {
        switch(dropdown.value)
        {
            case 0:
                SetGameState("sp");
                for (int i =0; i < players.Count; i++)
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
            default: Debug.LogError("Error!");
                break;
        }
    }

    private void MainMenu()
    {
        SetGameState("demo");
        mainMenu.SetActive(true);
    }

    public void PauseGame(bool pause)
    {
        isPaused = pause;
        if (gameObjectsToFreezeOnPause != null)
        {
            if (!isPaused) //Unpause
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
                    if (audioSource)
                    {
                        audioSource.UnPause();
                    }
                }
                foreach (GameObject go in gameObjectsToEnableOnPause)
                {
                    go.SetActive(false);
                }
                foreach (GameObject go in gameObjectsToEnableOnUnpause)
                {
                    go.SetActive(true);
                }
            }
            else //Pause
            {
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
                foreach (GameObject go in gameObjectsToEnableOnPause)
                {
                    go.SetActive(true);
                }
                foreach (GameObject go in gameObjectsToEnableOnUnpause)
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
        discInfo.text = string.Format("Current Pos: {0}\nPredict Pos: {1}\nVelocity: {2}\nHeading: {3}\n", disc.transform.position, disc.GetComponentInChildren<Transform>().position, disc.GetComponent<Rigidbody>().velocity, disc.transform.eulerAngles.y);
        scoreInfo.text = string.Format("Score: [{0} | {1}]\t\t\t", players[0].GetPlayerScore(), players[1].GetPlayerScore());
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
        MainMenu();
    }
	// Update is called once per frame
	void Update ()
    {
        if (enableDebug)
        {
            UpdateDebugInformationUI();
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
