using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewBottomDialogueData", menuName = "Dialogue/Bottom Dialogue Data")]
public class BottomDialogueData : ScriptableObject
{
    public List<BottomDialogueLine> dialogueLines;
}
