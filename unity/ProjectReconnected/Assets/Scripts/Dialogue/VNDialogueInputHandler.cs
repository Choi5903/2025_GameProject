using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VNDialogueInputHandler : MonoBehaviour
{
    private void Update()
    {
        // ��ȭ ���̰� Ÿ���� ���� �ƴ� ���� ���� ��� ����
        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.IsDialogueActive &&
            Input.GetKeyDown(KeyCode.F))
        {
            DialogueManager.Instance.DisplayNextSentence();
        }
    }
}
