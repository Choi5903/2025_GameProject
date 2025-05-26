using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("대화 데이터 (ScriptableObject)")]
    public ScriptableObject dialogueData;

    [Header("대화 종료 후 호출할 오브젝트")]
    public GameObject eventReceiver;

    public void TriggerDialogue()
    {
        if (dialogueData == null)
        {
            Debug.LogWarning("⚠️ Dialogue Data가 비어 있습니다!");
            return;
        }

        DialogueManagerV2.Instance.StartDialogue(dialogueData, OnDialogueEnd);
    }

    private void OnDialogueEnd()
    {
        if (eventReceiver != null)
        {
            var handler = eventReceiver.GetComponent<IDialogueEventHandler>();
            if (handler != null)
            {
                handler.OnDialogueEvent();
            }
        }
    }
}
