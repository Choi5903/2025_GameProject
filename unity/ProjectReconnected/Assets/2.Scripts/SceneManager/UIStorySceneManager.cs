using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class UIButtonEventPair
{
    public Button button;
    public GameObject targetEventObject;
}

[System.Serializable]
public class UIButtonScenePair
{
    public Button button;
    public string sceneName;
}

[System.Serializable]
public class EventCompletePair
{
    public MonoBehaviour eventSource; // BottomDialogueManager 등
    public GameObject targetEventObject; // 이후 실행할 대상
}

[System.Serializable]
public class UIButtonMethodPair
{
    public Button button;
    public MonoBehaviour targetScript;
    public string methodName; // public void 메서드명
}

public class UIStorySceneManager : MonoBehaviour
{
    [Header("씬 시작 시 실행할 이벤트 오브젝트들")]
    public List<GameObject> beginEventObjects;

    [Header("버튼 + 이벤트 오브젝트 연결")]
    public List<UIButtonEventPair> buttonEventPairs;

    [Header("종료 후 이어지는 이벤트")]
    public List<EventCompletePair> eventCompleteMappings;

    [Header("버튼 + 씬 전환 연결")]
    public List<UIButtonScenePair> buttonScenePairs;

    public static UIStorySceneManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 기존 코드 유지
        foreach (var pair in eventCompleteMappings)
        {
            if (pair.eventSource is IStoryEventNotifier notifier)
            {
                notifier.RegisterManager(this);
            }
        }
    }


    private void Start()
    {
        RunBeginEvents();
        BindButtonEvents();
    }

    private void RunBeginEvents()
    {
        foreach (var obj in beginEventObjects)
        {
            if (obj != null && obj.TryGetComponent(out IBeginEvent evt))
            {
                evt.TriggerEvent();
            }
        }
    }

    private void BindButtonEvents()
    {
        foreach (var pair in buttonEventPairs)
        {
            if (pair.button != null && pair.targetEventObject != null)
            {
                pair.button.onClick.AddListener(() =>
                {
                    if (pair.targetEventObject.TryGetComponent(out IBeginEvent evt))
                    {
                        evt.TriggerEvent();
                    }
                });
            }
        }

        foreach (var pair in buttonScenePairs)
        {
            if (pair.button != null && !string.IsNullOrEmpty(pair.sceneName))
            {
                pair.button.onClick.AddListener(() =>
                {
                    SceneManager.LoadScene(pair.sceneName);
                });
            }
        }
    }
    public void NotifyEventCompleted(MonoBehaviour source)
    {
        Debug.Log($"[UIStorySceneManager] 이벤트 종료 감지: {source}");
        foreach (var pair in eventCompleteMappings)
        {
            if (pair.eventSource == source)
            {
                Debug.Log($"→ 실행 대상: {pair.targetEventObject.name}");
                if (pair.targetEventObject.TryGetComponent(out IBeginEvent evt))
                {
                    evt.TriggerEvent();
                }
                break;
            }
        }
    }
    public void ForceTriggerFrom(MonoBehaviour source)
    {
        Debug.Log($"[강제 트리거] 요청됨: {source}");

        foreach (var pair in eventCompleteMappings)
        {
            if (pair.eventSource == source)
            {
                Debug.Log($"[강제 트리거] 매칭된 이벤트 실행: {pair.targetEventObject.name}");
                if (pair.targetEventObject.TryGetComponent(out IBeginEvent evt))
                {
                    evt.TriggerEvent();
                }
            }
        }
    }

}
