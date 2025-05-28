using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum UIEffectType
{
    FadeInOut,
    ScreenGlitch,
    CameraShake
}

public class UIEffectEvent : MonoBehaviour, IBeginEvent, IStoryEventNotifier
{
    public UIEffectType effectType;

    [Header("����")]
    public float duration = 0.5f;

    [Header("���̵� ����")]
    public CanvasGroup fadeCanvasGroup;

    [Header("�۸�ġ ����")]
    public Image glitchImageRenderer;
    public Sprite[] glitchSprites;
    public float glitchInterval = 0.05f;

    [Header("ī�޶� ��鸲 ����")]
    public RectTransform shakeTarget;
    public float shakeIntensity = 0.2f;
    public float shakeSpeed = 50f;

    [Header("ȿ����")]
    [SerializeField] private AudioClip effectSFX;

    private UIStorySceneManager manager;

    public void RegisterManager(UIStorySceneManager m)
    {
        manager = m;
    }
    public void TriggerEvent(System.Action onComplete = null)
    {
        switch (effectType)
        {
            case UIEffectType.FadeInOut:
                SFXManager.Instance.PlayFadeSound();
                StartCoroutine(FadeInOutRoutine(onComplete));
                break;
            case UIEffectType.ScreenGlitch:
                SFXManager.Instance.PlayGlitchSound();
                StartCoroutine(GlitchRoutine(onComplete));
                break;
            case UIEffectType.CameraShake:
                SFXManager.Instance.PlayShakeSound();
                StartCoroutine(UIPanelShakeRoutine(onComplete)); // ����
                break;
        }
    }


    IEnumerator FadeInOutRoutine(System.Action onComplete)
    {
        float fadeInTime = duration * 0.6f;
        float fadeOutTime = duration * 0.4f;

        float t = 0f;
        while (t < fadeInTime)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeInTime);
            t += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;
        t = 0f;

        while (t < fadeOutTime)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeOutTime);
            t += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;
        onComplete?.Invoke();
        manager?.NotifyEventCompleted(this);
    }
    IEnumerator GlitchRoutine(System.Action onComplete)
    {
        SFXManager.Instance.PlaySound(effectSFX);
        float elapsed = 0f;
        glitchImageRenderer.enabled = true;

        while (elapsed < duration)
        {
            int index = Random.Range(0, glitchSprites.Length);
            glitchImageRenderer.sprite = glitchSprites[index];
            //glitchImageRenderer.SetNativeSize();

            glitchImageRenderer.color = new Color(1f, 1f, 1f, 1f); // ���̰�
            yield return new WaitForSeconds(glitchInterval * 0.5f); // ���̴� �ð�

            glitchImageRenderer.color = new Color(1f, 1f, 1f, 0f); // ����
            yield return new WaitForSeconds(glitchInterval * 0.5f); // ����� �ð�

            elapsed += glitchInterval;
        }

        glitchImageRenderer.enabled = false;
        onComplete?.Invoke();
        manager?.NotifyEventCompleted(this);
    }
    
    IEnumerator UIPanelShakeRoutine(System.Action onComplete)
    {
        Vector2 originalPos = shakeTarget.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * shakeIntensity * 100f;
            float offsetY = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * shakeIntensity * 100f;
            shakeTarget.anchoredPosition = originalPos + new Vector2(offsetX, offsetY);

            elapsed += Time.deltaTime;
            yield return null;
        }

        shakeTarget.anchoredPosition = originalPos;
        onComplete?.Invoke();
        manager?.NotifyEventCompleted(this);
    }
}
