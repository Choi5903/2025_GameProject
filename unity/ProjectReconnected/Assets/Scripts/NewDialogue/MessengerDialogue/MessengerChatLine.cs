using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChatPosition
{
    Left,   // ���� ���
    Right   // �÷��̾� ���
}

[System.Serializable]
public class MessengerChatLine
{
    [Header("ä�� �ؽ�Ʈ")]
    [TextArea(2, 4)]
    public string message;

    [Header("��� ��ġ (��/��)")]
    public ChatPosition position;

    [Header("÷�� �̹��� (Optional)")]
    public Sprite attachmentImage;
}
