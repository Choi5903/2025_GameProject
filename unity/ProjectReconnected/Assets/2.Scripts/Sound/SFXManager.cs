using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Header("전역 공통 효과음")]
    public AudioClip buttonClick1;
    public AudioClip buttonClick2;

    [Header("UI 효과음")]
    public AudioClip dialogueNext;
    public AudioClip WindowOpen;
    public AudioClip WindowClose;

    [Header("화면 연출 효과음")]
    public AudioClip glitchSound;
    public AudioClip fadeSound;
    public AudioClip shakeSound;

    [Header("퍼즐 씬 효과음")]
    public AudioClip TimeshiftSound;
    public AudioClip clueSound;
    public AudioClip switchSound;
    public AudioClip doorSound;

    [Header("플레이어 효과음")]
    public List<AudioClip> stepSounds;
    public AudioClip jumpSound;
    public AudioClip interactSound;
    public AudioClip hitSound;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayButtonClick1()
    {
        PlaySound(buttonClick1);
    }

    public void PlayButtonClick2()
    {
        PlaySound(buttonClick2);
    }

    public void PlayWindowOpen()
    {
        PlaySound(WindowOpen);
    }
    public void PlayWindowClose()
    {
        PlaySound(WindowClose);
    }

    public void PlayDialogueNext()
    {
        PlaySound(dialogueNext);
    }
    public void PlayFadeSound()
    {
        PlaySound(fadeSound);
    }

    public void PlayGlitchSound()
    {
        PlaySound(glitchSound);
    }

    public void PlayShakeSound()
    {
        PlaySound(shakeSound);
    }
    // 퍼즐 씬
    public void PlayTimeShiftSound()
    {
        PlaySound(TimeshiftSound);
    }

    public void PlayClueSound()
    {
        PlaySound(clueSound);
    }

    public void PlaySwitchSound()
    {
        PlaySound(switchSound);
    }

    public void PlayDoorSound()
    {
        PlaySound(doorSound);
    }

    // 플레이어
    public void PlayRandomStepSound()
    {
        if (stepSounds != null && stepSounds.Count > 0)
        {
            int index = Random.Range(0, stepSounds.Count);
            PlaySound(stepSounds[index]);
        }
    }

    public void PlayJumpSound()
    {
        PlaySound(jumpSound);
    }

    public void PlayInteractSound()
    {
        PlaySound(interactSound);
    }

    public void PlayHitSound()
    {
        PlaySound(hitSound);
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}