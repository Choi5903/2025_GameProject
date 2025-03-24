using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractableObject : MonoBehaviour, IInteractable
{
    [TextArea]
    public string interactionDescription = "기본 상호작용 설명";

    public void Interact()
    {
        Debug.Log($"🔵 {gameObject.name}와 상호작용 실행: {interactionDescription}");
        // 필요한 실제 동작은 자식 클래스 또는 UnityEvent에서 오버라이드 가능
    }

    public void ShowInteractionUI(bool show)
    {
        if (InteractionUI.Instance != null)
        {
            InteractionUI.Instance.SetVisible(show);
        }
    }
}
