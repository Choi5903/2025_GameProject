
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeObject : MonoBehaviour
{
    public TimeState activeState = TimeState.Present;

    public void UpdateState(TimeState current)
    {
        bool shouldBeActive = (activeState == current);
        gameObject.SetActive(shouldBeActive);
        Debug.Log($"🔄 {gameObject.name} (시간대: {activeState}) → 현재 시간대: {current}, 활성화 여부: {shouldBeActive}");
    }

}
