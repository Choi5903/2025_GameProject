using System.Collections;
using UnityEngine;
using System;

public class PlayerTeleportEvent : MonoBehaviour, IBeginEvent
{
    [Header("이동 대상 플레이어")]
    public PlayerController player;

    [Header("이동 위치 앵커")]
    public Transform targetPosition;

    [Header("지연 시간 (초)")]
    public float delay = 0.8f;

    [Header("이동 후 방향 (선택)")]
    public bool faceRightAfterMove = true;

    public void TriggerEvent(Action onComplete = null)
    {
        if (player == null || targetPosition == null)
        {
            Debug.LogWarning("❌ Player 또는 TargetPosition이 설정되지 않았습니다.");
            onComplete?.Invoke(); // 실패해도 콜백은 호출
            return;
        }

        StartCoroutine(DelayedTeleport(onComplete));
    }

    private IEnumerator DelayedTeleport(Action onComplete)
    {
        yield return new WaitForSeconds(delay);

        player.transform.position = targetPosition.position;

        // 방향 설정
        float scaleX = Mathf.Abs(player.transform.localScale.x);
        player.transform.localScale = new Vector3(
            faceRightAfterMove ? scaleX : -scaleX,
            player.transform.localScale.y,
            player.transform.localScale.z
        );

        Debug.Log($"🟢 플레이어를 {targetPosition.position} 위치로 텔레포트 완료.");

        onComplete?.Invoke();
    }
}
