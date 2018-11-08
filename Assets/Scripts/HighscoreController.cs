using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreController : MonoBehaviour
{
    private string databaseBaseURL = "";
    private int databaseBaseURLPort = 0;
    private string databaseURL;
    private bool isGetDatabaseConnectionInfoRunning;


    public event Action<HighscoreList> DownloadedHighscoreList;
    public event Action<string> DownloadedHighscoreListToString;

    public void Awake()
    {
        StartCoroutine(GetDatabaseConnectionInfo());
    }

    IEnumerator GetDatabaseConnectionInfo()
    {
        isGetDatabaseConnectionInfoRunning = true;
        databaseBaseURL = "http://127.0.0.1";
        databaseBaseURLPort = 3000;
        WWW connection = new WWW(Application.absoluteURL + "/database/getConnectionInfo");
        yield return connection;
        if (connection.error == null)
        {
            DatabaseConnectionInfo dbConnectionInfo = DatabaseConnectionInfo.CreateFromJSON(connection.text);
            Debug.Log(string.Format("DB Connection Info: {0}:{1}", dbConnectionInfo.connectionURL, dbConnectionInfo.connectionPort));
            databaseBaseURL = dbConnectionInfo.connectionURL;
            databaseBaseURLPort = dbConnectionInfo.connectionPort;
        }
        else
        {
            Debug.LogError("Error: " + connection.error + "\n Could not get Database Connection Information. Using localhost:3000.");
        }
        databaseURL = string.Format("{0}:{1}", databaseBaseURL, databaseBaseURLPort);
        isGetDatabaseConnectionInfoRunning = false;
        Debug.LogWarning("Using the following Database Address: " + databaseURL);
        yield return null;
    }

    public IEnumerator StartHighscoreDownload(bool limit, int limitSize = 100, bool ToString = true)
    {
        yield return isGetDatabaseConnectionInfoRunning;
        WWW connection;
        if (limit)
        {
            connection = new WWW(databaseURL + "/highscore/listHighscoreLimit?limit=" + limitSize);
        }
        else
        {
            connection = new WWW(databaseURL + "/highscore/listHighscores");
        }
        yield return connection;
        if (connection.error == null)
        {
            Debug.Log("Highscore successfully downloaded!");
            HighscoreList highscoreListDB = HighscoreList.CreateFromJSON(connection.text);
            if (ToString)
            {
                ListHighscoreFromHighscoreList(highscoreListDB, limitSize);
            }
            else
            {
                if (DownloadedHighscoreList != null)
                {
                    DownloadedHighscoreList(highscoreListDB);
                }
            }
        }
        else
        {
            Debug.LogError("Error: " + connection.error);
        }  
        yield return null;
    }

    public string ListHighscoreFromHighscoreList(HighscoreList list, int size)
    {
        string output = "";
        foreach (Highscore highscore in list.data)
        {
            output = output + highscore.player1Name + " | ";
            output = output + highscore.player2Name + " | ";
            output = output + string.Format("{0:D2} | {1:D2}\n", highscore.player1Score, highscore.player2Score);
        }
        if (DownloadedHighscoreListToString != null)
        {
            DownloadedHighscoreListToString(output);
        }
        return output;
    }

    public IEnumerator SaveHighscoreInDatabase(Highscore input)
    {
        yield return isGetDatabaseConnectionInfoRunning;
        WWWForm form = new WWWForm();
        form.AddField("player1Name", input.player1Name);
        form.AddField("player2Name", input.player2Name);
        form.AddField("player1Score", input.player1Score);
        form.AddField("player2Score", input.player2Score);
        WWW connection = new WWW(databaseURL + "/highscore/insertHighscore", form);
        yield return connection;
        if (connection.error == null)
        {
            Debug.Log("Highscore successfully inserted!");
        }
        else
        {
            Debug.LogError("Error: " + connection.error);
        }
    }
}
