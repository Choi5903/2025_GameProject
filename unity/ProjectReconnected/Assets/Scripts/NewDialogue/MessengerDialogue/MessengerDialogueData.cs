using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewMessengerDialogueData", menuName = "Dialogue/Messenger Dialogue Data")]
public class MessengerDialogueData : ScriptableObject
{
    public List<MessengerChatLine> chatLines;
}
