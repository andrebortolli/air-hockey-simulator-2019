using UnityEngine;

[System.Serializable]
public class Highscore
{
    public int idHighscore, player1Score, player2Score;
    public string player1Name, player2Name;

    public static Highscore CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Highscore>(jsonString);
    }
    public Highscore (string player1Name, string player2Name, int player1Score, int player2Score)
    {
        this.player1Name = player1Name;
        this.player2Name = player2Name;
        this.player1Score = player1Score;
        this.player2Score = player2Score;
    }
}