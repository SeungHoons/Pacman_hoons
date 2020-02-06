using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public MapFileInfo mapFileInfo;

    [ContextMenu("Save To Json")]
    void SaveToJsonFile()
    {
        string jsonData = JsonUtility.ToJson(mapFileInfo, true);
        string path = Application.dataPath + "/MapInfoData.json";
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("Load From Json")]
    void LoadFromJsonFile()
    {
        string path = Application.dataPath + "/MapInfoData.json";
        string jsonData = null;
        if ((jsonData = File.ReadAllText(path)) != null)
        {
            mapFileInfo = JsonUtility.FromJson<MapFileInfo>(jsonData);
        }
    }


}

[System.Serializable]
public class MapFileInfo
{
    public int currentMapNum;
    public string[] mapName;
}

[System.Serializable]
public class MapStage
{
    
}