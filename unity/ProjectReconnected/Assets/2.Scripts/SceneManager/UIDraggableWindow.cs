using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDraggableWindow : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public RectTransform dragHandle;  // 창 상단바 (드래그 가능한 영역)
    private RectTransform windowRect;
    private Vector2 pointerOffset;

    private bool isDragging = false;

    void Awake()
    {
        windowRect = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // ✅ 드래그 핸들에서만 반응하도록 조건 추가
        if (RectTransformUtility.RectangleContainsScreenPoint(dragHandle, eventData.position, eventData.pressEventCamera))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                windowRect, eventData.position, eventData.pressEventCamera, out pointerOffset);

            isDragging = true;
            windowRect.SetAsLastSibling();
        }
        else
        {
            isDragging = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 localPointerPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            windowRect.parent as RectTransform, eventData.position, eventData.pressEventCamera, out localPointerPos))
        {
            windowRect.anchoredPosition = localPointerPos - pointerOffset;
        }
    }
}
