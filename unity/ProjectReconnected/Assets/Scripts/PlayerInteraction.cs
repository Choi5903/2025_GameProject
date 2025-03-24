
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteraction : MonoBehaviour
{
    private IInteractable currentTarget;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentTarget != null)
        {
            currentTarget.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            currentTarget = interactable;
            interactable.ShowInteractionUI(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable) && currentTarget == interactable)
        {
            interactable.ShowInteractionUI(false);
            currentTarget = null;
        }
    }
}
