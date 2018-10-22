using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HighscorePrompt : MonoBehaviour
{

    public Button okButton;
    public TMP_Text okInfoText;
    public TMP_Text[] player1Name;
    public TMP_Text[] player2Name;
    public TMP_Text player1Score;
    public TMP_Text player2Score;
    public TMP_Text highscoreListText;
    public int numberOfHighscoresToDisplay = 10;
    private GameController gameController;
    private HighscoreController highscoreController;
    public float timeToWaitBeforeReturningToMainMenu = 2.5f;

    void OnEnable()
    {
        gameController = FindObjectOfType<GameController>();
        highscoreController = FindObjectOfType<HighscoreController>();
        player1Score.text += gameController.players[0].GetPlayerScore();
        player2Score.text += gameController.players[1].GetPlayerScore();
        highscoreListText.text = highscoreController.ListHighscoreFromPlayerPrefs(numberOfHighscoresToDisplay, false);
        okInfoText.gameObject.SetActive(false);
        okButton.interactable = true;
    }
	
    public void Ok()
    {
        string player1NameString = "", player2NameString = "";
        for (int i = 0; i < player1Name.Length; i++)
        {
            player1NameString = player1NameString + player1Name[i].text;
        }
        for (int i = 0; i < player2Name.Length; i++)
        {
            player2NameString = player2NameString + player2Name[i].text;
        }
        highscoreController.HighscoreList.Add(new Highscore(player1NameString, player2NameString, gameController.players[0].GetPlayerScore(), gameController.players[1].GetPlayerScore()));
        highscoreController.SaveHighscoreInPlayerPrefs(highscoreController.HighscoreList);
        highscoreListText.text = highscoreController.ListHighscoreFromPlayerPrefs(numberOfHighscoresToDisplay, false);
        okButton.interactable = false;
        okInfoText.text = "Returning to Main Menu...";
        okInfoText.gameObject.SetActive(true);
    }

    public void ReturnToMainMenu(GameObject mainMenu)
    {
        StartCoroutine(WaitNSecondsAndReturnToMainMenu(timeToWaitBeforeReturningToMainMenu, mainMenu));
    }

    public IEnumerator WaitNSecondsAndReturnToMainMenu(float n, GameObject mainMenu)
    {
        yield return new WaitForSeconds(n);
        mainMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
