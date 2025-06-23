using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform2D : MonoBehaviour
{
    public enum MoveDirection { Horizontal, Vertical }

    [Header("이동 설정")]
    public MoveDirection direction = MoveDirection.Horizontal; // 횡/종 방향
    public float moveDistance = 5f;   // 이동 거리
    public float moveSpeed = 2f;      // 이동 속도 (유닛/초)
    public bool invertDirection = false; // 방향 반전 토글

    [Header("단방향 설정")]
    public bool oneWay = false; // 단방향 모드 사용 여부
    public float respawnDelay = 2f; // 다시 나타날 때까지 대기 시간

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 targetPos;
    private bool isMoving = true;
    private bool isOneWayTriggered = false;

    void Start()
    {
        startPos = transform.position;

        Vector3 offset = (direction == MoveDirection.Horizontal)
            ? new Vector3(moveDistance, 0f, 0f)
            : new Vector3(0f, moveDistance, 0f);

        if (invertDirection)
        {
            offset *= -1f;
        }

        endPos = startPos + offset;
        targetPos = endPos;
    }

    void Update()
    {
        if (!isMoving || moveSpeed <= 0f) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            if (oneWay && !isOneWayTriggered && targetPos == endPos)
            {
                StartCoroutine(OneWayResetCycle());
                isOneWayTriggered = true;
            }
            else if (!oneWay)
            {
                targetPos = (targetPos == endPos) ? startPos : endPos;
            }
        }
    }

    private IEnumerator OneWayResetCycle()
    {
        isMoving = false;
        yield return new WaitForSeconds(respawnDelay);
        transform.position = startPos;
        targetPos = endPos;
        isMoving = true;
        isOneWayTriggered = false;
    }
    private IEnumerator OneWayRoutine()
    {
        isMoving = false; // 일단 멈춤
        yield return new WaitForSeconds(respawnDelay); // 설정된 시간 대기
        transform.position = startPos; // 시작 위치로 복귀
        targetPos = endPos;            // 다음 목표 위치 설정
        isMoving = true;               // 이동 재개
    }

    public void ResetToStartPosition()
    {
        transform.position = startPos;
        targetPos = endPos;

        if (oneWay)
        {
            isMoving = true;
            StartCoroutine(OneWayRoutine());
        }
    }

}
