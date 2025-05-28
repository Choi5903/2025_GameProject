using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BottomDialogueManager))]
public class BottomDialogueTrigger : MonoBehaviour, IBeginEvent
{
    [Header("시작 대사 데이터")]
    public BottomDialogueData dialogueData;

    private BottomDialogueManager dialogueManager;

    private void Awake()
    {
        dialogueManager = GetComponent<BottomDialogueManager>();
    }

    public void TriggerEvent(System.Action onComplete = null)
    {
        if (dialogueData == null || dialogueManager == null)
        {
            Debug.LogWarning("❗ BottomDialogueTrigger: 대사 데이터 또는 매니저가 없습니다.");
            onComplete?.Invoke();
            return;
        }

        dialogueManager.StartDialogue(dialogueData, onComplete);
    }
}
