using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;

public class HighscoreController : MonoBehaviour
{
    private string source;
    private string server = "sql10.freemysqlhosting.net";
    private string database = "sql10262757";
    private string userId = "sql10262757";
    private bool pooling = false;
    private string password = "";
    private MySqlConnection connection;

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

    public void Awake()
    {
        try
        {
            StreamReader streamReader = new StreamReader("Assets\\Passwords\\highscore_db.passw");
            password = streamReader.ReadLine();
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
        this.source = string.Format("Server={0};Database={1};User ID={2};Pooling={3};Password={4}", server, database, userId, pooling, password);
    }

    public void Start()
    {
        //highscoreList = new List<Highscore>(ReadHighscoreFromDatabase());
    }

    private void DBConnect(string source)
    {
        this.connection = new MySqlConnection(source);
        try
        {
            this.connection.Open();
            Debug.Log(string.Format("Connected to Database: {0}", connection.Site));
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void DBDisconnect()
    {
        try
        {
            this.connection.Close();
            Debug.Log(string.Format("Disconnected from Database: {0}", connection.Site));
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }

    public List<Highscore> ReadHighscoreFromDatabase()
    {
        try
        {
            DBConnect(source);
            MySqlCommand query = this.connection.CreateCommand();
            query.CommandText = "SELECT * FROM sql10262757.Highscore";
            MySqlDataReader data = query.ExecuteReader();
            List<Highscore> aux = new List<Highscore>();
            while (data.Read())
            {
                aux.Add(new Highscore((string)data["player1Name"], (string)data["player2Name"], (int)data["player1Score"], (int)data["player2Score"]));
            }
            DBDisconnect();
            return aux;
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            return null;
        }
    }

    public string ListHighscoreFromDatabase(int size)
    {
        try
        {
            DBConnect(source);
            MySqlCommand query = this.connection.CreateCommand();
            query.CommandText = string.Format("SELECT * FROM {0} LIMIT 0, {1}", "sql10262757.Highscore", size);
            MySqlDataReader data = query.ExecuteReader();
            string output = "PLAYERS\t\t\tSCORES\n";
            while (data.Read())
            {
                output = output + (string)data["player1Name"] + " | ";
                output = output + (string)data["player2Name"] + " | ";
                output = output + string.Format("{0:D2} | {1:D2}\n", (int)data["player1Score"], (int)data["player2Score"]);
            }
            DBDisconnect();
            return output;
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            return null;
        }
    }

    public void SaveHighscoreInDatabase(Highscore input)
    {
        DBConnect(source);
        try
        {
            MySqlCommand query = this.connection.CreateCommand();
            query.CommandText = string.Format("INSERT INTO {0}(player1Name, player2Name, player1Score, player2Score) values ('{1}','{2}',{3},{4})", "sql10262757.Highscore", input.GetPlayer1Name(), input.GetPlayer2Name(), input.GetPlayer1Score(), input.GetPlayer2Score());
            query.ExecuteNonQuery();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
        DBDisconnect();
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
