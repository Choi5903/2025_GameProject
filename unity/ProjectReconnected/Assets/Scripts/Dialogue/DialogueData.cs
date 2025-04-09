using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    public List<DialogueLine> dialogueLines;
}