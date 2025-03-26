using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractableDialogue : MonoBehaviour, IInteractable
{
    [Header("대화 데이터")]
    public DialogueData dialogueData;

    public GameObject interactionPrompt;

    private bool hasInteracted = false;

    private void Awake()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    public void Interact()
    {
        if (hasInteracted || DialogueManager.Instance.IsDialogueActive()) return;

        if (dialogueData == null)
        {
            Debug.LogWarning($"❌ {gameObject.name}의 DialogueData가 연결되어 있지 않습니다!");
            return;
        }

        DialogueManager.Instance.StartDialogue(dialogueData);
        hasInteracted = true;
    }

    public void ResetInteraction()
    {
        hasInteracted = false;
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
            ResetInteraction(); // 충돌 해제 시 상호작용 플래그 리셋
        }
    }
}
