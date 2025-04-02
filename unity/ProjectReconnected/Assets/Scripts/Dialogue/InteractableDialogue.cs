using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDialogue : MonoBehaviour, IInteractable
{
    [Header("대화 데이터")]
    public DialogueData dialogueData;

    [Header("UI")]
    public GameObject interactionPrompt;

    [Header("대화 후 작동할 상호작용 오브젝트")]
    public InteractableObject objectToTrigger;

    private bool hasInteracted = false;

    private void Awake()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    public void Interact()
    {
        if (hasInteracted || DialogueManager.Instance.IsDialogueActive) return;

        if (dialogueData == null)
        {
            Debug.LogWarning($"❌ {gameObject.name}의 DialogueData가 연결되어 있지 않습니다!");
            return;
        }

        hasInteracted = true;

        // 플레이어 이동 잠금
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
            player.SetMovementEnabled(false);

        // 대화 시작
        DialogueManager.Instance.StartDialogue(dialogueData, () =>
        {
            // 대화 종료 후 실행할 내용
            if (objectToTrigger != null)
                objectToTrigger.Interact();

            // 플레이어 이동 다시 허용
            if (player != null)
                player.SetMovementEnabled(true);
        });
    }

    public void ShowInteractionUI(bool show)
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(show);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hasInteracted = false;
        }
    }
}
