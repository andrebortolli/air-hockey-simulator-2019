using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoresUIDEBUG : MonoBehaviour 
{
    private GameController gameController;
    private HighscoreController highscoreController;
    public TMP_Text highscoreListText;

	void OnEnable () 
    {
        gameController = FindObjectOfType<GameController>();
        highscoreController = FindObjectOfType<HighscoreController>();
    }
	
	// Update is called once per frame
	void Update () 
    {
        highscoreListText.text = highscoreController.ListHighscoreFromPlayerPrefs(highscoreController.HighscoreList.Count, true);
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            highscoreController.HighscoreList.Add(new Highscore("DBG", "DBG", Random.Range(0, 10), Random.Range(0, 10)));
            highscoreController.SaveHighscoreInPlayerPrefs(highscoreController.HighscoreList);
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            highscoreController.HighscoreList.Clear();
            PlayerPrefs.DeleteAll();
            highscoreController.SaveHighscoreInPlayerPrefs(highscoreController.HighscoreList);
        }
    }
}
