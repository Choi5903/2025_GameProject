
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
        if (Input.GetKeyDown(KeyCode.Q)) ShiftTime(-1); // 왼쪽 (과거로)
        else if (Input.GetKeyDown(KeyCode.E)) ShiftTime(1); // 오른쪽 (미래로)
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

        currentTimeState = availableTimeStates[newIndex];
        Debug.Log($"🕒 시점 변환됨! 현재 시점: {currentTimeState}");

        TimeObjectManager.Instance?.UpdateStates(currentTimeState);
    }
}
