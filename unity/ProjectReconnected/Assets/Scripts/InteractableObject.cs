using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("상호작용 설정")]
    public bool deactivateSelf = false;
    public List<GameObject> objectsToActivate;
    public List<GameObject> objectsToDeactivate;
    public List<Transform> objectsToMove;
    public List<Vector3> newPositions;

    public float restorationChange = 0f;
    public int clueChange = 0;
    public GameObject imageToShow;

    public GameObject interactionPrompt;

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
        // 복원율 및 단서 변경
        if (restorationChange != 0)
            GameManager.Instance.ChangeRestoration(restorationChange);
        if (clueChange != 0)
            GameManager.Instance.ChangeMemoryClues(clueChange);

        // 이미지 출력
        if (imageToShow != null)
            imageToShow.SetActive(true);

        // 오브젝트 활성화/비활성화
        foreach (GameObject obj in objectsToActivate)
            if (obj != null) obj.SetActive(true);

        foreach (GameObject obj in objectsToDeactivate)
            if (obj != null) obj.SetActive(false); // 여기서 작동 안 했던 문제 해결

        // 오브젝트 위치 이동
        for (int i = 0; i < Mathf.Min(objectsToMove.Count, newPositions.Count); i++)
        {
            if (objectsToMove[i] != null)
                objectsToMove[i].position = newPositions[i];
        }

        yield return null;

        if (deactivateSelf)
        {
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public void ShowInteractionUI(bool show)
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(show);
    }
}
