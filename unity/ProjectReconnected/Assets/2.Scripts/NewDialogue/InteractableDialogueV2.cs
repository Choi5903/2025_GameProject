using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDialogueV2 : MonoBehaviour, IInteractable
{
    [Header("대화 데이터 (Bottom / Messenger / SpeechBubble 가능)")]
    public ScriptableObject dialogueData;

    [Header("UI")]
    public GameObject interactionPrompt;

    [Header("설정")]
    public bool isRepeatable = false;

    [Header("대화 후 작동할 상호작용 오브젝트")]
    public InteractableObject objectToTrigger;

    [Header("대화 후 실행할 이벤트 (오브젝트 기반)")]
    public GameObject postDialogueEventObject;

    [Header("대화 후 실행할 이벤트 (스크립트 직접 지정)")]
    public MonoBehaviour postDialogueEventScript; // IBeginEvent 인터페이스 구현 스크립트

    private bool hasInteracted = false;

    private void Awake()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    public void Interact()
    {
        if (!isRepeatable && hasInteracted) return;
        if (DialogueManagerV2.Instance == null || dialogueData == null)
        {
            Debug.LogWarning($"❌ {gameObject.name}에 필요한 정보가 없습니다.");
            return;
        }

        hasInteracted = true;

        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null) player.SetMovementEnabled(false);

        DialogueManagerV2.Instance.StartDialogue(dialogueData, () =>
        {
            // 🎯 오브젝트 상호작용 실행
            objectToTrigger?.Interact();

            // 🎯 이벤트 오브젝트 실행
            if (postDialogueEventObject != null &&
                postDialogueEventObject.TryGetComponent(out IBeginEvent evtObj))
            {
                evtObj.TriggerEvent();
            }

            // 🎯 스크립트 직접 실행
            if (postDialogueEventScript != null && postDialogueEventScript is IBeginEvent evtScript)
            {
                evtScript.TriggerEvent();
            }

            // 🎯 이동 허용
            if (player != null) player.SetMovementEnabled(true);
        });
    }

    public void ShowInteractionUI(bool show)
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(show);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isRepeatable)
            hasInteracted = false;
    }
}
