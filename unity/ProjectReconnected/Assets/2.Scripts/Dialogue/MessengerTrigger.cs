using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessengerTrigger : MonoBehaviour
{
    [Header("연결된 대화 데이터")]
    public MessengerDialogueData dialogueData;

    public void StartMessengerDialogue()
    {
        if (dialogueData != null)
        {
            MessengerDialogueManager.Instance.StartDialogue(dialogueData);
        }
        else
        {
            Debug.LogWarning("💬 대화 데이터가 비어 있습니다.");
        }
    }
}
