using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpeechBubbleLine
{
    [Header("대사 텍스트")]
    [TextArea(2, 3)]
    public string sentence;
}
