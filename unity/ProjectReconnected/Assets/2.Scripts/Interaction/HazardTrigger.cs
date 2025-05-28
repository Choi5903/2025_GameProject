
using UnityEngine;
using UnityEngine.SceneManagement;

public class HazardTrigger : MonoBehaviour
{
    [Header("복원율 감소")]
    public bool reduceRestoration = false;
    public float restorationAmount = -10f;

    [Header("스테이지 초기화")]
    public bool resetScene = false;

    [Header("특정 위치로 이동")]
    public bool movePlayer = false;
    public Vector3 targetPosition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            Debug.Log("⚠️ 충돌한 대상이 Player가 아님: " + other.name);
            return;
        }

        Debug.Log("💥 위험 오브젝트와 충돌 감지됨: " + gameObject.name);
        SFXManager.Instance.PlayHitSound();

        if (reduceRestoration)
        {
            if (GameManager.Instance != null)
            {
                Debug.Log($"🧨 복원율 감소 시도: {restorationAmount}");
                GameManager.Instance.ChangeRestoration(restorationAmount);
            }
            else
            {
                Debug.LogError("❌ GameManager.Instance가 null입니다!");
            }
        }

        if (movePlayer)
        {
            Debug.Log("🚀 플레이어 이동: " + targetPosition);
            other.transform.position = targetPosition;
        }

        if (resetScene)
        {
            Debug.Log("🔄 씬 리셋 실행");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
