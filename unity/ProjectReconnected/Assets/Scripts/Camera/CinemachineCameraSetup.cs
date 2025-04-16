
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
    public float minY = -5f;
    public float maxY = 5f;
    public float cameraSmoothSpeed = 5f;


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
            Vector3 targetPos = vcam.Follow.position;

            float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
            float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);

            Vector3 desiredPosition = new Vector3(clampedX, clampedY, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, desiredPosition, cameraSmoothSpeed * Time.deltaTime);
        }
    }

}
