using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SpeechBubbleManager : MonoBehaviour
{
    public static SpeechBubbleManager Instance;

    [Header("말풍선 프리팹")]
    public GameObject speechBubblePrefab;

    [Header("UI 설정")]
    public Canvas worldCanvas; // World Space용 Canvas
    public Vector2 bubbleOffset = new Vector2(0, 1.5f);

    [Header("출력 설정")]
    public float sentenceDisplayTime = 2.5f;

    private System.Action onDialogueEnd;
    private Queue<SpeechBubbleLine> bubbleLines = new Queue<SpeechBubbleLine>();
    private Transform targetTransform;
    private GameObject activeBubble;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartDialogue(SpeechBubbleData data, Transform target, System.Action endCallback = null)
    {
        if (data == null || data.bubbleLines.Count == 0)
        {
            Debug.LogWarning("⚠️ 말풍선 대사 데이터가 비어 있습니다!");
            endCallback?.Invoke();
            return;
        }

        targetTransform = target;
        onDialogueEnd = endCallback;

        bubbleLines.Clear();
        foreach (var line in data.bubbleLines)
        {
            bubbleLines.Enqueue(line);
        }

        StartCoroutine(PlaySpeechBubbles());
    }

    private IEnumerator PlaySpeechBubbles()
    {
        while (bubbleLines.Count > 0)
        {
            var line = bubbleLines.Dequeue();
            ShowBubble(line.sentence);

            yield return new WaitForSeconds(sentenceDisplayTime);

            HideBubble();
        }

        onDialogueEnd?.Invoke();
        onDialogueEnd = null;
    }

    private void ShowBubble(string sentence)
    {
        if (activeBubble != null) Destroy(activeBubble);

        activeBubble = Instantiate(speechBubblePrefab, worldCanvas.transform);
        TMP_Text text = activeBubble.GetComponentInChildren<TMP_Text>();
        text.text = sentence;

        UpdateBubblePosition();
    }

    private void HideBubble()
    {
        if (activeBubble != null)
        {
            Destroy(activeBubble);
            activeBubble = null;
        }
    }

    private void LateUpdate()
    {
        if (activeBubble != null && targetTransform != null)
        {
            UpdateBubblePosition();
        }
    }

    private void UpdateBubblePosition()
    {
        if (targetTransform == null || activeBubble == null) return;

        Vector3 worldPosition = targetTransform.position + (Vector3)bubbleOffset;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        activeBubble.transform.position = screenPos;
    }
}
