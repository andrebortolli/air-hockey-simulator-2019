using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;

public class HighscoreController : MonoBehaviour
{
    public string databaseBaseURL = "localhost";
    public int databaseBaseURLPort = 3000;
    private string databaseURL;

    public void Awake()
    {
        databaseURL = string.Format("{0}:{1}", databaseBaseURL, databaseBaseURLPort);
    }

    public List<Highscore> GetHighscoresFromDatabase(bool limit, int limitSize = 100)
    {
        try
        {
            WWW connection;
            if (limit)
            {
                connection = new WWW(databaseURL + "/highscore/listHighscoreLimit?limit=" + limitSize);
            }
            else
            {
                connection = new WWW(databaseURL + "/highscore/listHighscores");
            }
            while (connection.isDone == false)
            {
                Debug.Log("Waiting for the connection to the Database to finish.");
            }
            string connectionText = connection.text;
            connection.Dispose();
            //Debug.Log(connectionText);
            HighscoreList highscoreListDB = HighscoreList.CreateFromJSON(connectionText);
            //Debug.Log(highscoreListDB.HighscoreListToString());
            return highscoreListDB.data;
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            return null;
        }
    }
    public string ListHighscoreFromDatabase(int size)
    {
        List<Highscore> list = GetHighscoresFromDatabase(true, size);
        string output = "";
        foreach (Highscore highscore in list)
        {
            output = output + highscore.player1Name + " | ";
            output = output + highscore.player2Name + " | ";
            output = output + string.Format("{0:D2} | {1:D2}\n", highscore.player1Score, highscore.player2Score);
        }
        return output;
    }

    public void SaveHighscoreInDatabase(Highscore input)
    {
        try
        {
            WWWForm form = new WWWForm();
            form.AddField("player1Name", input.player1Name);
            form.AddField("player2Name", input.player2Name);
            form.AddField("player1Score", input.player1Score);
            form.AddField("player2Score", input.player2Score);
            WWW connection = new WWW(databaseURL + "/highscore/insertHighscore", form);
            while (connection.isDone == false)
            {
                Debug.Log("Waiting for Database connection to finish.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
}
