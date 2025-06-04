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
    public MonoBehaviour eventSource; // BottomDialogueManager ��
    public GameObject targetEventObject; // ���� ������ ���
}

[System.Serializable]
public class UIButtonMethodPair
{
    public Button button;
    public MonoBehaviour targetScript;
    public string methodName; // public void �޼����
}

public class UIStorySceneManager : MonoBehaviour
{
    [Header("�� ���� �� ������ �̺�Ʈ ������Ʈ��")]
    public List<GameObject> beginEventObjects;

    [Header("��ư + �̺�Ʈ ������Ʈ ����")]
    public List<UIButtonEventPair> buttonEventPairs;

    [Header("���� �� �̾����� �̺�Ʈ")]
    public List<EventCompletePair> eventCompleteMappings;

    [Header("��ư + �� ��ȯ ����")]
    public List<UIButtonScenePair> buttonScenePairs;

    public static UIStorySceneManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // ���� �ڵ� ����
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
        Debug.Log($"[UIStorySceneManager] �̺�Ʈ ���� ����: {source}");
        foreach (var pair in eventCompleteMappings)
        {
            if (pair.eventSource == source)
            {
                Debug.Log($"�� ���� ���: {pair.targetEventObject.name}");
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
        Debug.Log($"[���� Ʈ����] ��û��: {source}");

        foreach (var pair in eventCompleteMappings)
        {
            if (pair.eventSource == source)
            {
                Debug.Log($"[���� Ʈ����] ��Ī�� �̺�Ʈ ����: {pair.targetEventObject.name}");
                if (pair.targetEventObject.TryGetComponent(out IBeginEvent evt))
                {
                    evt.TriggerEvent();
                }
            }
        }
    }

}
