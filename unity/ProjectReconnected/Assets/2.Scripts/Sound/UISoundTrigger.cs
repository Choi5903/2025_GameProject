using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundTrigger : MonoBehaviour
{
    [Header("UI ������Ʈ ȿ����")]
    public AudioClip onEnableSound;
    public AudioClip onDisableSound;

    private void OnEnable()
    {
        if (onEnableSound != null)
        {
            SFXManager.Instance?.PlaySound(onEnableSound);
        }
    }

    private void OnDisable()
    {
        if (onDisableSound != null)
        {
            SFXManager.Instance?.PlaySound(onDisableSound);
        }
    }
}
