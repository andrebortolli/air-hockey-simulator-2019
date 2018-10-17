using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptTools.ObjectState
{

    public class SaveGameObjectState
    {
        private List<string> savedGameObjectsJson;

        public SaveGameObjectState()
        {
            savedGameObjectsJson = new List<string>();
        }
        public void Save(GameObject gameObjectToSave)
        {
            savedGameObjectsJson.Add(JsonUtility.ToJson(gameObjectToSave));
        }
        public GameObject Restore(int gameObjectListIndex)
        {
            GameObject gameObjectToReturn;
            gameObjectToReturn = JsonUtility.FromJson<GameObject>(savedGameObjectsJson[gameObjectListIndex]);
            return gameObjectToReturn;
        }
        public string SavedGameObjectsToString()
        {
            string stringToReturn = "";
            GameObject gameObjectFromJson;
            for (int i = 0; i < savedGameObjectsJson.Count; i++)
            {
                gameObjectFromJson = JsonUtility.FromJson<GameObject>(savedGameObjectsJson[i]);
                stringToReturn = string.Format("{1}\n{2} | {3} | {4}", stringToReturn, i, gameObjectFromJson.name, gameObjectFromJson.GetInstanceID());
            }
            return stringToReturn;
        }
        public string GetGameObjectAsJsonFromList(int gameObjectListIndex)
        {
            return savedGameObjectsJson[gameObjectListIndex];
        }
    }
}