using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class BottomDialogueManager : MonoBehaviour
{
    public static BottomDialogueManager Instance;
    private UIStorySceneManager manager;

    [Header("UI 연결")]
    public GameObject dialogueRoot;           // 전체 BottomDialogue GameObject
    public Image backgroundImage;             // 화면 전체 어둡게
    public GameObject dialoguePanel;          // 하단 Panel
    public TMP_Text speakerNameText;
    public TMP_Text dialogueText;

    [Header("타이핑 효과")]
    public float typingSpeed = 0.05f;

    private Queue<BottomDialogueLine> dialogueLines = new Queue<BottomDialogueLine>();
    private System.Action onDialogueEnd;
    private bool isTyping = false;
    private bool isActive = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialogueRoot.SetActive(false); // 시작 시 비활성화
    }
    private void Update()
    {
        if (!isActive) return;

        if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
        {
            DisplayNextLine();
        }

        // ✅ 자동 종료 감지
        if (dialogueLines.Count == 0 && !isTyping)
        {
            EndDialogue();
        }
    }

    public void StartDialogue(BottomDialogueData data, System.Action endCallback = null)
    {
        if (data == null || data.dialogueLines.Count == 0)
        {
            Debug.LogWarning("⚠️ 대화 데이터가 비어 있음");
            endCallback?.Invoke();
            return;
        }

        dialogueRoot.SetActive(true);
        isActive = true;

        dialogueLines.Clear();
        foreach (var line in data.dialogueLines)
        {
            dialogueLines.Enqueue(line);
        }

        onDialogueEnd = endCallback;
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        SFXManager.Instance.PlayButtonClick1();
        if (isTyping)
        {
            // 타이핑 중이라면 즉시 완성
            StopAllCoroutines();
            dialogueText.maxVisibleCharacters = dialogueText.text.Length;
            isTyping = false;
            return;
        }

        if (dialogueLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        var line = dialogueLines.Dequeue();
        UpdateUI(line);
        StartCoroutine(TypeSentence(line.sentence));
    }

    private void UpdateUI(BottomDialogueLine line)
    {
        speakerNameText.text = line.speakerName;
        dialogueText.text = "";

        if (backgroundImage != null)
        {
            if (line.backgroundImage != null)
            {
                backgroundImage.sprite = line.backgroundImage;
                backgroundImage.gameObject.SetActive(true);
            }
            else
            {
                backgroundImage.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.maxVisibleCharacters = 0;
        dialogueText.text = sentence;

        for (int i = 0; i <= sentence.Length; i++)
        {
            dialogueText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void EndDialogue()
    {
        dialogueRoot.SetActive(false);
        isActive = false;
        onDialogueEnd?.Invoke();
        onDialogueEnd = null;

        OnDialogueEnd();
        UIStorySceneManager.Instance?.ForceTriggerFrom(this);

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
