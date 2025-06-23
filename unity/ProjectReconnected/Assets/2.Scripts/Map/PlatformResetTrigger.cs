using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformResetTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        MovingPlatform2D platform = other.GetComponent<MovingPlatform2D>();

        if (platform != null)
        {
            platform.ResetToStartPosition();
        }
    }
}
