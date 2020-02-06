using System;
using System.Collections;
using System.Collections.Generic;
using GameSaveDataIO;
using UnityEngine;



namespace GameSaveData
{
    [System.Serializable]
    public class AllStageData
    {
        public int highScore;
        public List<StageData> MainStageData = new List<StageData>();
    }
    
    [System.Serializable]
    public class StageData
    {
        public int scatterModeTime1 ;
        public int scatterModeTime2 ;
        public int scatterModeTime3 ;

        public int chaseModeTime1 ;
        public int chaseModeTime2 ;

        public int frightenedModeTimer ;
        public int startBlinkingTime ;

        public int pinkyReleaseTimer;
        public int inkyReleaseTimer ;
        public int clydeReleaseTimer;
        public int blinkyReleaseTimer;

        public float moveSpeed ;
        public float normalMoveSpeed ;
        public float frightenedMoveSpeed ;
        public float eatenMoveSpeed;
    }
}
