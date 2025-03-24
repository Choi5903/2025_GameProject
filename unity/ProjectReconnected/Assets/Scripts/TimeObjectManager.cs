using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeObjectManager : MonoBehaviour
{
    public static TimeObjectManager Instance;

    private TimeObject[] timeObjects;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateStates(TimeState current)
    {
        // 비활성화된 오브젝트까지 모두 포함하여 검색
        timeObjects = FindObjectsOfType<TimeObject>(true);
        Debug.Log($"🔍 시간 오브젝트 {timeObjects.Length}개 감지됨");

        foreach (var obj in timeObjects)
        {
            obj.UpdateState(current);
        }
    }
}
