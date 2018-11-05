using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighscoreList
{
    public List<Highscore> data;

    public static HighscoreList CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<HighscoreList>(jsonString);
    }

    public string HighscoreListToString()
    {
        string output = "";
        foreach (Highscore h in data)
        {
            output += string.Format("P1: {0} | P2: {1} | SP1: {2} | SP2: {3}\n", h.player1Name, h.player2Name, h.player1Score, h.player2Score);
        }
        Debug.Log(output);
        return output;
    }
}
