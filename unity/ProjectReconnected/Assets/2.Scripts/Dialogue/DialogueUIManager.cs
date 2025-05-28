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
    public Image leftCharacterImage;
    public Image rightCharacterImage;
    public Image extraImage;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowDialogue(string speakerName, string sentence,
                             Sprite leftCG, Sprite rightCG, Sprite extra)
    {
        dialoguePanel.SetActive(true);
        speakerNameText.text = speakerName;
        dialogueText.text = sentence;

        if (leftCharacterImage != null)
            leftCharacterImage.sprite = leftCG;

        if (rightCharacterImage != null)
            rightCharacterImage.sprite = rightCG;

        if (extraImage != null)
            extraImage.sprite = extra;
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
