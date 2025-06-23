using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatController : MonoBehaviour
{
    private static CheatController instance;

    [Header("플레이어 참조")]
    public PlayerController player;

    [Header("텔레포트 앵커 (1~0 키 순서대로 최대 10개)")]
    public Transform[] teleportAnchors = new Transform[10];

    [Header("텔레포트 시 방향")]
    public bool faceRightAfterTeleport = true;

    [Header("복원율 변경량")]
    public float restorationStep = 10f;

    [Header("씬 이동 치트 (B/N/M 키)")]
    public string sceneNameV;
    public string sceneNameB;
    public string sceneNameN;
    public string sceneNameM;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (player == null) return;

        HandleTeleportKeys();
        HandleRestorationKeys();
        HandleSceneResetKey();
        HandleSceneShortcutKeys();
    }

    private void HandleTeleportKeys()
    {
        for (int i = 0; i <= 8; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                TeleportToAnchor(i);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            TeleportToAnchor(9);
        }
    }

    private void HandleRestorationKeys()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            GameManager.Instance?.ChangeRestoration(-restorationStep);
        }

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            GameManager.Instance?.ChangeRestoration(restorationStep);
        }
    }

    private void HandleSceneResetKey()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Debug.Log("🔁 씬 리셋 실행");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void HandleSceneShortcutKeys()
    {
        if (Input.GetKeyDown(KeyCode.V) && !string.IsNullOrEmpty(sceneNameB))
        {
            Debug.Log($"🔁 B 키 → 씬 이동: {sceneNameB}");
            SceneManager.LoadScene(sceneNameV);
        }
        
        if (Input.GetKeyDown(KeyCode.B) && !string.IsNullOrEmpty(sceneNameB))
        {
            Debug.Log($"🔁 B 키 → 씬 이동: {sceneNameB}");
            SceneManager.LoadScene(sceneNameB);
        }

        if (Input.GetKeyDown(KeyCode.N) && !string.IsNullOrEmpty(sceneNameN))
        {
            Debug.Log($"🔁 N 키 → 씬 이동: {sceneNameN}");
            SceneManager.LoadScene(sceneNameN);
        }

        if (Input.GetKeyDown(KeyCode.M) && !string.IsNullOrEmpty(sceneNameM))
        {
            Debug.Log($"🔁 M 키 → 씬 이동: {sceneNameM}");
            SceneManager.LoadScene(sceneNameM);
        }
    }

    private void TeleportToAnchor(int index)
    {
        if (index < 0 || index >= teleportAnchors.Length) return;

        Transform anchor = teleportAnchors[index];
        if (anchor == null)
        {
            Debug.LogWarning($"❌ 텔레포트 앵커 {index + 1} 이 비어있습니다.");
            return;
        }

        player.transform.position = anchor.position;

        float scaleX = Mathf.Abs(player.transform.localScale.x);
        player.transform.localScale = new Vector3(
            faceRightAfterTeleport ? scaleX : -scaleX,
            player.transform.localScale.y,
            player.transform.localScale.z
        );

        Debug.Log($"🟢 플레이어 {index + 1}번 위치로 이동됨: {anchor.position}");
    }
}
