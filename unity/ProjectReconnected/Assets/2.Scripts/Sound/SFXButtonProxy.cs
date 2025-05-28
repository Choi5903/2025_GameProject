using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXButtonProxy : MonoBehaviour
{
    public enum SoundType { ButtonClick1, ButtonClick2, DialogueNext }
    [Header("버튼 클릭 시 재생할 효과음 유형")]
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
