using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFXOnEnable : MonoBehaviour
{
    public enum SFXType
    {
        None,
        ButtonClick1,
        ButtonClick2,
        WindowOpen,
        WindowClose,
        NoticeOn
    }

    [Header("활성화 시 재생할 효과음")]
    public SFXType selectedSFX = SFXType.None;

    private void OnEnable()
    {
        if (SFXManager.Instance == null) return;

        switch (selectedSFX)
        {
            case SFXType.ButtonClick1:
                SFXManager.Instance.PlayButtonClick1();
                break;
            case SFXType.ButtonClick2:
                SFXManager.Instance.PlayButtonClick2();
                break;
            case SFXType.WindowOpen:
                SFXManager.Instance.PlayWindowOpen();
                break;
            case SFXType.WindowClose:
                SFXManager.Instance.PlayWindowClose();
                break;
            case SFXType.NoticeOn:
                SFXManager.Instance.NoticeOn();
                break;
        }
    }
}
