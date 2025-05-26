using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeShiftUnlockTrigger : MonoBehaviour
{
    public bool destroyOnTrigger = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🔓 시점 전환 해금됨!");
            GameManager.Instance.canShiftTime = true;

            if (destroyOnTrigger)
                Destroy(gameObject);
        }
    }
}
