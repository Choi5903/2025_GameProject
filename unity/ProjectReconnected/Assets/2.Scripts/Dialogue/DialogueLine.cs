using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [Header("기본 정보")]
    public string speakerName;
    [TextArea(2, 4)]
    public string sentence;

    [Header("이미지")]
    public Sprite characterCGLeft;    // 왼쪽 캐릭터
    public Sprite characterCGRight;   // 오른쪽 캐릭터
    public Sprite extraImage;         // 연출 이미지 등
}
