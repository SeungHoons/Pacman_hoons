using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSaveData;

namespace GameSaveDataIO
{
    [CreateAssetMenu(fileName = "StageDataSO"
                     , menuName = "StageGameData"
                     , order = 3)]
    [System.Serializable]
    public class StageDataScriptableObject : ScriptableObject
    {
        public int highScore;
        public List<StageData> MainStageData;
    }
}