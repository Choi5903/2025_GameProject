
using UnityEngine;

public class TriggerDeactivateTarget : MonoBehaviour
{
    [Header("비활성화할 대상 오브젝트")]
    public GameObject target;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (target != null)
        {
            TimeObject t = target.GetComponent<TimeObject>();
            if (t != null)
            {
                t.overrideActive = true;
            }

            target.SetActive(false);
            Debug.Log($"🔻 {target.name} 비활성화됨 + override 적용");
        }
    }
}
