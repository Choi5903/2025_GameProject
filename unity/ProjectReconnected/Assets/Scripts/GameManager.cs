using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("게임 데이터")]
    public GameData gameData = new GameData();

    [Header("시간 상태")]
    public TimeState currentTimeState = TimeState.Present;
    public List<TimeState> availableTimeStates = new List<TimeState> { TimeState.Past, TimeState.Present };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        TimeObjectManager.Instance?.UpdateStates(currentTimeState);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) ShiftTime(-1);
        else if (Input.GetKeyDown(KeyCode.E)) ShiftTime(1);
    }

    public void ShiftTime(int direction)
    {
        int currentIndex = availableTimeStates.IndexOf(currentTimeState);
        int newIndex = currentIndex + direction;

        if (newIndex < 0 || newIndex >= availableTimeStates.Count)
        {
            Debug.Log("🚫 이동 불가한 시점입니다.");
            return;
        }

        TimeState nextTimeState = availableTimeStates[newIndex];
        Debug.Log($"🕒 시점 변환됨! {currentTimeState} → {nextTimeState}");

        // 1️⃣ 위치 복사 먼저 수행 (비활성화 되기 전에)
        foreach (var obj in FindObjectsOfType<TimeSyncedObject>(true)) // 비활성 오브젝트 포함
        {
            obj.SyncIfNeeded(nextTimeState);
        }

        // 2️⃣ 시점 상태 변경
        currentTimeState = nextTimeState;

        // 3️⃣ 오브젝트 활성/비활성 상태 업데이트
        TimeObjectManager.Instance?.UpdateStates(currentTimeState);
    }

    public void ChangeRestoration(float amount)
    {
        gameData.restorationRate = Mathf.Clamp(gameData.restorationRate + amount, 0f, 100f);
        Debug.Log($"📊 복원율 변경됨: {gameData.restorationRate}%");
    }

    public void ChangeMemoryClues(int amount)
    {
        gameData.memoryClues += amount;
        Debug.Log($"🧩 기억 단서 변경됨: {gameData.memoryClues}개");
    }
}
