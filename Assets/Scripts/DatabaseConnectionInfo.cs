using UnityEngine;

[System.Serializable]
public class DatabaseConnectionInfo
{
    public string connectionURL = "";
    public int connectionPort = 0;

    public static DatabaseConnectionInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<DatabaseConnectionInfo>(jsonString);
    }
    public string ToJSON()
    {
        return JsonUtility.ToJson(this);
    }
}
