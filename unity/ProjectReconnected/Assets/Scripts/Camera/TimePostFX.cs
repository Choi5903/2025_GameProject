
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimePostFX : MonoBehaviour
{
    [Header("포스트 프로세싱 설정")]
    public Volume postVolume;

    private ColorAdjustments colorAdjustments;
    private LensDistortion lensDistortion;
    private ChromaticAberration chromaticAberration;

    private void Start()
    {
        if (postVolume.profile.TryGet(out colorAdjustments))
        {
            Debug.Log("✅ ColorAdjustments 연결됨");
        }

        if (postVolume.profile.TryGet(out lensDistortion))
        {
            Debug.Log("✅ LensDistortion 연결됨");
        }

        if (postVolume.profile.TryGet(out chromaticAberration))
        {
            Debug.Log("✅ ChromaticAberration 연결됨");
        }
    }

    public void SetPastVisual()
    {
        if (colorAdjustments != null)
            colorAdjustments.saturation.value = -100f;
    }

    public void SetPresentVisual()
    {
        if (colorAdjustments != null)
            colorAdjustments.saturation.value = 0f;
    }

    public void PlayGlitchEffect(float duration = 0.4f)
    {
        StartCoroutine(GlitchCoroutine(duration));
    }

    private IEnumerator GlitchCoroutine(float duration)
    {
        if (lensDistortion != null) lensDistortion.intensity.value = -0.4f;
        if (chromaticAberration != null) chromaticAberration.intensity.value = 1f;

        yield return new WaitForSeconds(duration);

        if (lensDistortion != null) lensDistortion.intensity.value = 0f;
        if (chromaticAberration != null) chromaticAberration.intensity.value = 0f;
    }
}
