using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDraggableWindow : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public RectTransform dragHandle;  // 창 상단바
    private RectTransform windowRect;
    private Vector2 pointerOffset;

    void Awake()
    {
        windowRect = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            windowRect, eventData.position, eventData.pressEventCamera, out pointerOffset);
        // ✅ 창을 가장 앞으로 가져오기
        windowRect.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPointerPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            windowRect.parent as RectTransform, eventData.position, eventData.pressEventCamera, out localPointerPos))
        {
            windowRect.anchoredPosition = localPointerPos - pointerOffset;
        }
    }
}
