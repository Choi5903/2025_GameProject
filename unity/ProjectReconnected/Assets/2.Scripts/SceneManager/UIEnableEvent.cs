using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnableEvent : MonoBehaviour, IBeginEvent, IStoryEventNotifier
{
    public List<GameObject> targets;
    public bool setActive = true;
    private UIStorySceneManager manager;

    public void TriggerEvent(System.Action onComplete = null)
    {
        foreach (var go in targets)
            if (go != null) go.SetActive(setActive);

        onComplete?.Invoke();
        manager?.NotifyEventCompleted(this);
    }

    public void RegisterManager(UIStorySceneManager m)
    {
        manager = m;
    }
}
