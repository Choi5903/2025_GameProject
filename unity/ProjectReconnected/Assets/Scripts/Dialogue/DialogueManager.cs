using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private Queue<DialogueLine> sentences = new Queue<DialogueLine>();
    private System.Action onDialogueEnd;
    private bool isDialogueActive = false;
    public bool IsDialogueActive => isDialogueActive;

    private PlayerController player;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public void StartDialogue(DialogueData data, System.Action endCallback = null)
    {
        if (data == null || data.dialogueLines.Count == 0)
        {
            Debug.LogWarning("⚠️ DialogueData가 비어 있거나 없음!");
            return;
        }

        sentences.Clear();
        foreach (var line in data.dialogueLines)
        {
            sentences.Enqueue(line);
        }

        isDialogueActive = true;
        onDialogueEnd = endCallback;

        if (PlayerController.Instance != null)
            PlayerController.Instance.SetMovementEnabled(false);

        LockPlayer();
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = sentences.Dequeue();
        DialogueUIManager.Instance.ShowDialogue(line.speakerName, line.sentence, line.portrait);
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        DialogueUIManager.Instance.HideDialogue();

        if (PlayerController.Instance != null)
            PlayerController.Instance.SetMovementEnabled(true);

        onDialogueEnd?.Invoke();
        onDialogueEnd = null;
        UnlockPlayer();
    }

    private void LockPlayer()
    {
        if (player != null)
        {
            player.SetMovementEnabled(false);

            // ✅ 애니메이터 강제로 idle 설정
            Animator animator = player.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("isWalking", false);
            }
        }
    }

    private void UnlockPlayer()
    {
        if (player != null)
            player.SetMovementEnabled(true);
    }
}