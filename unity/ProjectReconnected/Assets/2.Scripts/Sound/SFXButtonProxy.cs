using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXButtonProxy : MonoBehaviour
{
    public enum SoundType { ButtonClick1, ButtonClick2, DialogueNext }
    [Header("��ư Ŭ�� �� ����� ȿ���� ����")]
    public SoundType soundToPlay;

    public void PlaySound()
    {
        if (SFXManager.Instance == null) return;

        switch (soundToPlay)
        {
            case SoundType.ButtonClick1:
                SFXManager.Instance.PlayButtonClick1();
                break;
            case SoundType.ButtonClick2:
                SFXManager.Instance.PlayButtonClick2();
                break;
            case SoundType.DialogueNext:
                SFXManager.Instance.PlayDialogueNext();
                break;
        }
    }
}
