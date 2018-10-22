using System.Collections;
using System.Collections.Generic;

public class Highscore
{
    
    string player1Name, player2Name;
    int player1Score, player2Score;

    public Highscore()
    {
        player1Name = "";
        player2Name = "";
        player1Score = 0;
        player2Score = 0;
    }

    public Highscore(string player1Name, string player2Name, int player1Score, int player2Score)
    {
        this.player1Name = player1Name;
        this.player2Name = player2Name;
        this.player1Score = player1Score;
        this.player2Score = player2Score;
    }

    public string GetPlayer1Name()
    {
        return player1Name;
    }

    public string GetPlayer2Name()
    {
        return player2Name;
    }

    public int GetPlayer1Score()
    {
        return player1Score;
    }

    public int GetPlayer2Score()
    {
        return player2Score;
    }
}
