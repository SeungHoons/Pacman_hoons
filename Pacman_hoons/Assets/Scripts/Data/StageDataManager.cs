using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSaveData;
using GameSaveDataIO;

public class StageDataManager : Singletone<StageDataManager>
{
    public StageDataScriptableObject stageDataSO;

    public int hightScore;
    // Start is called before the first frame update
    void Start()
    {
        if (stageDataSO == null)
            stageDataSO = (StageDataScriptableObject)Resources.Load("Data/StageDataSO");
    }

    public StageData GetStageData(int _stage)
    {
        return stageDataSO.MainStageData[(_stage -1)];
    }
}
