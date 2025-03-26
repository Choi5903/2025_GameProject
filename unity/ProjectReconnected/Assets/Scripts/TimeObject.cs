
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeObject : MonoBehaviour
{
    public TimeState activeState = TimeState.Present;

    [Header("상호작용으로 비활성화된 경우")]
    public bool overrideActive = false;

    private void Awake()
    {
        // 보강: 씬 로드 직후 바로 비활성화 반영
        if (overrideActive)
        {
            Debug.Log($"[TimeObject:Awake] {gameObject.name} → overrideActive 적용됨 → 강제 비활성화");
            gameObject.SetActive(false);
        }
    }

    public void UpdateState(TimeState current)
    {
        if (overrideActive)
        {
            Debug.Log($"[TimeObject:UpdateState] {gameObject.name} → overrideActive 적용 → 비활성화 유지");
            gameObject.SetActive(false);
            return;
        }

        bool shouldBeActive = (current == activeState);
        gameObject.SetActive(shouldBeActive);
        Debug.Log($"[TimeObject:UpdateState] {gameObject.name} → {current} 기준 {(shouldBeActive ? "활성화" : "비활성화")}");
    }
}
