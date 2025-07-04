﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneTimeStateMapping
{
    public string sceneName;
    public TimeState startTimeState;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("게임 데이터")]
    public GameData gameData = new GameData();

    [Header("초기 복원율")]
    public float initialRestorationRate = 40f;
    [Header("초기 복원율 설정")]
    public bool useInitialRestorationRate = true;  // ✅ 토글 추가
    

    [Header("시간 상태")]
    public TimeState currentTimeState = TimeState.Present;
    public List<TimeState> availableTimeStates = new List<TimeState> { TimeState.Past, TimeState.Present };

    [Header("시점 전환 페널티")]
    public float restorationPenaltyOnTimeShift = 5f;

    [Header("시점 전환 허용 여부")]
    public bool canShiftTime = false; // 기본값 false

    [Header("복원율 임계값")]
    public float restorationThreshold = 20f;

    [Header("씬별 시작 시간 설정")]
    public List<SceneTimeStateMapping> sceneTimeMappings;
    private Dictionary<string, TimeState> sceneTimeDict = new Dictionary<string, TimeState>();

    [Header("미니게임 진행 상태")]
    public bool isMiniGamePlaying = false;

    [Header("글리치 효과 연결")]
    public UIEffectEvent glitchEffectEvent; //글리치 효과 연결
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            foreach (var mapping in sceneTimeMappings)
            {
                if (!sceneTimeDict.ContainsKey(mapping.sceneName))
                {
                    sceneTimeDict.Add(mapping.sceneName, mapping.startTimeState);
                }
            }
        }
        else Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        InitializeGameState();
    }

    public void SetMiniGamePlaying(bool value)
    {
        isMiniGamePlaying = value;

        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.SetMovementEnabled(!value);

            if (value)
            {
                Animator animator = player.GetComponent<Animator>();
                if (animator != null)
                    animator.SetBool("isWalking", false);
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (sceneTimeDict.TryGetValue(scene.name, out TimeState startState))
        {
            currentTimeState = startState;
        }
        else
        {
            currentTimeState = TimeState.Present;
        }

        InitializeGameState();
    }

    private void InitializeGameState()
    {
        if (useInitialRestorationRate)
            gameData.restorationRate = initialRestorationRate;

        TimeObjectManager.Instance?.UpdateStates(currentTimeState);
        GameUIManager.Instance?.UpdateRestorationUI(gameData.restorationRate);
        GameUIManager.Instance?.UpdateClueUI(gameData.memoryClues);
    }

    private void Update()
    {
        if (!canShiftTime) return; // ✅ 시점 전환 비활성화 시 무시

        if (Input.GetKeyDown(KeyCode.Q)) ShiftTime(-1);
        else if (Input.GetKeyDown(KeyCode.E)) ShiftTime(1);
        else if (Input.GetKeyDown(KeyCode.R)) ChangeRestoration(30);
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
        SFXManager.Instance.PlayTimeShiftSound();
        glitchEffectEvent.TriggerEvent();
        Debug.Log($"🕒 시점 변환됨! {currentTimeState} → {nextTimeState}");

        foreach (var obj in FindObjectsOfType<TimeSyncedObject>(true))
        {
            obj.SyncIfNeeded(nextTimeState);
        }

        currentTimeState = nextTimeState;

        if (restorationPenaltyOnTimeShift > 0f)
        {
            ChangeRestoration(-restorationPenaltyOnTimeShift);
        }

        TimeObjectManager.Instance?.UpdateStates(currentTimeState);
    }

    public void ChangeRestoration(float amount)
    {
        float before = gameData.restorationRate;
        gameData.restorationRate = Mathf.Clamp(before + amount, 0f, 100f);
        float after = gameData.restorationRate;

        Debug.Log($"📊 복원율 변경: {before} → {after} (변경값: {amount})");
        GameUIManager.Instance?.UpdateRestorationUI(gameData.restorationRate);

        if (after <= restorationThreshold)
        {
            Debug.Log($"🛑 복원율 {after}% (기준: {restorationThreshold}%) → 초기화 조건 충족!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Debug.Log($"✅ 복원율이 {restorationThreshold}% 초과 → 씬 유지");
        }
    }

    public void ChangeMemoryClues(int amount)
    {
        gameData.memoryClues += amount;
        Debug.Log($"🧩 기억 단서 변경됨: {gameData.memoryClues}개");
        GameUIManager.Instance?.UpdateClueUI(gameData.memoryClues);
    }
}
