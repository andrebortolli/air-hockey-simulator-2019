using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreController : MonoBehaviour
{
    public string databaseBaseURL = "localhost";
    public int databaseBaseURLPort = 3000;
    private string databaseURL;

    public event Action<HighscoreList> DownloadedHighscoreList;
    public event Action<string> DownloadedHighscoreListToString;

    public void Awake()
    {
        databaseURL = string.Format("{0}:{1}", databaseBaseURL, databaseBaseURLPort);
    }

    public IEnumerator StartHighscoreDownload(bool limit, int limitSize = 100, bool ToString = true)
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
        yield return connection;
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

        WWWForm form = new WWWForm();
        form.AddField("player1Name", input.player1Name);
        form.AddField("player2Name", input.player2Name);
        form.AddField("player1Score", input.player1Score);
        form.AddField("player2Score", input.player2Score);
        WWW connection = new WWW(databaseURL + "/highscore/insertHighscore", form);
        yield return connection;
    }
}
