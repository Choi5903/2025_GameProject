using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VNDialogueInputHandler : MonoBehaviour
{
    private void Update()
    {
        // 대화 중이고 타이핑 중이 아닐 때만 다음 대사 진행
        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.IsDialogueActive &&
            Input.GetKeyDown(KeyCode.F))
        {
            DialogueManager.Instance.DisplayNextSentence();
        }
    }
}
