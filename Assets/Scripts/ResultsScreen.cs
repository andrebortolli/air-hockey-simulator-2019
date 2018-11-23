using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultsScreen : MonoBehaviour
{
    public TMP_Text player1Score, player2Score, results;
    private GameController gameController;
	// Use this for initialization
	void Awake ()
    {
        gameController = FindObjectOfType<GameController>();	
	}

    private void OnEnable()
    {
        player1Score.text = string.Format("{0:D2}", gameController.players[0].GetPlayerScore());
        player2Score.text = string.Format("{0:D2}", gameController.players[1].GetPlayerScore());
        if (gameController.players[0].GetPlayerScore() > gameController.players[1].GetPlayerScore())
        {
            if (gameController.players[0].aI)
            {
                results.text = "AI 1 Won!";
            }
            else
            {
                results.text = "Player 1 Won!";
            }
        }
        else if (gameController.players[0].GetPlayerScore() < gameController.players[1].GetPlayerScore())
        {
            if (gameController.players[1].aI)
            {
                results.text = "AI 2 Won!";
            }
            else
            {
                results.text = "Player 2 Won!";
            }
        }
        else if (gameController.players[0].GetPlayerScore() == gameController.players[1].GetPlayerScore())
        {
            results.text = "Draw!";
        }
        else
        {
            Debug.LogError("Error");
        }
    }
}
