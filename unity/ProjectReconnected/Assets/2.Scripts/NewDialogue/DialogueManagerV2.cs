using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManagerV2 : MonoBehaviour
{
    public static DialogueManagerV2 Instance;

    private System.Action onDialogueEnd;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartDialogue(ScriptableObject dialogueData, System.Action endCallback = null)
    {
        onDialogueEnd = endCallback;

        if (dialogueData is BottomDialogueData bottomData)
        {
            BottomDialogueManager.Instance.StartDialogue(bottomData, EndDialogue);
        }
        else if (dialogueData is MessengerDialogueData messengerData)
        {
            MessengerDialogueManager.Instance.StartDialogue(messengerData, EndDialogue);
        }
        else if (dialogueData is SpeechBubbleData speechData)
        {
            Debug.LogWarning("⚠ SpeechBubble은 targetTransform이 필요하므로 DialogueTrigger 또는 외부에서 직접 호출해야 합니다.");
            EndDialogue();
        }
        else
        {
            Debug.LogWarning("⚠️ 지원되지 않는 DialogueData 타입입니다!");
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        onDialogueEnd?.Invoke();
        onDialogueEnd = null;
    }
}
