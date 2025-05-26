using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MessengerDialogueManager : MonoBehaviour
{
    public static MessengerDialogueManager Instance;

    [Header("UI 연결")]
    public GameObject messengerPanel;
    public RectTransform chatContentArea;
    public ScrollRect scrollRect;
    public GameObject leftMessagePrefab;
    public GameObject rightMessagePrefab;

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

        if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
        {
            DisplayNextChat();
        }
    }

    public void StartDialogue(MessengerDialogueData data, System.Action endCallback = null)
    {
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
        DisplayNextChat();
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
        GameObject prefab = (line.position == ChatPosition.Left) ? leftMessagePrefab : rightMessagePrefab;
        GameObject chatInstance = Instantiate(prefab, chatContentArea);

        // 모든 자식 강제 활성화 (혹시라도 비활성 상태로 저장된 경우)
        Transform[] children = chatInstance.GetComponentsInChildren<Transform>(true);
        foreach (var child in children)
        {
            child.gameObject.SetActive(true);
        }

        // 안전하게 텍스트 찾기
        TMP_Text messageText = chatInstance.GetComponentInChildren<TMP_Text>(true);
        if (messageText != null)
        {
            messageText.text = line.message;
        }

        // 첨부 이미지 처리
        Image[] allImages = chatInstance.GetComponentsInChildren<Image>(true);
        foreach (var img in allImages)
        {
            if (img.name == "AttachmentImage")
            {
                if (line.attachmentImage != null)
                {
                    img.sprite = line.attachmentImage;
                    img.gameObject.SetActive(true);
                }
                else
                {
                    img.gameObject.SetActive(false);
                }
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContentArea);
        ScrollToBottom();

        isWaitingForInput = true; // 다음 입력 대기
    }

    private void EndDialogue()
    {
        messengerPanel.SetActive(false);
        isWaitingForInput = false;
        onDialogueEnd?.Invoke();
        onDialogueEnd = null;
    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
