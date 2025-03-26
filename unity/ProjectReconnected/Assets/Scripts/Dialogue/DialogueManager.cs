
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private Queue<DialogueLine> dialogueQueue = new Queue<DialogueLine>();
    private bool isDialogueActive = false;
    private PlayerController player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public void StartDialogue(DialogueData dialogueData)
    {
        if (isDialogueActive || dialogueData == null) return;

        isDialogueActive = true;
        dialogueQueue.Clear();

        foreach (DialogueLine line in dialogueData.dialogueLines)
        {
            dialogueQueue.Enqueue(line);
        }

        LockPlayer();
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = dialogueQueue.Dequeue();
        DialogueUIManager.Instance.ShowDialogue(line.speakerName, line.sentence, line.portrait);
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        DialogueUIManager.Instance.HideDialogue();
        UnlockPlayer();
    }

    private void LockPlayer()
    {
        if (player != null)
            player.SetMovementEnabled(false);
    }

    private void UnlockPlayer()
    {
        if (player != null)
            player.SetMovementEnabled(true);
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}
