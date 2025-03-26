
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineCameraSetup : MonoBehaviour
{
    [Header("카메라 대상")]
    public Transform player;

    [Header("맵 경계 제한")]
    public float minX = -10f;
    public float maxX = 10f;

    private CinemachineVirtualCamera vcam;
    private CinemachineFramingTransposer framing;

    private void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        if (vcam != null && player != null)
        {
            vcam.Follow = player;

            framing = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (framing != null)
            {
                framing.m_DeadZoneHeight = 2f;
                framing.m_DeadZoneWidth = 0.2f;
                framing.m_ScreenY = 0.5f; // 수직 고정
                framing.m_YDamping = 0f;  // Y축 흔들림 방지
            }
        }
    }

    private void LateUpdate()
    {
        if (vcam != null && vcam.Follow != null)
        {
            Vector3 pos = vcam.Follow.position;
            float clampedX = Mathf.Clamp(pos.x, minX, maxX);
            Vector3 cameraTargetPos = new Vector3(clampedX, transform.position.y, transform.position.z);
            transform.position = cameraTargetPos;
        }
    }
}
