using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ������ ���� ������ ����ü
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

    // �� �����ڵ� ���� (EasySave ��� �ʿ��� �� ����)
    public GameData()
    {
        restorationRate = 0f;
        memoryClues = 0;
        currentTimeState = TimeState.Present;
    }
}
