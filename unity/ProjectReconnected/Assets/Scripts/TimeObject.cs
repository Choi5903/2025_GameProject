
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeObject : MonoBehaviour
{
    public TimeState activeState = TimeState.Present;

    [Header("상호작용으로 비활성화된 경우")]
    public bool overrideActive = false;

    public void UpdateState(TimeState current)
    {
        if (overrideActive)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(current == activeState);
    }
}
