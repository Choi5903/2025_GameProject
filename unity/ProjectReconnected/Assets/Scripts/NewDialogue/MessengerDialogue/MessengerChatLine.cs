using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChatPosition
{
    Left,   // 상대방 대사
    Right   // 플레이어 대사
}

[System.Serializable]
public class MessengerChatLine
{
    [Header("채팅 텍스트")]
    [TextArea(2, 4)]
    public string message;

    [Header("출력 위치 (좌/우)")]
    public ChatPosition position;

    [Header("첨부 이미지 (Optional)")]
    public Sprite attachmentImage;
}
