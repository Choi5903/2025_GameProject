using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BottomDialogueLine
{
    [Header("ȭ�� �̸� (Optional)")]
    public string speakerName;

    [Header("��� �ؽ�Ʈ")]
    [TextArea(2, 5)]
    public string sentence;

    [Header("��� �̹��� (Optional)")]
    public Sprite backgroundImage;
}
