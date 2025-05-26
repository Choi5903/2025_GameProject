using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionWithFade : MonoBehaviour
{
    [Header("이동할 씬 번호")]
    public int targetSceneIndex = 0;

    [Header("페이드 설정")]
    public Image fadeImage;           // 캔버스에 있는 검은 이미지
    public float fadeDuration = 1f;   // 페이드 시간

    private bool isTransitioning = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTransitioning) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log($"🚪 씬 전환 트리거 감지 → {targetSceneIndex}번 씬으로 전환 (페이드 포함)");
            StartCoroutine(FadeAndLoadScene());
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        isTransitioning = true;

        if (fadeImage != null)
        {
            float t = 0f;
            Color color = fadeImage.color;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Clamp01(t / fadeDuration);
                fadeImage.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }
        }

        SceneManager.LoadScene(targetSceneIndex);
    }
}