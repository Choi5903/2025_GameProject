using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIWindowController : MonoBehaviour
{
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        ResetToCenter();
        // ✅ 창을 가장 앞으로 가져오기
        transform.SetAsLastSibling();
    }

    public void ResetToCenter()
    {
        rectTransform.anchoredPosition = Vector2.zero;
    }

    // ✅ 버튼에서 이 메서드를 호출하여 창 상태 전환
    public void ToggleWindow()
    {
        SFXManager.Instance.PlayButtonClick1();

        bool isCurrentlyActive = gameObject.activeSelf;
        bool willBeActive = !isCurrentlyActive;

        gameObject.SetActive(willBeActive);

        // ✅ 상태 전환 후 효과음 재생
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
