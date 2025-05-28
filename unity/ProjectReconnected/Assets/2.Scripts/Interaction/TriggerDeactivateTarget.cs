using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TriggerDeactivateTarget : MonoBehaviour, IInteractable
{
    [Header("삭제할 대상 오브젝트")]
    public GameObject target;

    [Header("상호작용 UI")]
    public GameObject interactionPrompt;

    private void Awake()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    public void Interact()
    {
        if (target != null)
        {
            Destroy(target);  // ✅ 씬에서 완전 삭제
            Debug.Log($"🗑️ {target.name} → Destroy로 완전 제거됨");
        }

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    public void ShowInteractionUI(bool show)
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(show);
    }
}