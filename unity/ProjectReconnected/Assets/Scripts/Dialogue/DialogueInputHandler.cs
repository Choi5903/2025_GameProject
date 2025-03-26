using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInputHandler : MonoBehaviour
{
    private void Update()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive() && Input.GetKeyDown(KeyCode.F))
        {
            DialogueManager.Instance.DisplayNextSentence();
        }
    }
}
