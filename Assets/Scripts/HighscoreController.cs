using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HighscoreController : MonoBehaviour
{

    public void Awake()
    {
        highscoreList = new List<Highscore>(ReadHighscoresFromPlayerPrefs());
    }

    private List<Highscore> highscoreList;
    public List<Highscore> HighscoreList
    {
        get
        {
            return highscoreList;
        }
        set
        {
            highscoreList = new List<Highscore>(value);
        }
    }

    public List<Highscore> OrderHighscoreByScoreDescend(List<Highscore> input)
    {
        return input.OrderByDescending(h => h.GetPlayer1Score()).ToList();
    }

    public void SaveHighscoreInPlayerPrefs(List<Highscore> input)
    {
        input = OrderHighscoreByScoreDescend(input);
        for (int i = 0; i < PlayerPrefs.GetInt("Highscore_Count"); i++)
        {
            PlayerPrefs.DeleteKey("Highscore_" + i + "_Player1Name");
            PlayerPrefs.DeleteKey("Highscore_" + i + "_Player2Name");
            PlayerPrefs.DeleteKey("Highscore_" + i + "_Score");
        }
        PlayerPrefs.SetInt("Highscore_Count", 0);
        for (int i = 0; i < input.Count; i++)
        {
            PlayerPrefs.SetString("Highscore_" + i + "_Player1Name", input[i].GetPlayer1Name());
            PlayerPrefs.SetString("Highscore_" + i + "_Player2Name", input[i].GetPlayer2Name());
            PlayerPrefs.SetInt("Highscore_" + i + "_Player1Score", input[i].GetPlayer1Score());
            PlayerPrefs.SetInt("Highscore_" + i + "_Player2Score", input[i].GetPlayer2Score());
            PlayerPrefs.SetInt("Highscore_Count", input.Count);
        }
    }

    public List<Highscore> ReadHighscoresFromPlayerPrefs()
    {
        int highscoreCount = PlayerPrefs.GetInt("Highscore_Count");
        List<Highscore> aux = new List<Highscore>();
        for (int i = 0; i < highscoreCount; i++)
        {
            aux.Add(new Highscore(PlayerPrefs.GetString("Highscore_" + i + "_Player1Name"), PlayerPrefs.GetString("Highscore_" + i + "_Player2Name"), PlayerPrefs.GetInt("Highscore_" + i + "_Player1Score"), PlayerPrefs.GetInt("Highscore_" + i + "_Player2Score")));
        }
        return aux;
    }

    public string ListHighscoreFromPlayerPrefs(int size, bool isDebug)
    {
        string output;

        if (isDebug == true)
        {
            output = "---Highscores---\n";
        }
        else
        {
            output = "PLAYERS\t\t\tSCORE\n";
        }

        for (int i = 0; i < size; i++)
        {
            output = output + PlayerPrefs.GetString("Highscore_" + i + "_Player1Name") + " | ";
            output = output + PlayerPrefs.GetString("Highscore_" + i + "_Player2Name") + " | ";
            output = output + string.Format("{0:D2} | {1:D2}\n", PlayerPrefs.GetInt("Highscore_" + i + "_Player1Score"), PlayerPrefs.GetInt("Highscore_" + i + "_Player2Score"));
        }
        return output;
    }
}
