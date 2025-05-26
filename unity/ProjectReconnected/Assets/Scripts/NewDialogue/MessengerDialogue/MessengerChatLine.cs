using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChatPosition { Left, Right }

[System.Serializable]
public class MessengerChatLine
{
    [TextArea(2, 4)]
    public string message;
    public ChatPosition position;

    [Range(1, 3)]
    public int sizeLevel = 2; // 1=소형, 2=중형, 3=대형
}
