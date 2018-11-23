using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultsScreen : MonoBehaviour
{
    public TMP_Text player1 ,player2, player1Score, player2Score, results;
    private GameController gameController;
    Color32 player1Color = new Color32(102,64,255,255), player2Color = new Color32(255, 119, 91,255);
	// Use this for initialization
	void Awake ()
    {
        gameController = FindObjectOfType<GameController>();	
	}

    private void OnEnable()
    {
        if (gameController.players[0].aI)
        {
            player1.text = "AI 1";
        }
        else
        {
            player1.text = "Player 1";
        }
        if (gameController.players[1].aI)
        {
            player2.text = "AI 2";
        }
        else
        {
            player2.text = "Player 2";
        }
        player1Score.text = string.Format("{0:D2}", gameController.players[0].GetPlayerScore());
        player2Score.text = string.Format("{0:D2}", gameController.players[1].GetPlayerScore());
        if (gameController.players[0].GetPlayerScore() > gameController.players[1].GetPlayerScore())
        {
            if (gameController.players[0].aI)
            {
                results.GetComponent<TextMeshProUGUI>().faceColor = player1Color;
                results.text = "AI 1 Won!";
            }
            else
            {
                results.GetComponent<TextMeshProUGUI>().faceColor = player1Color;
                results.text = "Player 1 Won!";
            }
        }
        else if (gameController.players[0].GetPlayerScore() < gameController.players[1].GetPlayerScore())
        {
            if (gameController.players[1].aI)
            {
                results.GetComponent<TextMeshProUGUI>().faceColor = player2Color;
                results.text = "AI 2 Won!";
            }
            else
            {
                results.GetComponent<TextMeshProUGUI>().faceColor = player2Color;
                results.text = "Player 2 Won!";
            }
        }
        else if (gameController.players[0].GetPlayerScore() == gameController.players[1].GetPlayerScore())
        {
            results.GetComponent<TextMeshProUGUI>().faceColor = Color.white;
            results.text = "Draw!";
        }
        else
        {
            Debug.LogError("Error");
        }
    }
}
