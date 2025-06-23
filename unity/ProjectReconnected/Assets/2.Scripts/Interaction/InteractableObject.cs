using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("상호작용 설정")]
    public bool deactivateSelf = false;
    public List<GameObject> objectsToActivate;
    public List<GameObject> objectsToDestroy;
    public List<Transform> objectsToMove;
    public List<Vector3> newPositions;

    public float restorationChange = 0f;
    public int clueChange = 0;
    public GameObject imageToShow;

    public GameObject interactionPrompt;

    [Header("상호작용 후 실행할 이벤트 (오브젝트 기반)")]
    public GameObject postInteractionEventObject;

    [Header("상호작용 후 실행할 이벤트 (스크립트 직접 지정)")]
    public MonoBehaviour postInteractionEventScript; // IBeginEvent 인터페이스 구현 스크립트

    private void Awake()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    public void Interact()
    {
        StartCoroutine(ExecuteInteraction());
    }

    private IEnumerator ExecuteInteraction()
    {
        if (restorationChange != 0)
        {
            SFXManager.Instance?.PlayClueSound();
            GameManager.Instance?.ChangeRestoration(restorationChange);
        }

        if (clueChange != 0)
        {
            SFXManager.Instance?.PlayClueSound();
            GameManager.Instance?.ChangeMemoryClues(clueChange);
        }

        if (imageToShow != null)
            imageToShow.SetActive(true);

        if (objectsToActivate != null)
        {
            foreach (GameObject obj in objectsToActivate)
                if (obj != null) obj.SetActive(true);
        }

        if (objectsToDestroy != null)
        {
            foreach (GameObject obj in objectsToDestroy)
                if (obj != null) Destroy(obj);
        }

        if (objectsToMove != null && newPositions != null)
        {
            for (int i = 0; i < Mathf.Min(objectsToMove.Count, newPositions.Count); i++)
            {
                if (objectsToMove[i] != null)
                    objectsToMove[i].position = newPositions[i];
            }
        }

        // 🎯 이벤트 오브젝트 실행
        if (postInteractionEventObject != null &&
            postInteractionEventObject.TryGetComponent(out IBeginEvent evtObj))
        {
            evtObj.TriggerEvent();
        }

        // 🎯 스크립트 직접 실행
        if (postInteractionEventScript != null && postInteractionEventScript is IBeginEvent evtScript)
        {
            evtScript.TriggerEvent();
        }

        yield return null;

        if (deactivateSelf)
        {
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);

            Destroy(gameObject);
        }
    }

    public void ShowInteractionUI(bool show)
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(show);
    }
}
