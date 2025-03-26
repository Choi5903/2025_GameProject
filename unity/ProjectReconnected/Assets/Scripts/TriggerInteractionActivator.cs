
using UnityEngine;

public class TriggerInteractionActivator : MonoBehaviour
{
    [Header("자동 실행할 상호작용 오브젝트")]
    public GameObject interactableTarget;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (interactableTarget == null) return;

        // Rigidbody2D를 가진 객체가 올라왔을 때만 동작
        if (other.attachedRigidbody != null)
        {
            IInteractable interactable = interactableTarget.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
                Debug.Log($"🔘 버튼 트리거 작동: {interactableTarget.name} 상호작용 실행됨");
            }
        }
    }
}
