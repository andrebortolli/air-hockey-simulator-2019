//Programmed by: André Bortolli (RA: 16236796) and Gabriel Solano (RA: 16554685)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Scripts.DebugInfo;

public class GameController : MonoBehaviour
{
    private int[] playerScore = new int[2];
    public bool enableDebug;
    private DebugInformation dbgInfo;
    public TMP_Text fpsCounter;
    public TMP_Text frameCounter;
    public List<PlayerController> players;

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
