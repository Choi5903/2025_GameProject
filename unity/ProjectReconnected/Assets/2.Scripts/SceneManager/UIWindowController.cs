using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIWindowController : MonoBehaviour
{
    private RectTransform rectTransform;

    [Header("초기 위치 설정 (패널마다 개별 지정)")]
    public Vector2 initialAnchoredPosition = Vector2.zero;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (initialAnchoredPosition == Vector2.zero)
            initialAnchoredPosition = rectTransform.anchoredPosition;
    }


    void OnEnable()
    {
        ResetToInitialPosition();
        transform.SetAsLastSibling();
    }

    public void ResetToInitialPosition()
    {
        rectTransform.anchoredPosition = initialAnchoredPosition;
    }

    public void ToggleWindow()
    {
        SFXManager.Instance.PlayButtonClick1();

        bool isCurrentlyActive = gameObject.activeSelf;
        bool willBeActive = !isCurrentlyActive;

        gameObject.SetActive(willBeActive);

        if (willBeActive)
        {
            SFXManager.Instance.PlayWindowOpen();
        }
        else
        {
            SFXManager.Instance.PlayWindowClose();
        }
    }
}
