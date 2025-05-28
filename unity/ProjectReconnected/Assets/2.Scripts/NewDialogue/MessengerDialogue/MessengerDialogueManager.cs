using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MessengerDialogueManager : MonoBehaviour
{
    public static MessengerDialogueManager Instance;
    private UIStorySceneManager manager;

    [Header("UI 연결")]
    public GameObject messengerPanel;
    public RectTransform chatContentArea;
    public ScrollRect scrollRect;

    [Header("메시지 프리팹 (좌/우, 소/중/대)")]
    public GameObject leftSmall;
    public GameObject leftMedium;
    public GameObject leftLarge;
    public GameObject rightSmall;
    public GameObject rightMedium;
    public GameObject rightLarge;

    private Queue<MessengerChatLine> chatLines = new Queue<MessengerChatLine>();
    private System.Action onDialogueEnd;
    private bool isWaitingForInput = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        messengerPanel.SetActive(false);
    }
    private void Update()
    {
        if (!messengerPanel.activeSelf || !isWaitingForInput) return;

        if (Input.GetMouseButtonDown(0))
        {
            DisplayNextChat();
            SFXManager.Instance.PlayButtonClick1();
        }

        // ✅ 자동 종료 감지
        if (chatLines.Count == 0)
        {
            EndDialogue();
        }
    }

    public void StartDialogue(MessengerDialogueData data, System.Action endCallback = null)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContentArea);

        if (data == null || data.chatLines.Count == 0)
        {
            Debug.LogWarning("⚠️ 메신저 대화 데이터가 비어 있습니다!");
            endCallback?.Invoke();
            return;
        }

        messengerPanel.SetActive(true);
        chatLines.Clear();
        foreach (var line in data.chatLines)
        {
            chatLines.Enqueue(line);
        }

        onDialogueEnd = endCallback;
        StartCoroutine(DelayFirstChat());
    }

    public void DisplayNextChat()
    {
        isWaitingForInput = false;

        if (chatLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        MessengerChatLine line = chatLines.Dequeue();
        GameObject prefab = GetBubblePrefab(line.position, line.sizeLevel);

        GameObject chatInstance = Instantiate(prefab, chatContentArea);
        chatInstance.transform.SetParent(chatContentArea, false);

        TMP_Text messageText = chatInstance.GetComponentInChildren<TMP_Text>(true);
        if (messageText != null)
        {
            messageText.text = line.message;
            messageText.alignment = (line.position == ChatPosition.Left) ? TextAlignmentOptions.Left : TextAlignmentOptions.Right;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)chatInstance.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContentArea);

        ScrollToBottom();
        isWaitingForInput = true;
    }

    private GameObject GetBubblePrefab(ChatPosition position, int sizeLevel)
    {
        sizeLevel = Mathf.Clamp(sizeLevel, 1, 3);
        return (position, sizeLevel) switch
        {
            (ChatPosition.Left, 1) => leftSmall,
            (ChatPosition.Left, 2) => leftMedium,
            (ChatPosition.Left, 3) => leftLarge,
            (ChatPosition.Right, 1) => rightSmall,
            (ChatPosition.Right, 2) => rightMedium,
            (ChatPosition.Right, 3) => rightLarge,
            _ => leftMedium
        };
    }
    private void EndDialogue()
    {
        isWaitingForInput = false;
        onDialogueEnd?.Invoke();
        onDialogueEnd = null;

        OnDialogueEnd(); // ✅ 추가
        UIStorySceneManager.Instance?.ForceTriggerFrom(this);

    }

    private IEnumerator DelayFirstChat()
    {
        yield return null;
        DisplayNextChat();
    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContentArea);
    }
    public void RegisterManager(UIStorySceneManager m)
    {
        manager = m;
    }

    private void OnDialogueEnd()
    {
        manager?.NotifyEventCompleted(this);
    }
}
