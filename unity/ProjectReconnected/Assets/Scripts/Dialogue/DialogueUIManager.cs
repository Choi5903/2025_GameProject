using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUIManager : MonoBehaviour
{
    public static DialogueUIManager Instance;

    [Header("대화 UI")]
    public GameObject dialoguePanel;
    public TMP_Text speakerNameText;
    public TMP_Text dialogueText;
    public Image speakerImage;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowDialogue(string speakerName, string sentence, Sprite portrait = null)
    {
        dialoguePanel.SetActive(true);
        speakerNameText.text = speakerName;
        dialogueText.text = sentence;
        if (portrait != null)
            speakerImage.sprite = portrait;
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}