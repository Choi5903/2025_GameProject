using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 저장 가능한 게임 데이터 구조체
[System.Serializable]
public class GameData
{
    public float restorationRate;
    public int memoryClues;
    public TimeState currentTimeState;

    public GameData(float rate, int clues, TimeState timeState)
    {
        restorationRate = rate;
        memoryClues = clues;
        currentTimeState = timeState;
    }

    // 빈 생성자도 제공 (EasySave 등에서 필요할 수 있음)
    public GameData()
    {
        restorationRate = 0f;
        memoryClues = 0;
        currentTimeState = TimeState.Present;
    }
}
