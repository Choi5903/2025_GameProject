using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeSyncedObject : MonoBehaviour
{
    public TimeState timeState = TimeState.Past;
    public Transform linkedObjectInNextTime;

    public void SyncIfNeeded(TimeState current)
    {
        Debug.Log($"🧪 SyncIfNeeded 호출됨 | 현재 시간대: {current}, 내 시간대: {timeState}");

        if (linkedObjectInNextTime == null)
        {
            Debug.LogWarning($"⚠️ {gameObject.name}의 linkedObjectInNextTime이 null입니다!");
            return;
        }

        bool isValidTransition =
            (timeState == TimeState.Past && current == TimeState.Present) ||
            (timeState == TimeState.Present && current == TimeState.Future);

        if (isValidTransition)
        {
            Vector3 sourcePos = transform.position;
            linkedObjectInNextTime.position = new Vector3(sourcePos.x, sourcePos.y, linkedObjectInNextTime.position.z);

            Debug.Log($"🔁 위치 강제 복사됨: {linkedObjectInNextTime.name} ← {linkedObjectInNextTime.position}");
        }
        else
        {
            Debug.Log($"⏩ 유효하지 않은 시간 전환: 복사 생략");
        }
    }
}
