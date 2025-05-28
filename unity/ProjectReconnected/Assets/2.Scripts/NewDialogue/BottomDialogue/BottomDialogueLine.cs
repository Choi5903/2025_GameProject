using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BottomDialogueLine
{
    [Header("화자 이름 (Optional)")]
    public string speakerName;

    [Header("대사 텍스트")]
    [TextArea(2, 5)]
    public string sentence;

    [Header("배경 이미지 (Optional)")]
    public Sprite backgroundImage;
}
